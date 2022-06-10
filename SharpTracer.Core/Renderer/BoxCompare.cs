using SharpTracer.Core.Logging;

namespace SharpTracer.Core.Renderer;

public class BoxCompare : IComparer<IHittable>
{
    private readonly int _axis;

    public BoxCompare(int axis) => _axis = axis;

    public int Compare(IHittable a, IHittable b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        AABB boxA = a.BoundingBox(0f, 0f);
        AABB boxB = b.BoundingBox(0f, 0f);

        if (boxA is null || boxB is null)
        {
            ConsoleLogger.Get().LogError("No bounding box in BvhNode ctor");
            throw new Exception();
        }

        if (_axis == 0)
        {
            if (boxA.Min.X < boxB.Min.X)
            {
                return -1;
            }

            if (MathF.Abs(boxA.Min.X - boxB.Min.X) < float.Epsilon)
            {
                return 0;
            }

            return 1;
        }

        if (_axis == 1)
        {
            if (boxA.Min.Y < boxB.Min.Y)
            {
                return -1;
            }

            if (MathF.Abs(boxA.Min.Y - boxB.Min.Y) < float.Epsilon)
            {
                return 0;
            }

            return 1;
        }

        if (_axis == 2)
        {
            if (boxA.Min.Z < boxB.Min.Z)
            {
                return -1;
            }

            if (MathF.Abs(boxA.Min.Z - boxB.Min.Z) < float.Epsilon)
            {
                return 0;
            }

            return 1;
        }

        ConsoleLogger.Get().LogError("Unsupported axis");
        throw new ArgumentException();
    }
}
