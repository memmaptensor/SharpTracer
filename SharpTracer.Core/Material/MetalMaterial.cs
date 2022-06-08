using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Material;

public class MetalMaterial : IMaterial
{
    public MetalMaterial(Color albedo) => Albedo = albedo;

    public Color Albedo { get; }

    public void Scatter(Ray ray, HitRecord hit, out Color attenuation, out Ray outRay)
    {
        Vector3 reflected = Reflect(Vector3.Normalize(ray.Direction), hit.Normals);
        outRay = new Ray(hit.Position, reflected);
        if (Vector3.Dot(outRay.Direction, hit.Normals) <= 0)
        {
            attenuation = Vector3.Zero.ToColor();
        }
        else
        {
            attenuation = Albedo;
        }
    }

    private Vector3 Reflect(Vector3 v, Vector3 n) => v - 2f * Vector3.Dot(v, n) * n;
}
