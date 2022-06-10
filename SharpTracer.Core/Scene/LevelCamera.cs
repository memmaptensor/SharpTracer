using System.Numerics;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.Scene;

public class LevelCamera : IEyeView
{
    public Camera GetCamera()
    {
        Vector3 lookFrom = new(13f, 2f, 3f);
        Vector3 lookAt = new(0f, 0f, 0f);
        float fov = 20f;
        float distToFocus = 10f;
        float aperture = 0.02f;
        Camera camera = new(800, 600, lookFrom, lookAt, fov, aperture, distToFocus, 0f, 1f);

        return camera;
    }
}
