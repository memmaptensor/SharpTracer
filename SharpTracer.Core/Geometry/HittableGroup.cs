using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Geometry;

public class HittableGroup : IHittable
{
    public HittableGroup() => HittableList = new List<IHittable>();

    public List<IHittable> HittableList { get; }

    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit)
    {
        HitRecord tempHit = new();
        var hitAnything = false;
        var closestSoFar = tMax;

        foreach (var hittable in HittableList)
        {
            if (hittable.Hit(ray, tMin, closestSoFar, ref tempHit))
            {
                hitAnything = true;
                closestSoFar = tempHit.T;
                hit = tempHit;
            }
        }

        return hitAnything;
    }

    public AABB BoundingBox(float time0, float time1)
    {
        if (HittableList.Count <= 0)
        {
            return null;
        }

        AABB outputBox = null;
        var firstBox = true;

        foreach (var obj in HittableList)
        {
            var tempBox = obj.BoundingBox(time0, time1);
            if (tempBox is null)
            {
                return null;
            }

            outputBox = firstBox ? tempBox : AABB.SurroundingBox(outputBox, tempBox);
            firstBox = false;
        }

        return outputBox;
    }
}
