using SharpTracer.Core.Material;

namespace SharpTracer.Core.Renderer;

public interface IHittableMat : IHittable
{
    public IMaterial Material { get; set; }
}
