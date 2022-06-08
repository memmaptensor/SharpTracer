using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Logging;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core;

internal class Program
{
    private static async Task Main(string[] args)
    {
        const string folderPath = @"C:\Users\ASUS\Downloads";
        const string fileName = "pathtracing.png";
        string fullPath = Path.Combine(folderPath, fileName);
        const int maxDepth = 20;

        // Materials
        RoughMaterial materialGround = new(ColorHelper.FromRGBAF(0.8f, 0.8f, 0f));
        RoughMaterial materialCenter = new(ColorHelper.FromRGBAF(0.7f, 0.3f, 0.3f));
        MetalMaterial materialLeft = new(ColorHelper.FromRGBAF(0.8f, 0.8f, 0.8f));
        MetalMaterial materialRight = new(ColorHelper.FromRGBAF(0.8f, 0.6f, 0.2f));

        // World
        HittableGroup world = new();
        world.HittableList.Add(new Sphere(materialGround, new Vector3(0f, -100.5f, -1f), 100f));
        world.HittableList.Add(new Sphere(materialCenter, new Vector3(0f, 0f, -1f), 0.5f));
        world.HittableList.Add(new Sphere(materialLeft, new Vector3(-1f, 0f, -1f), 0.5f));
        world.HittableList.Add(new Sphere(materialRight, new Vector3(1f, 0f, -1f), 0.5f));

        // Camera
        Camera camera = new(800, 600, 1f, new Vector3(0f, 0f, 0f));

        // Render
        PNGRenderer png = new(camera.Width, camera.Height);

        // MT with each scanline
        List<Task> scanlineTasks = new();
        for (int y = 0; y < camera.Height; y++)
        {
            int Y = y;
            scanlineTasks.Add(Task.Run(() => CalculateScanline(new Random(), Y, camera, world, maxDepth, png)));
        }

        await Task.WhenAll(scanlineTasks);

        ConsoleLogger.Get().LogInfo("Done");
        png.WriteToFile(fullPath);
        ProcessStartInfo info = new(fullPath) {UseShellExecute = true};
        Process.Start(info);
    }

    private static void CalculateScanline(Random rng, int y, Camera camera, HittableGroup world, int maxDepth,
        IRenderer renderer)
    {
        for (int x = 0; x < camera.Width; x++)
        {
            Vector3 colorVec = Vector3.Zero;
            for (int s = 0; s < ColorHelper.SamplesPerPixel; s++)
            {
                float u = (x + rng.NextSingle()) / camera.Width;
                float v = (y + rng.NextSingle()) / camera.Height;
                Ray ray = camera.GetRay(u, v);
                colorVec += RayColor(ray, world, maxDepth);
            }

            renderer.SetPixel(x, y, ColorHelper.WriteColor(colorVec).ToColor());
        }

        ConsoleLogger.Get().LogInfo($"Scanlines remaining: {camera.Height - y}");
    }

    private static Vector3 RayColor(Ray ray, HittableGroup world, int stackDepth)
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
