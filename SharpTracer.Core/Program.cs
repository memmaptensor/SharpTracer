using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text.Json;
using SharpTracer.Core.Logging;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Scene;
using SharpTracer.Core.SceneCamera;
using SharpTracer.Core.Settings;
using SharpTracer.Core.Utility;
using SimpleImageIO;

namespace SharpTracer.Core;

internal class Program
{
    private static async Task Main(string[] args)
    {
        const string pathSettings = "settings.json";

        RenderSettings settings;
        try
        {
            var jsonText = await File.ReadAllTextAsync(pathSettings);
            settings = JsonSerializer.Deserialize<RenderSettings>(jsonText);
            if (settings is null)
            {
                throw new Exception();
            }
        }
        catch
        {
            ConsoleLogger.Get().LogError("Failed to load settings");
            throw;
        }

        var fullPath = Path.Combine(settings.FolderPath, settings.FileName);

        RenderOptions options = new() { MaxDepth = 5, SamplesPerPixel = 500, Gamma = /*0.8f*/ 1f };

        IScene scene = new TheNextWeekScene();
        // HittableGroup world = scene.Render();
        BvhNode world = new(scene.Render(), 0f, 1f);

        IEyeView eye = new TheNextWeekCamera();
        var camera = eye.GetCamera();

        // Should really be called scene ambient light and not background
        // ICameraBackground background = new SolidBackground(Color.LightBlue);
        ICameraBackground background = new SolidBackground(Color.Black);

        // Render
        RgbImage img = new(camera.Width, camera.Height);

        // Execute
        var sw = Stopwatch.StartNew();
        ConsoleLogger.Get().LogInfo("Start pathtracing");
        var scanlinesLeft = camera.Height;
        Parallel.For(0, camera.Height, new ParallelOptions { MaxDegreeOfParallelism = settings.NumTasks },
            y => CalculateScanline(y, camera, background, world, options.MaxDepth, img, options.Gamma,
                ref scanlinesLeft,
                options.SamplesPerPixel, settings.NumTasks));
        ConsoleLogger.Get().LogInfo("Done pathtracing");

        var image = img;
        if (settings.Denoise)
        {
            ConsoleLogger.Get().LogInfo("Start denoising");
            using (Denoiser denoiser = new())
            {
                image = denoiser.Denoise(img);
            }

            ConsoleLogger.Get().LogInfo("Done denoising");
        }

        ConsoleLogger.Get().LogInfo("Start writing to disk");
        image.WriteToFile(fullPath);
        ConsoleLogger.Get().LogInfo("Done writing to disk");
        sw.Stop();
        ConsoleLogger.Get().LogInfo($"Rendering done, time elapsed: {sw.Elapsed}");
        sw.Reset();

        ConsoleLogger.Get().LogInfo("Opening");
        ProcessStartInfo info = new(fullPath) { UseShellExecute = true };
        Process.Start(info);
        ConsoleLogger.Get().LogInfo("Done");
    }

    private static void CalculateScanline(
        int y,
        Camera camera,
        ICameraBackground background,
        IHittable world,
        int maxDepth,
        RgbImage renderer,
        float gamma,
        ref int scanlineCount,
        float samplesPerPixel,
        int numTasks)
    {
        Parallel.For(0, camera.Width, new ParallelOptions { MaxDegreeOfParallelism = numTasks },
            x =>
            {
                var colorVec = Vector3.Zero;
                for (var s = 0; s < samplesPerPixel; s++)
                {
                    var u = (x + Random.Shared.NextSingle()) / camera.Width;
                    var v = (y + Random.Shared.NextSingle()) / camera.Height;
                    var ray = camera.GetRay(u, v);
                    colorVec += RayColor(ray, world, maxDepth, background);
                }

                renderer.SetPixel(x, camera.Height - 1 - y, ColorHelper.WriteColor(colorVec, gamma, samplesPerPixel));
            });

        ConsoleLogger.Get().LogInfo($"Scanlines remaining: {scanlineCount--}");
    }

    private static Vector3 RayColor(Ray ray, IHittable world, int stackDepth, ICameraBackground background)
    {
        if (stackDepth <= 0)
        {
            return Vector3.Zero;
        }

        HitRecord hit = default;

        if (world.Hit(ray, 0.001f, float.PositiveInfinity, ref hit))
        {
            var emitted = hit.Material.Emitted(hit.UV, hit.Position);
            if (hit.Material.Scatter(ray, hit, out var attenuation, out var scattered))
            {
                return emitted + attenuation.ToVector3() * RayColor(scattered, world, stackDepth - 1, background);
            }

            return emitted;
        }

        return background.GetScreenSpaceColor(ray);
    }
}
