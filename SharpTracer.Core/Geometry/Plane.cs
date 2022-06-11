using System.Numerics;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Geometry;

public class Plane : IHittableMat
{
    private const float Thickness = 0.0001f;

    public Plane(IMaterial material, Vector2 bl, Vector2 tr, float z)
    {
        Material = material;
        BL = bl;
        TR = tr;
        Z = z;
    }

    public Vector2 BL { get; }
    public Vector2 TR { get; }
    public float Z { get; }
    public IMaterial Material { get; set; }

    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit)
    {
        float t = (Z - ray.Origin.Z) / ray.Direction.Z;
        if (t < tMin || t > tMax)
        {
            return false;
        }

        float x = ray.Origin.X + t * ray.Direction.X;
        float y = ray.Origin.Y + t * ray.Direction.Y;
        if (x < BL.X || x > TR.X || y < BL.Y || y > TR.Y)
        {
            return false;
        }

        hit.UV.X = (x - BL.X) / (TR.X - BL.X);
        hit.UV.Y = (y - BL.Y) / (TR.Y - BL.Y);
        hit.T = t;
        Vector3 outwardNormal = new(0f, 0f, 1f);
        hit.SetFaceNormal(ray, outwardNormal);
        hit.Material = Material;
        hit.Position = ray.At(t);
        return true;
    }

    public AABB BoundingBox(float time0, float time1)
    {
        Vector3 tl = new(BL, Z - Thickness);
        Vector3 br = new(TR, Z + Thickness);

        return new AABB(tl, br);
    }
}
