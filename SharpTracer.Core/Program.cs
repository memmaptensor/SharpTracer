using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text.Json;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Logging;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;
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
            string jsonText = await File.ReadAllTextAsync(pathSettings);
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

        string fullPath = Path.Combine(settings.FolderPath, settings.FileName);
        const int maxDepth = 20;

        HittableGroup world = new();

        // Ground
        RoughMaterial groundMaterial = new(ColorHelper.FromRGBAF(0.5f, 0.5f, 0.5f));
        world.HittableList.Add(new Sphere(groundMaterial, new Transform(new Vector3(0f, -1000f, 0f)), 1000f));
        // Random small spheres
        Random rng = new();
        for (int i = -11; i < 11; i++)
        for (int j = -11; j < 11; j++)
        {
            float materialProbability = rng.NextSingle();
            Vector3 center = new(i + 0.9f * rng.NextSingle(), 0.2f, j + 0.9f * rng.NextSingle());
            if ((center - new Vector3(4f, 0.2f, 0f)).LengthSquared() > 0.9f * 0.9f)
            {
                IMaterial material;
                Color albedo = ColorHelper.FromRandom(rng);
                if (materialProbability < 0.9f)
                {
                    material = new RoughMaterial(albedo);
                }
                else if (materialProbability < 0.95f)
                {
                    float fuzz = rng.NextSingle() * 0.5f;
                    material = new MetalMaterial(albedo, fuzz);
                }
                else
                {
                    material = new DielectricMaterial(Color.White, 1.5f);
                }

                Vector3 center2 = center + new Vector3(0f, 0.5f * rng.NextSingle(), 0f);
                world.HittableList.Add(
                    new MovingSphere(
                        material,
                        new Transform(center),
                        new Transform(center2, 1f),
                        0.2f));
            }
        }

        // Big spheres
        DielectricMaterial glassMaterial = new(Color.White, 1.5f);
        RoughMaterial roughMaterial = new(ColorHelper.FromRGBAF(0.4f, 0.2f, 0.1f));
        MetalMaterial metalMaterial = new(ColorHelper.FromRGBAF(0.7f, 0.6f, 0.5f), 0f);
        world.HittableList.Add(new Sphere(glassMaterial, new Transform(new Vector3(0f, 1f, 0f)), 1f));
        world.HittableList.Add(new Sphere(roughMaterial, new Transform(new Vector3(-4f, 1f, 0f)), 1f));
        world.HittableList.Add(new Sphere(metalMaterial, new Transform(new Vector3(4f, 1f, 0f)), 1f));

        // Camera
        Vector3 lookFrom = new(13f, 2f, 3f);
        Vector3 lookAt = new(0f, 0f, 0f);
        float fov = 20f;
        float distToFocus = 10f;
        float aperture = 0.02f;
        Camera camera = new(1920, 1080, lookFrom, lookAt, fov, aperture, distToFocus, 0f, 1f);

        // Render
        RgbImage img = new(camera.Width, camera.Height);
        const float gamma = 1.2f;

        // Spawn tasks with N scanlines
        int scanlinesLeft = camera.Height;
        int scanlinesPerTask = camera.Height / settings.NumTasks;
        int leftoverScanlines = camera.Height % settings.NumTasks;
        List<Task> scanlineTasks = new();
        for (int y = 0; y < camera.Height - leftoverScanlines; y += scanlinesPerTask)
        {
            int threadLocalY = y;
            scanlineTasks.Add(Task.Run(() =>
            {
                Random localRng = new();
                for (int scanline = threadLocalY; scanline < threadLocalY + scanlinesPerTask; scanline++)
                {
                    CalculateScanline(localRng, scanline, camera, world, maxDepth, img, gamma, ref scanlinesLeft);
                }
            }));
        }

        for (int y = camera.Height - leftoverScanlines; y < camera.Height; y++)
        {
            int threadLocalY = y;
            scanlineTasks.Add(Task.Run(() =>
            {
                Random localRng = new();
                CalculateScanline(localRng, threadLocalY, camera, world, maxDepth, img, gamma, ref scanlinesLeft);
            }));
        }

        // Execute
        Stopwatch sw = Stopwatch.StartNew();
        ConsoleLogger.Get().LogInfo("Start pathtracing");
        await Task.WhenAll(scanlineTasks);
        ConsoleLogger.Get().LogInfo("Done pathtracing");

        RgbImage image;
        if (settings.Denoise)
        {
            ConsoleLogger.Get().LogInfo("Start denoising");
            using (Denoiser denoiser = new())
            {
                image = denoiser.Denoise(img);
            }

            ConsoleLogger.Get().LogInfo("Done denoising");
        }
        else
        {
            image = img;
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
        Random rng,
        int y,
        Camera camera,
        IHittable world,
        int maxDepth,
        RgbImage renderer,
        float gamma,
        ref int scanlineCount)
    {
        for (int x = 0; x < camera.Width; x++)
        {
            Vector3 colorVec = Vector3.Zero;
            for (int s = 0; s < ColorHelper.SamplesPerPixel; s++)
            {
                float u = (x + rng.NextSingle()) / camera.Width;
                float v = (y + rng.NextSingle()) / camera.Height;
                Ray ray = camera.GetRay(rng, u, v);
                colorVec += RayColor(ray, world, maxDepth);
            }

            renderer.SetPixel(x, camera.Height - 1 - y, ColorHelper.WriteColor(colorVec, gamma));
        }

        ConsoleLogger.Get().LogInfo($"Scanlines remaining: {scanlineCount--}");
    }

    private static Vector3 RayColor(Ray ray, IHittable world, int stackDepth)
    {
        if (stackDepth <= 0)
        {
            return Vector3.Zero;
        }

        HitRecord? hit = world.HitIfExists(ray, 0.001f, float.PositiveInfinity);
        if (hit != null)
        {
            hit.Value.Material.Scatter(ray, hit.Value, out Color attenuation, out Ray outRay);
            return attenuation.ToVector3() * RayColor(outRay, world, stackDepth - 1);
        }

        Color topColor = Color.LightSkyBlue;
        Color bottomColor = Color.White;
        Vector3 dir = Vector3.Normalize(ray.Direction);
        float t = 0.5f * (dir.Y + 1f);
        return (1f - t) * bottomColor.ToVector3() + t * topColor.ToVector3();
    }
}
