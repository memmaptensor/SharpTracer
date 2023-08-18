using System.Numerics;
using SharpTracer.Core.SceneCamera;

namespace SharpTracer.Core.Scene;

public class FarCamera : IEyeView
{
    public Camera GetCamera()
    {
        Vector3 lookFrom = new(26f, 3f, 6f);
        Vector3 lookAt = new(0f, 2f, 0f);
        var fov = 20f;
        var distToFocus = Vector3.Distance(lookFrom, lookAt);
        var aperture = 0.001f;
        Camera camera = new(800, 600, lookFrom, lookAt, fov, aperture, distToFocus, 0f, 1f);

        return camera;
    }
}
