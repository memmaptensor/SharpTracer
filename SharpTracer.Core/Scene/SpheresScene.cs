using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Material;
using SharpTracer.Core.Texture;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Scene;

public class SpheresScene : IScene
{
    public HittableGroup Render()
    {
        HittableGroup world = new();
        // Ground
        Color evenColor = ColorHelper.FromRGBAF(0.2f, 0.3f, 0.1f);
        Color oddColor = ColorHelper.FromRGBAF(0.9f, 0.9f, 0.9f);
        CheckerboardTexture checkerboardTex = new(oddColor, evenColor);
        TexturedMaterial groundMaterial = new(checkerboardTex);
        world.HittableList.Add(new Sphere(groundMaterial, new GeometricTransform(new Vector3(0f, -1000f, 0f)), 1000f));
        // Random small spheres
        for (int i = -11; i < 11; i++)
        for (int j = -11; j < 11; j++)
        {
            float materialProbability = Random.Shared.NextSingle();
            Vector3 center = new(i + 0.9f * Random.Shared.NextSingle(), 0.2f, j + 0.9f * Random.Shared.NextSingle());
            if ((center - new Vector3(4f, 0.2f, 0f)).LengthSquared() > 0.9f * 0.9f)
            {
                IMaterial material;
                Color albedo = ColorHelper.FromRandom();
                if (materialProbability < 0.9f)
                {
                    material = new RoughMaterial(albedo);
                }
                else if (materialProbability < 0.95f)
                {
                    float fuzz = Random.Shared.NextSingle() * 0.5f;
                    material = new MetalMaterial(albedo, fuzz);
                }
                else
                {
                    material = new DielectricMaterial(Color.White, 1.5f);
                }

                Vector3 center2 = center + new Vector3(0f, 0.5f * Random.Shared.NextSingle(), 0f);
                world.HittableList.Add(
                    new MovingSphere(
                        material,
                        new GeometricTransform(center),
                        new GeometricTransform(center2, 1f),
                        0.2f));
            }
        }

        // Big spheres
        DielectricMaterial glassMaterial = new(Color.White, 1.5f);
        RoughMaterial roughMaterial = new(ColorHelper.FromRGBAF(0.4f, 0.2f, 0.1f));
        MetalMaterial metalMaterial = new(ColorHelper.FromRGBAF(0.7f, 0.6f, 0.5f), 0f);
        world.HittableList.Add(new Sphere(glassMaterial, new GeometricTransform(new Vector3(0f, 1f, 0f)), 1f));
        world.HittableList.Add(new Sphere(roughMaterial, new GeometricTransform(new Vector3(-4f, 1f, 0f)), 1f));
        world.HittableList.Add(new Sphere(metalMaterial, new GeometricTransform(new Vector3(4f, 1f, 0f)), 1f));

        return world;
    }
}
