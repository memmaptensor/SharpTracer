using System.Numerics;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Geometry;

public class MovingSphere : Sphere
{
    private Transform _t0, _t1;

    public MovingSphere(IMaterial material, Transform t0, Transform t1, float radius) : base(material,
        t0, radius)
    {
        _t0 = t0;
        _t1 = t1;
    }

    public float Time { get; set; }

    public override Vector3 Center =>
        _t0.Center + (Time - _t0.Time) / (_t1.Time - _t0.Time) * (_t1.Center - _t0.Center);

    public override AxisAlignedBoundingBox BoundingBox(float time0, float time1)
    {
        Time = time0;
        AxisAlignedBoundingBox box0 = new(
            Center - new Vector3(Radius, Radius, Radius),
            Center + new Vector3(Radius, Radius, Radius));
        Time = time1;
        AxisAlignedBoundingBox box1 = new(
            Center - new Vector3(Radius, Radius, Radius),
            Center + new Vector3(Radius, Radius, Radius));

        return AxisAlignedBoundingBox.SurroundingBox(box0, box1);
    }

    public override HitRecord? HitIfExists(Ray ray, float tMin, float tMax)
    {
        Time = ray.Time;
        return base.HitIfExists(ray, tMin, tMax);
    }
}
