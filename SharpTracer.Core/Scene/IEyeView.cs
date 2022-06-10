using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Scene;

public interface IEyeView
{
    public Camera GetCamera();
}
