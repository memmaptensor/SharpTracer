using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Texture;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Scene;

public class PerlinScene : IScene
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

        world.HittableList.Add(new Sphere(sphereMat, new Transform(new Vector3(0f, -1000f, 0f)), 1000f));
        world.HittableList.Add(new Sphere(sphereMat, new Transform(new Vector3(0f, 2f, 0f)), 2f));

        return world;
    }
}
