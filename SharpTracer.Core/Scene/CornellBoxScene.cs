using System.Numerics;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Geometry.Plane;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Scene;

public class CornellBoxScene : IScene
{
    public HittableGroup Render()
    {
        HittableGroup world = new();

        DiffuseLightMaterial diffLightMat = new(new Vector3(15f, 15f, 15f));
        RoughMaterial redMat = new(ColorHelper.FromRGBAF(.65f, .05f, .05f));
        RoughMaterial whiteMat = new(ColorHelper.FromRGBAF(.73f, .73f, .73f));
        RoughMaterial greenMat = new(ColorHelper.FromRGBAF(.12f, .45f, .15f));

        world.HittableList.Add(new XPlane(redMat, new Vector2(0f, 0f), new Vector2(555f, 555f), 0f));
        world.HittableList.Add(new XPlane(greenMat, new Vector2(0f, 0f), new Vector2(555f, 555f), 555f));
        world.HittableList.Add(new YPlane(whiteMat, new Vector2(0f, 0f), new Vector2(555f, 555f), 0f));
        world.HittableList.Add(new YPlane(whiteMat, new Vector2(0f, 0f), new Vector2(555f, 555f), 555f));
        world.HittableList.Add(new ZPlane(whiteMat, new Vector2(0f, 0f), new Vector2(555f, 555f), 555f));
        world.HittableList.Add(new YPlane(diffLightMat, new Vector2(213f, 227f), new Vector2(343f, 332f), 554f));

        Cube cube1 = new(whiteMat, new GeometricTransform(
            new Vector3(165f, 330f, 165f) / 2f,
            new Vector3(165f, 330f, 165f)));
        Matrix4x4 cube1Mat = Matrix4x4.CreateRotationY(15f.ToRadians()) *
                             Matrix4x4.CreateTranslation(new Vector3(265f, 0f, 295f));
        HitTransformer transformedCube1 = new(cube1, cube1Mat);
        world.HittableList.Add(transformedCube1);

        Cube cube2 = new(whiteMat, new GeometricTransform(
            new Vector3(165f, 165f, 165f) / 2f,
            new Vector3(165f, 165f, 165f)));
        Matrix4x4 cube2Mat = Matrix4x4.CreateRotationY(-18f.ToRadians()) *
                             Matrix4x4.CreateTranslation(new Vector3(130f, 0f, 65f));
        HitTransformer transformedCube2 = new(cube2, cube2Mat);
        world.HittableList.Add(transformedCube2);

        return world;
    }
}
