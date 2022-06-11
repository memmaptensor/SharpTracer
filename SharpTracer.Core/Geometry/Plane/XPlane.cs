using System.Numerics;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Geometry.Plane;

public class XPlane : ZPlane
{
    public XPlane(IMaterial material, Vector2 bl, Vector2 tr, float x) :
        base(material, bl, tr, x)
    {
    }

    protected override void SetHitParams(Ray ray, out float t, out Vector3 outwardNormal, out float a, out float b)
    {
        t = (K - ray.Origin.X) / ray.Direction.X;
        outwardNormal = new Vector3(1f, 0f, 0f);
        a = ray.Origin.Y + t * ray.Direction.Y;
        b = ray.Origin.Z + t * ray.Direction.Z;
    }

    public override AABB BoundingBox(float time0, float time1)
    {
        Vector3 bl = new(K - Thickness, BL.X, BL.Y);
        Vector3 tr = new(K + Thickness, TR.X, TR.Y);

        return new AABB(bl, tr);
    }
}
