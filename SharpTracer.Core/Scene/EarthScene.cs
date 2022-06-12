using System.Numerics;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Material;
using SharpTracer.Core.Texture;

namespace SharpTracer.Core.Scene;

public class EarthScene : IScene
{
    public HittableGroup Render()
    {
        HittableGroup world = new();
        ImageTexture earthTex = new("earthmap.jpg");
        TexturedMaterial earthMat = new(earthTex);

        world.HittableList.Add(new Sphere(earthMat, new GeometricTransform(new Vector3(0f, 0f, 0f)), 2f));

        return world;
    }
}
