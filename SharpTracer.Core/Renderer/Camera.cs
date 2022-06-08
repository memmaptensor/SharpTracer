using System.Numerics;

namespace SharpTracer.Core.Renderer;

public class Camera
{
    public Camera(int width, int height, float focalLength, Vector3 origin)
    {
        Width = width;
        Height = height;
        FocalLength = focalLength;
        Origin = origin;
    }

    public int Width { get; }
    public int Height { get; }
    public float FocalLength { get; }
    public Vector3 Origin { get; }

    public float AspectRatio => (float)Width / Height;
    public float ViewportHeight => 2f;
    public float ViewportWidth => AspectRatio * ViewportHeight;
    public Vector3 Horizontal => new(ViewportWidth, 0f, 0f);
    public Vector3 Vertical => new(0f, ViewportHeight, 0f);
    public Vector3 LowerLeftCorner => Origin - Horizontal / 2f - Vertical / 2f - new Vector3(0f, 0f, FocalLength);

    public Ray GetRay(float u, float v) => new(Origin, LowerLeftCorner + u * Horizontal + v * Vertical - Origin);
}
