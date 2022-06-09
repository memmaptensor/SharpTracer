namespace SharpTracer.Core.Renderer;

public interface IHittable
{
    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit);
    public AABB BoundingBox(float time0, float time1);
}
