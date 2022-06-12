using System.Numerics;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Geometry;

public class Sphere : IHittableMat, IUVMap
{
    private readonly GeometricTransform _transform;

    public Sphere(IMaterial material, GeometricTransform GeometricTransform, float radius)
    {
        Material = material;
        _transform = GeometricTransform;
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
        hit.UV = GetUV(outwardNormal);
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

    public Vector2 GetUV(Vector3 p)
    {
        float theta = MathF.Acos(-p.Y);
        float phi = MathF.Atan2(-p.Z, p.X) + MathF.PI;

        float u = phi / (2f * MathF.PI);
        float v = theta / MathF.PI;

        return new Vector2(u, v);
    }

    public static Vector3 RandomPointInSphere()
    {
        while (true)
        {
            float x, y, z;
            x = Random.Shared.NextSingle() * 2f - 1f;
            y = Random.Shared.NextSingle() * 2f - 1f;
            z = Random.Shared.NextSingle() * 2f - 1f;
            Vector3 vec = new(x, y, z);
            if (vec.LengthSquared() < 1f)
            {
                return vec;
            }
        }
    }
}
