using System.Numerics;

namespace SharpTracer.Core.Renderer;

public class AABB
{
    public AABB(Vector3 a, Vector3 b)
    {
        Min = a;
        Max = b;
    }

    public Vector3 Min { get; }
    public Vector3 Max { get; }

    public bool IsHit(Ray ray, float tMin, float tMax)
    {
        {
            float invD = 1f / ray.Direction.X;
            float t0 = (Min.X - ray.Origin.X) * invD;
            float t1 = (Max.X - ray.Origin.X) * invD;
            if (invD < 0f)
            {
                (t0, t1) = (t1, t0);
            }

            tMin = t0 > tMin ? t0 : tMin;
            tMax = t1 < tMax ? t1 : tMax;
            if (tMax <= tMin)
            {
                return false;
            }
        }
        {
            float invD = 1f / ray.Direction.Y;
            float t0 = (Min.Y - ray.Origin.Y) * invD;
            float t1 = (Max.Y - ray.Origin.Y) * invD;
            if (invD < 0f)
            {
                (t0, t1) = (t1, t0);
            }

            tMin = t0 > tMin ? t0 : tMin;
            tMax = t1 < tMax ? t1 : tMax;
            if (tMax <= tMin)
            {
                return false;
            }
        }
        {
            float invD = 1f / ray.Direction.Z;
            float t0 = (Min.Z - ray.Origin.Z) * invD;
            float t1 = (Max.Z - ray.Origin.Z) * invD;
            if (invD < 0f)
            {
                (t0, t1) = (t1, t0);
            }

            tMin = t0 > tMin ? t0 : tMin;
            tMax = t1 < tMax ? t1 : tMax;
            if (tMax <= tMin)
            {
                return false;
            }
        }


        return true;
    }

    public static AABB SurroundingBox(AABB box0, AABB box1)
    {
        Vector3 small = new(
            MathF.Min(box0.Min.X, box1.Min.X),
            MathF.Min(box0.Min.Y, box1.Min.Y),
            MathF.Min(box0.Min.Z, box1.Min.Z));
        Vector3 big = new(
            MathF.Max(box0.Max.X, box1.Max.X),
            MathF.Max(box0.Max.Y, box1.Max.Y),
            MathF.Max(box0.Max.Z, box1.Max.Z));

        return new AABB(small, big);
    }
}
