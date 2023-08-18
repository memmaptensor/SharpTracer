using System.Numerics;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Material;
using SharpTracer.Core.Texture;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Scene;

public class CheckerScene : IScene
{
    public HittableGroup Render()
    {
        HittableGroup world = new();
        var evenColor = ColorHelper.FromRGBAF(0.2f, 0.3f, 0.1f);
        var oddColor = ColorHelper.FromRGBAF(0.9f, 0.9f, 0.9f);
        CheckerboardTexture checkerboardTex = new(oddColor, evenColor);
        TexturedMaterial sphereMat = new(checkerboardTex);

        world.HittableList.Add(new Sphere(sphereMat, new GeometricTransform(new Vector3(0f, -10f, 0f)), 10f));
        world.HittableList.Add(new Sphere(sphereMat, new GeometricTransform(new Vector3(0f, 10f, 0f)), 10f));

        return world;
    }
}
