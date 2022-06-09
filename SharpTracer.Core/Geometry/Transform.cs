using System.Numerics;

namespace SharpTracer.Core.Geometry;

public struct Transform
{
    public float Time;
    public Vector3 Center;

    public Transform(Vector3 center, float time = 0f)
    {
        Time = time;
        Center = center;
    }
}
