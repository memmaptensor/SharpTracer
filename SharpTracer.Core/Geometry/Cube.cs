using System.Numerics;
using SharpTracer.Core.Geometry.Plane;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Geometry;

public class Cube : IHittableMat
{
    private readonly HittableGroup _box;

    public Cube(IMaterial material, GeometricTransform GeometricTransform)
    {
        Material = material;
        _box = new HittableGroup();

        _box.HittableList.Add(new XPlane(
            material,
            new Vector2(GeometricTransform.Center.Y - 0.5f * GeometricTransform.Scale.Y,
                GeometricTransform.Center.Z - 0.5f * GeometricTransform.Scale.Z),
            new Vector2(GeometricTransform.Center.Y + 0.5f * GeometricTransform.Scale.Y,
                GeometricTransform.Center.Z + 0.5f * GeometricTransform.Scale.Z),
            GeometricTransform.Center.X + 0.5f * GeometricTransform.Scale.X));
        _box.HittableList.Add(new XPlane(
            material,
            new Vector2(GeometricTransform.Center.Y - 0.5f * GeometricTransform.Scale.Y,
                GeometricTransform.Center.Z - 0.5f * GeometricTransform.Scale.Z),
            new Vector2(GeometricTransform.Center.Y + 0.5f * GeometricTransform.Scale.Y,
                GeometricTransform.Center.Z + 0.5f * GeometricTransform.Scale.Z),
            GeometricTransform.Center.X - 0.5f * GeometricTransform.Scale.X));
        _box.HittableList.Add(new YPlane(
            material,
            new Vector2(GeometricTransform.Center.X - 0.5f * GeometricTransform.Scale.X,
                GeometricTransform.Center.Z - 0.5f * GeometricTransform.Scale.Z),
            new Vector2(GeometricTransform.Center.X + 0.5f * GeometricTransform.Scale.X,
                GeometricTransform.Center.Z + 0.5f * GeometricTransform.Scale.Z),
            GeometricTransform.Center.Y + 0.5f * GeometricTransform.Scale.Y));
        _box.HittableList.Add(new YPlane(
            material,
            new Vector2(GeometricTransform.Center.X - 0.5f * GeometricTransform.Scale.X,
                GeometricTransform.Center.Z - 0.5f * GeometricTransform.Scale.Z),
            new Vector2(GeometricTransform.Center.X + 0.5f * GeometricTransform.Scale.X,
                GeometricTransform.Center.Z + 0.5f * GeometricTransform.Scale.Z),
            GeometricTransform.Center.Y - 0.5f * GeometricTransform.Scale.Y));
        _box.HittableList.Add(new ZPlane(
            material,
            new Vector2(GeometricTransform.Center.X - 0.5f * GeometricTransform.Scale.X,
                GeometricTransform.Center.Y - 0.5f * GeometricTransform.Scale.Y),
            new Vector2(GeometricTransform.Center.X + 0.5f * GeometricTransform.Scale.X,
                GeometricTransform.Center.Y + 0.5f * GeometricTransform.Scale.Y),
            GeometricTransform.Center.Z + 0.5f * GeometricTransform.Scale.Z));
        _box.HittableList.Add(new ZPlane(
            material,
            new Vector2(GeometricTransform.Center.X - 0.5f * GeometricTransform.Scale.X,
                GeometricTransform.Center.Y - 0.5f * GeometricTransform.Scale.Y),
            new Vector2(GeometricTransform.Center.X + 0.5f * GeometricTransform.Scale.X,
                GeometricTransform.Center.Y + 0.5f * GeometricTransform.Scale.Y),
            GeometricTransform.Center.Z - 0.5f * GeometricTransform.Scale.Z));
    }

    public IMaterial Material { get; set; }

    public virtual bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit) => _box.Hit(ray, tMin, tMax, ref hit);

    public virtual AABB BoundingBox(float time0, float time1) => _box.BoundingBox(time0, time1);

    public static Vector3 RandomPointInCube(float min, float max)
    {
        float x = Remap.Map(Random.Shared.NextSingle(), 0f, 1f, min, max);
        float y = Remap.Map(Random.Shared.NextSingle(), 0f, 1f, min, max);
        float z = Remap.Map(Random.Shared.NextSingle(), 0f, 1f, min, max);

        return new Vector3(x, y, z);
    }
}
