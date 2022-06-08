namespace SharpTracer.Core.Renderer;

public class HittableGroup
{
    public HittableGroup() => HittableList = new List<IHittable>();

    public List<IHittable> HittableList { get; }

    public HitRecord? HitIfExists(Ray ray, float tMin, float tMax)
    {
        HitRecord? hitRecord = null;
        float closestSoFar = tMax;

        foreach (IHittable hittable in HittableList)
        {
            HitRecord? hit = hittable.HitIfExists(ray, tMin, closestSoFar);
            if (hit != null)
            {
                closestSoFar = (float)hit.Value.T;
                hitRecord = hit.Value;
            }
        }

        return hitRecord;
    }
}
