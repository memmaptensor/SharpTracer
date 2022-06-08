using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Material;

public class RoughMaterial : IMaterial
{
    public RoughMaterial(Color albedo) => Albedo = albedo;

    public Color Albedo { get; }

    public void Scatter(Ray ray, HitRecord hit, out Color attenuation, out Ray outRay)
    {
        Vector3 scatterDir = hit.Normals + Vector3.Normalize(Sphere.RandomPointInSphere(new Random()));
        if (scatterDir.IsNearZero())
        {
            scatterDir = hit.Normals;
        }

        outRay = new Ray(hit.Position, scatterDir);
        attenuation = Albedo;
    }
}
