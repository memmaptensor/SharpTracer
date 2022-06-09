using SharpTracer.Core.Logging;

namespace SharpTracer.Core.Renderer;

public class BvhNode : IHittable
{
    public BvhNode(HittableGroup world, float time0, float time1) :
        this(world.HittableList, 0, world.HittableList.Count, time0, time1)
    {
    }

    public BvhNode(List<IHittable> obj, int start, int end, float time0, float time1)
    {
        int axis = new Random().Next(0, 3);
        BoxCompare comparator =
            axis == 0 ? new BoxCompare(0) :
            axis == 1 ? new BoxCompare(1) :
            new BoxCompare(2);
        int objectSpan = end - start;

        if (objectSpan == 1)
        {
            Left = Right = obj[start];
        }
        else if (objectSpan == 2)
        {
            if (comparator.Compare(obj[start], obj[start + 1]) <= 0)
            {
                Left = obj[start];
                Right = obj[start + 1];
            }
            else
            {
                Left = obj[start + 1];
                Right = obj[start];
            }
        }
        else
        {
            obj.Sort(start, end - start, comparator);
            int mid = start + objectSpan / 2;
            Left = new BvhNode(obj, start, mid, time0, time1);
            Right = new BvhNode(obj, mid, end, time0, time1);
        }

        AABB boxLeft = Left.BoundingBox(time0, time1);
        AABB boxRight = Right.BoundingBox(time0, time1);

        if (boxLeft is null || boxRight is null)
        {
            ConsoleLogger.Get().LogError("No bounding box in BvhNode ctor");
            throw new Exception();
        }

        Box = AABB.SurroundingBox(boxLeft, boxRight);
    }

    public IHittable Left { get; }
    public IHittable Right { get; }
    public AABB Box { get; }

    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit)
    {
        if (!Box.IsHit(ray, tMin, tMax))
        {
            return false;
        }

        bool hitLeft = Left.Hit(ray, tMin, tMax, ref hit);
        bool hitRight = Right.Hit(ray, tMin, hitLeft ? hit.T : tMax, ref hit);

        return hitLeft || hitRight;
    }

    public AABB BoundingBox(float time0, float time1) => Box;
}
