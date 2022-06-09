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

    public void Scatter(Ray ray, HitRecord hit, out Color attenuation, out Ray outRay)
    {
        attenuation = Albedo;
        float refractionRatio = hit.IsFrontFace ? 1f / IOR : IOR;
        Vector3 dir = Vector3.Normalize(ray.Direction);

        float cosTheta = MathF.Min(Vector3.Dot(-dir, hit.Normals), 1f);
        float sinTheta = MathF.Sqrt(1f - cosTheta * cosTheta);

        Vector3 rayDir;
        if (refractionRatio * sinTheta > 1f || Reflectance(cosTheta, refractionRatio) > new Random().NextSingle())
        {
            rayDir = Vector3.Reflect(dir, hit.Normals);
        }
        else
        {
            rayDir = Refract(dir, hit.Normals, refractionRatio);
        }

        outRay = new Ray(hit.Position, rayDir, ray.Time);
    }

    private Vector3 Refract(Vector3 uv, Vector3 n, float etaiOverEtat)
    {
        float cosTheta = MathF.Min(Vector3.Dot(-uv, n), 1f);
        Vector3 rayOutPerpendicular = etaiOverEtat * (uv + cosTheta * n);
        Vector3 rayOutParallel = -MathF.Sqrt(MathF.Abs(1f - rayOutPerpendicular.LengthSquared())) * n;
        return rayOutPerpendicular + rayOutParallel;
    }

    private float Reflectance(float cosine, float refractionRatio)
    {
        // Schlick's approximation
        float r0 = (1 - refractionRatio) / (1 + refractionRatio);
        r0 *= r0;
        return r0 + (1 - r0) * MathF.Pow(1 - cosine, 5);
    }
}
