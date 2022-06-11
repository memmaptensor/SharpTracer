using System.Numerics;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Geometry;

public class MovingSphere : Sphere
{
    private readonly GeometricTransform _t0;
    private readonly GeometricTransform _t1;

    public MovingSphere(IMaterial material, GeometricTransform t0, GeometricTransform t1, float radius) : base(material,
        t0, radius)
    {
        _t0 = t0;
        _t1 = t1;
    }

    public float Time { get; set; }

    public override Vector3 Center =>
        _t0.Center + (Time - _t0.Time) / (_t1.Time - _t0.Time) * (_t1.Center - _t0.Center);

    public override AABB BoundingBox(float time0, float time1)
    {
        Time = time0;
        AABB box0 = new(
            Center - new Vector3(Radius, Radius, Radius),
            Center + new Vector3(Radius, Radius, Radius));
        Time = time1;
        AABB box1 = new(
            Center - new Vector3(Radius, Radius, Radius),
            Center + new Vector3(Radius, Radius, Radius));

        return AABB.SurroundingBox(box0, box1);
    }

    public override bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit)
    {
        Time = ray.Time;
        return base.Hit(ray, tMin, tMax, ref hit);
    }
}
