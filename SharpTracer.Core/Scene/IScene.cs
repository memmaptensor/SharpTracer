using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Scene;

public interface IScene
{
    public HittableGroup Render();
}
