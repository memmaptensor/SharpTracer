using SharpTracer.Core.SceneCamera;

namespace SharpTracer.Core.Scene;

public interface IEyeView
{
    public Camera GetCamera();
}
