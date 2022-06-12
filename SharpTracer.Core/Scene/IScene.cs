using SharpTracer.Core.Geometry;

namespace SharpTracer.Core.Scene;

public interface IScene
{
    public HittableGroup Render();
}
