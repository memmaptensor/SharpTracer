using SharpTracer.Core.Logging;

namespace SharpTracer.Core.Renderer;

public class BvhNode : IHittable
{
    public BvhNode(HittableGroup world, float time0, float time1) :
        this(world.HittableList, 0, world.HittableList.Count, time0, time1)
    {
    }

    public BvhNode(List<IHittable> src, int start, int end, float time0, float time1)
    {
        List<IHittable> objects = new(src);

        int axis = new Random().Next(0, 3);
        BoxCompare comparator =
            axis == 0 ? new BoxCompare(0) :
            axis == 1 ? new BoxCompare(1) :
            new BoxCompare(2);
        int objectSpan = end - start;

        if (objectSpan == 1)
        {
            Left = Right = objects[start];
        }
        else if (objectSpan == 2)
        {
            if (comparator.Compare(objects[start], objects[start + 1]) == -1)
            {
                Left = objects[start];
                Right = objects[start + 1];
            }
            else
            {
                Left = objects[start + 1];
                Right = objects[start];
            }
        }
        else
        {
            objects.Sort(start, end - start, comparator);
            int mid = start + objectSpan / 2;
            Left = new BvhNode(objects, start, mid, time0, time1);
            Right = new BvhNode(objects, mid, end, time0, time1);
        }

        AxisAlignedBoundingBox boxLeft = Left.BoundingBox(time0, time1);
        AxisAlignedBoundingBox boxRight = Right.BoundingBox(time0, time1);

        if (boxLeft is null || boxRight is null)
        {
            ConsoleLogger.Get().LogError("No bounding box in BvhNode ctor");
            throw new Exception();
        }

        Box = AxisAlignedBoundingBox.SurroundingBox(boxLeft, boxRight);
    }

    public IHittable Left { get; }
    public IHittable Right { get; }
    public AxisAlignedBoundingBox Box { get; }

    public HitRecord? HitIfExists(Ray ray, float tMin, float tMax)
    {
        if (!Box.IsHit(ray, tMin, tMax))
        {
            return null;
        }

        HitRecord? hitLeft = Left.HitIfExists(ray, tMin, tMax);
        HitRecord? hitRight = Right.HitIfExists(ray, tMin, hitLeft is null ? ray.Time : tMax);

        return hitLeft ?? hitRight;
    }

    public AxisAlignedBoundingBox BoundingBox(float time0, float time1) => Box;
}
