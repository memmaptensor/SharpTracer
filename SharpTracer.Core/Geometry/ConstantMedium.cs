using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Material;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Geometry;

public class ConstantMedium : IHittableMat
{
    public ConstantMedium(IHittable original, float density, Color albedo)
    {
        Original = original;
        NegInvDensity = -1f / density;
        Material = new IsotropicMaterial(albedo);
    }

    public float NegInvDensity { get; }
    public IHittable Original { get; }
    public IMaterial Material { get; set; }

    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit)
    {
        HitRecord hit1 = default;
        HitRecord hit2 = default;

        if (!Original.Hit(ray, float.NegativeInfinity, float.PositiveInfinity, ref hit1))
        {
            return false;
        }

        if (!Original.Hit(ray, hit1.T + 0.0001f, float.PositiveInfinity, ref hit2))
        {
            return false;
        }

        if (hit1.T < tMin)
        {
            hit1.T = tMin;
        }

        if (hit2.T > tMax)
        {
            hit2.T = tMax;
        }

        if (hit1.T >= hit2.T)
        {
            return false;
        }

        if (hit1.T < 0)
        {
            hit1.T = 0;
        }

        var rayLength = ray.Direction.Length();
        var distanceInsideBoundary = (hit2.T - hit1.T) * rayLength;
        var hitDistance = NegInvDensity * MathF.Log(Random.Shared.NextSingle());

        if (hitDistance > distanceInsideBoundary)
        {
            return false;
        }

        hit.T = hit1.T + hitDistance / rayLength;
        hit.Position = ray.At(hit.T);
        hit.Normals = new Vector3(1f, 0f, 0f); // arbitrary
        hit.IsFrontFace = true; // arbitrary
        hit.Material = Material;

        return true;
    }

    public AABB BoundingBox(float time0, float time1) => Original.BoundingBox(time0, time1);
}
