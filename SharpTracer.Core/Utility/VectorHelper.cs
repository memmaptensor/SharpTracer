using System.Numerics;

namespace SharpTracer.Core.Utility;

public static class VectorHelper
{
    public static bool IsNearZero(this Vector3 vec)
    {
        const float epsilon = float.Epsilon;
        var x = MathF.Abs(vec.X);
        var y = MathF.Abs(vec.Y);
        var z = MathF.Abs(vec.Z);
        return x < epsilon && y < epsilon && z < epsilon;
    }
}
