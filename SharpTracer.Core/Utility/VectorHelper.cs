using System.Numerics;

namespace SharpTracer.Core.Utility;

public static class VectorHelper
{
    public static bool IsNearZero(this Vector3 vec)
    {
        const float epsilon = float.Epsilon;
        float x = MathF.Abs(vec.X);
        float y = MathF.Abs(vec.Y);
        float z = MathF.Abs(vec.Z);
        return x < epsilon && y < epsilon && z < epsilon;
    }
}
