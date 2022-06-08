namespace SharpTracer.Core.Utility;

public static class FloatHelper
{
    public static float ToRadians(this float deg) => deg * MathF.PI / 180f;

    public static float ToDegrees(this float rad) => rad * 180f / MathF.PI;
}
