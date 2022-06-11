using System.Numerics;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Geometry.Plane;

public class YPlane : ZPlane
{
    public YPlane(IMaterial material, Vector2 bl, Vector2 tr, float y) :
        base(material, bl, tr, y)
    {
    }

    protected override void SetHitParams(Ray ray, out float t, out Vector3 outwardNormal, out float a, out float b)
    {
        t = (K - ray.Origin.Y) / ray.Direction.Y;
        outwardNormal = new Vector3(0f, 1f, 0f);
        a = ray.Origin.X + t * ray.Direction.X;
        b = ray.Origin.Z + t * ray.Direction.Z;
    }

    public override AABB BoundingBox(float time0, float time1)
    {
        Vector3 bl = new(BL.X, K - Thickness, BL.Y);
        Vector3 tr = new(TR.X, K + Thickness, TR.Y);

        return new AABB(bl, tr);
    }
}
