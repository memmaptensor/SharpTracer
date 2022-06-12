using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Geometry.Plane;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Texture;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Scene;

public class TheNextWeekScene : IScene
{
    public HittableGroup Render()
    {
        HittableGroup world = new();

        RoughMaterial groundMat = new(ColorHelper.FromRGBAF(0.48f, 0.83f, 0.53f));
        const int boxesPerSide = 20;
        for (int i = 0; i < boxesPerSide; i++)
        for (int j = 0; j < boxesPerSide; j++)
        {
            float w = 100f;
            float x0 = -1000f + i * w;
            float z0 = -1000f + j * w;
            float y0 = 0f;
            float x1 = x0 + w;
            float y1 = Remap.Map(Random.Shared.NextSingle(), 0f, 1f, 1f, 101f);
            float z1 = z0 + w;

            Vector3 closest = new(x0, y0, z0);
            Vector3 farthest = new(x1, y1, z1);

            world.HittableList.Add(new Cube(groundMat, new GeometricTransform(
                (farthest + closest) / 2f,
                farthest - closest
            )));
        }

        DiffuseLightMaterial lightMat = new(new Vector3(7f, 7f, 7f));
        world.HittableList.Add(new YPlane(lightMat, new Vector2(123f, 147f), new Vector2(423f, 412f), 554f));
        Vector3 center1 = new(400f, 400f, 200f);
        Vector3 center2 = center1 + new Vector3(30f, 0f, 0f);
        RoughMaterial movingSphereMat = new(ColorHelper.FromRGBAF(0.7f, 0.3f, 0.1f));
        world.HittableList.Add(new MovingSphere(movingSphereMat, new GeometricTransform(center1),
            new GeometricTransform(center2, 1f), 50f));
        world.HittableList.Add(new Sphere(new DielectricMaterial(Color.White, 1.5f),
            new GeometricTransform(new Vector3(260f, 150f, 45f)), 50f));
        world.HittableList.Add(new Sphere(new MetalMaterial(ColorHelper.FromRGBAF(0.8f, 0.8f, 0.9f), 1f),
            new GeometricTransform(new Vector3(0f, 150f, 145f)), 50f));
        Sphere boundary = new(new DielectricMaterial(Color.White, 1.5f),
            new GeometricTransform(new Vector3(360f, 150f, 145f)), 70f);
        world.HittableList.Add(boundary);
        world.HittableList.Add(new ConstantMedium(boundary, 0.2f, ColorHelper.FromRGBAF(0.2f, 0.4f, 0.9f)));
        Sphere boundary2 = new(new DielectricMaterial(Color.White, 1.5f), new GeometricTransform(Vector3.Zero),
            5000f);
        world.HittableList.Add(new ConstantMedium(boundary2, 0.0001f, Color.White));
        TexturedMaterial earthMat = new(new ImageTexture("earthmap.jpg"));
        world.HittableList.Add(new Sphere(earthMat, new GeometricTransform(new Vector3(400f, 200f, 400f)), 100f));
        NoiseData noiseData = new(
            FastNoiseLite.NoiseType.Perlin,
            1337,
            0.001f,
            new Vector3(100f, 100f, 100f),
            new Vector3(0f, 0f, 0f),
            7);
        FastNoiseLite noise = new();
        TexturedMaterial marbleMat = new(new NoiseTexture(noise, noiseData, Color.White));
        world.HittableList.Add(new Sphere(marbleMat, new GeometricTransform(new Vector3(220f, 280f, 300f)), 80f));

        HittableGroup sphereBox = new();
        RoughMaterial whiteMat = new(ColorHelper.FromRGBAF(0.73f, 0.73f, 0.73f));
        for (int i = 0; i < 1000; i++)
        {
            sphereBox.HittableList.Add(new Sphere(whiteMat, new GeometricTransform(Cube.RandomPointInCube(0f, 165f)),
                10f));
        }

        Matrix4x4 sphereBoxTransform = Matrix4x4.CreateRotationY(15f.ToRadians()) *
                                       Matrix4x4.CreateTranslation(new Vector3(-100f, 270f, 395f));
        world.HittableList.Add(new HitTransformer(new BvhNode(sphereBox, 0f, 1f), sphereBoxTransform));

        return world;
    }
}
