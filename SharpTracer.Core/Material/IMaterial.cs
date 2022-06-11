using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Material;

public interface IMaterial
{
    public bool Scatter(Ray ray, HitRecord hit, out Color attenuation, out Ray outRay);
    public Vector3 Emitted(Vector2 uv, Vector3 p) => Vector3.Zero;
}
