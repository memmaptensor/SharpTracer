using System.Drawing;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Material;

public interface IMaterial
{
    public void Scatter(Ray ray, HitRecord hit, out Color attenuation, out Ray outRay);
}
