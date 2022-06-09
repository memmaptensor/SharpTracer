using System.Numerics;

namespace SharpTracer.Core.Renderer;

public struct Ray
{
    public Vector3 Origin;
    public Vector3 Direction;
    public float Time;

    public Ray(Vector3 origin, Vector3 direction, float time = 0f)
    {
        Origin = origin;
        Direction = direction;
        Time = time;
    }

    public Vector3 At(float t) => Origin + t * Direction;
}
