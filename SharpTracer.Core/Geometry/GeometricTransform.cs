using System.Numerics;

namespace SharpTracer.Core.Geometry;

public struct GeometricTransform
{
    public float Time;
    public Vector3 Center;
    public Vector3 Scale;

    public GeometricTransform(Vector3 center, float time = 0f)
    {
        Time = time;
        Center = center;
        Scale = Vector3.One;
    }

    public GeometricTransform(Vector3 center, Vector3 scale, float time = 0f)
    {
        Time = time;
        Center = center;
        Scale = scale;
    }
}
