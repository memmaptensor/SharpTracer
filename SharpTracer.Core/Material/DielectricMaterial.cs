using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Material;

public class DielectricMaterial : IMaterial
{
    public DielectricMaterial(Color albedo, float ior)
    {
        Albedo = albedo;
        IOR = ior;
    }

    public Color Albedo { get; }
    public float IOR { get; }

    public bool Scatter(Ray ray, HitRecord hit, out Color attenuation, out Ray outRay)
    {
        attenuation = Albedo;
        var refractionRatio = hit.IsFrontFace ? 1f / IOR : IOR;
        var dir = Vector3.Normalize(ray.Direction);

        var cosTheta = MathF.Min(Vector3.Dot(-dir, hit.Normals), 1f);
        var sinTheta = MathF.Sqrt(1f - cosTheta * cosTheta);

        Vector3 rayDir;
        if (refractionRatio * sinTheta > 1f || Reflectance(cosTheta, refractionRatio) > Random.Shared.NextSingle())
        {
            rayDir = Vector3.Reflect(dir, hit.Normals);
        }
        else
        {
            rayDir = Refract(dir, hit.Normals, refractionRatio);
        }

        outRay = new Ray(hit.Position, rayDir, ray.Time);
        return true;
    }

    private static Vector3 Refract(Vector3 uv, Vector3 n, float etaiOverEtat)
    {
        var cosTheta = MathF.Min(Vector3.Dot(-uv, n), 1f);
        var rayOutPerpendicular = etaiOverEtat * (uv + cosTheta * n);
        var rayOutParallel = -MathF.Sqrt(MathF.Abs(1f - rayOutPerpendicular.LengthSquared())) * n;
        return rayOutPerpendicular + rayOutParallel;
    }

    private static float Reflectance(float cosine, float refractionRatio)
    {
        // Schlick's approximation
        var r0 = (1 - refractionRatio) / (1 + refractionRatio);
        r0 *= r0;
        return r0 + (1 - r0) * MathF.Pow(1 - cosine, 5);
    }
}
