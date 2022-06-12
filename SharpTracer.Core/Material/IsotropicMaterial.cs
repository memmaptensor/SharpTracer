using System.Drawing;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Material;

public class IsotropicMaterial : IMaterial
{
    public IsotropicMaterial(Color albedo) => Albedo = albedo;
    public Color Albedo { get; }

    public bool Scatter(Ray ray, HitRecord hit, out Color attenuation, out Ray outRay)
    {
        outRay = new Ray(hit.Position, Sphere.RandomPointInSphere(), ray.Time);
        attenuation = Albedo;

        return true;
    }
}
