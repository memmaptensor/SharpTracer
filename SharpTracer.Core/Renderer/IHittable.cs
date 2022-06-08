using SharpTracer.Core.Material;

namespace SharpTracer.Core.Renderer;

public interface IHittable
{
    public IMaterial Material { get; set; }
    public HitRecord? HitIfExists(Ray ray, float tMin, float tMax);
}
