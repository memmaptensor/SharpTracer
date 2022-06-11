using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Geometry.Plane;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Texture;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Scene;

public class LightedPerlinScene : IScene
{
    public HittableGroup Render()
    {
        HittableGroup world = new();

        NoiseData noiseData = new(
            FastNoiseLite.NoiseType.Perlin,
            1337,
            1f,
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 0f, 0f),
            7);

        FastNoiseLite noise = new();
        MarbleTexture noiseTex = new(noise, noiseData, Color.White);
        TexturedMaterial sphereMat = new(noiseTex);

        world.HittableList.Add(new Sphere(sphereMat, new GeometricTransform(new Vector3(0f, -1000f, 0f)), 1000f));
        world.HittableList.Add(new Sphere(sphereMat, new GeometricTransform(new Vector3(0f, 2f, 0f)), 2f));

        DiffuseLightMaterial diffLightMat = new(new Vector3(4f, 4f, 4f));
        world.HittableList.Add(new ZPlane(
            diffLightMat,
            new Vector2(3f, 1f),
            new Vector2(5f, 3f),
            -2f));
        world.HittableList.Add(new Sphere(diffLightMat, new GeometricTransform(new Vector3(0f, 7f, 0f)), 2f));

        return world;
    }
}
