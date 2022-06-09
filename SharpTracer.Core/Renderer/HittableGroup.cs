namespace SharpTracer.Core.Renderer;

public class HittableGroup : IHittable
{
    public HittableGroup() => HittableList = new List<IHittable>();

    public List<IHittable> HittableList { get; }

    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit)
    {
        HitRecord tempHit = new();
        bool hitAnything = false;
        float closestSoFar = tMax;

        foreach (IHittable hittable in HittableList)
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
        bool firstBox = true;

        foreach (IHittable obj in HittableList)
        {
            AABB tempBox = obj.BoundingBox(time0, time1);
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
