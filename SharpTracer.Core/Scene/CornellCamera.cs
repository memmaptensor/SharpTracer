using System.Numerics;
using SharpTracer.Core.SceneCamera;

namespace SharpTracer.Core.Scene;

public class CornellCamera : IEyeView
{
    public Camera GetCamera()
    {
        Vector3 lookFrom = new(278f, 278f, -750f);
        Vector3 lookAt = new(278f, 278f, 0f);
        float fov = 40f;
        float distToFocus = Vector3.Distance(lookFrom, lookAt);
        float aperture = 0.001f;
        Camera camera = new(600, 600, lookFrom, lookAt, fov, aperture, distToFocus, 0f, 1f);

        return camera;
    }
}
