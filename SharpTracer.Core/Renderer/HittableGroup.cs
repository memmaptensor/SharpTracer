namespace SharpTracer.Core.Renderer;

public class HittableGroup : IHittable
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

    public AxisAlignedBoundingBox BoundingBox(float time0, float time1)
    {
        if (HittableList.Count <= 0)
        {
            return null;
        }

        AxisAlignedBoundingBox outputBox = null;
        bool firstBox = true;

        foreach (IHittable obj in HittableList)
        {
            AxisAlignedBoundingBox tempBox = obj.BoundingBox(time0, time1);
            if (tempBox is null)
            {
                return null;
            }

            outputBox = firstBox ? tempBox : AxisAlignedBoundingBox.SurroundingBox(outputBox, tempBox);
            firstBox = false;
        }

        return outputBox;
    }
}
