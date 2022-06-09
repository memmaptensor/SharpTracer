using System.Numerics;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Renderer;

public class Camera
{
    private float _time0, _time1;
    private Vector3 _u, _v, _w;

    public Camera(int width, int height, Vector3 origin, Vector3 lookAt, float fov, float aperture, float focusDistance,
        float time0, float time1)
    {
        Width = width;
        Height = height;
        Origin = origin;
        AspectRatio = (float)Width / Height;
        _time0 = time0;
        _time1 = time1;

        float theta = fov.ToRadians();
        float h = MathF.Tan(theta / 2f);
        float viewportHeight = 2f * h;
        float viewportWidth = AspectRatio * viewportHeight;

        Vector3 vup = Vector3.UnitY;
        _w = Vector3.Normalize(Origin - lookAt);
        _u = Vector3.Normalize(Vector3.Cross(vup, _w));
        _v = Vector3.Cross(_w, _u);

        Horizontal = viewportWidth * _u * focusDistance;
        Vertical = viewportHeight * _v * focusDistance;
        LowerLeftCorner = Origin - Horizontal / 2f - Vertical / 2f - _w * focusDistance;
        LensRadius = aperture / 2f;
    }

    public int Width { get; }
    public int Height { get; }
    public Vector3 Origin { get; }

    public float AspectRatio { get; }
    public Vector3 Horizontal { get; }
    public Vector3 Vertical { get; }
    public Vector3 LowerLeftCorner { get; }

    public float LensRadius { get; }


    public Ray GetRay(Random rng, float s, float t)
    {
        Vector3 rd = LensRadius * RandomInUnitDisk(rng);
        Vector3 offset = _u * rd.X + Vertical * rd.Y;

        float randomTime = FloatHelper.Lerp(_time0, _time1, rng.NextSingle());
        return new Ray(Origin + offset, LowerLeftCorner + s * Horizontal + t * Vertical - Origin - offset, randomTime);
    }

    private static Vector3 RandomInUnitDisk(Random rng)
    {
        while (true)
        {
            float a = rng.NextSingle() * 2f - 1f;
            float b = rng.NextSingle() * 2f - 1f;
            Vector3 p = new(a, b, 0f);
            if (p.LengthSquared() < 1f)
            {
                return p;
            }
        }
    }
}
