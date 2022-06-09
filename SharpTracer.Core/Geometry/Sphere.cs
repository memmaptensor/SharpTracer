using System.Numerics;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Geometry;

public class Sphere : IHittableMat
{
    private Transform _transform;

    public Sphere(IMaterial material, Transform transform, float radius)
    {
        Material = material;
        _transform = transform;
        Radius = radius;
    }

    public virtual Vector3 Center => _transform.Center;
    public float Radius { get; }

    public IMaterial Material { get; set; }

    public virtual bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit)
    {
        Vector3 oc = ray.Origin - Center;
        float a = ray.Direction.LengthSquared();
        float halfB = Vector3.Dot(oc, ray.Direction);
        float c = oc.LengthSquared() - Radius * Radius;
        float discriminant = halfB * halfB - a * c;

        if (discriminant < 0f)
        {
            return false;
        }

        float sqrtD = MathF.Sqrt(discriminant);
        // Nearest root within range
        float root = (-halfB - sqrtD) / a;
        if (root < tMin || tMax < root)
        {
            root = (-halfB + sqrtD) / a;
            if (root < tMin || tMax < root)
            {
                return false;
            }
        }

        hit = new HitRecord();
        hit.T = root;
        hit.Position = ray.At(root);
        Vector3 outwardNormal = (hit.Position - Center) / Radius;
        hit.SetFaceNormal(ray, outwardNormal);
        hit.Material = Material;

        return true;
    }

    public virtual AABB BoundingBox(float time0, float time1)
    {
        AABB aabb = new(
            Center - new Vector3(Radius, Radius, Radius),
            Center + new Vector3(Radius, Radius, Radius));
        return aabb;
    }

    public static Vector3 RandomPointInSphere(Random rng)
    {
        while (true)
        {
            float x, y, z;
            x = rng.NextSingle() * 2f - 1f;
            y = rng.NextSingle() * 2f - 1f;
            z = rng.NextSingle() * 2f - 1f;
            Vector3 vec = new(x, y, z);
            if (vec.LengthSquared() < 1f)
            {
                return vec;
            }
        }
    }
}
