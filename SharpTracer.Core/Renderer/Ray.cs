using System.Numerics;

namespace SharpTracer.Core.Renderer;

public struct Ray
{
    public Vector3 Origin;
    public Vector3 Direction;

    public Ray(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public Vector3 At(float t) => Origin + t * Direction;
}
