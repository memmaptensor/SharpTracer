using System.Numerics;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Geometry.Plane;

public class ZPlane : IHittableMat
{
    protected const float Thickness = 0.0001f;

    public ZPlane(IMaterial material, Vector2 bl, Vector2 tr, float z)
    {
        Material = material;
        BL = bl;
        TR = tr;
        K = z;
    }

    protected Vector2 BL { get; }
    protected Vector2 TR { get; }
    protected float K { get; }
    public IMaterial Material { get; set; }

    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit)
    {
        SetHitParams(ray, out var t, out var outwardNormal, out var a, out var b);

        if (t < tMin || t > tMax)
        {
            return false;
        }

        if (a < BL.X || a > TR.X || b < BL.Y || b > TR.Y)
        {
            return false;
        }

        hit.UV.X = (a - BL.X) / (TR.X - BL.X);
        hit.UV.Y = (b - BL.Y) / (TR.Y - BL.Y);
        hit.T = t;
        hit.SetFaceNormal(ray, outwardNormal);
        hit.Material = Material;
        hit.Position = ray.At(t);
        return true;
    }

    public virtual AABB BoundingBox(float time0, float time1)
    {
        Vector3 bl = new(BL, K - Thickness);
        Vector3 tr = new(TR, K + Thickness);

        return new AABB(bl, tr);
    }

    protected virtual void SetHitParams(Ray ray, out float t, out Vector3 outwardNormal, out float x, out float y)
    {
        t = (K - ray.Origin.Z) / ray.Direction.Z;
        outwardNormal = new Vector3(0f, 0f, 1f);
        x = ray.Origin.X + t * ray.Direction.X;
        y = ray.Origin.Y + t * ray.Direction.Y;
    }
}
