using System.Numerics;
using SharpTracer.Core.SceneCamera;

namespace SharpTracer.Core.Scene;

public class CornellCamera : IEyeView
{
    public Camera GetCamera()
    {
        Vector3 lookFrom = new(278f, 278f, -750f);
        Vector3 lookAt = new(278f, 278f, 0f);
        var fov = 40f;
        var distToFocus = /*Vector3.Distance(lookFrom, lookAt)*/10f;
        var aperture = /*0.001f*/0f;
        var time0 = 0.0f;
        var time1 = 1.0f;

        Camera camera = new( /*8*/2048, /*8*/2048, lookFrom, lookAt, fov, aperture, distToFocus, time0, time1);

        return camera;
    }
}
