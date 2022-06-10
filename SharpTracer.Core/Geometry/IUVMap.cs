using System.Numerics;

namespace SharpTracer.Core.Geometry;

public interface IUVMap
{
    public Vector2 GetUV(Vector3 p);
}
