namespace SharpTracer.Core.Renderer;

public interface IHittable
{
    public HitRecord? HitIfExists(Ray ray, float tMin, float tMax);
    public AxisAlignedBoundingBox BoundingBox(float time0, float time1);
}
