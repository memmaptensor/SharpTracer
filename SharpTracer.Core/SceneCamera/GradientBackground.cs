using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.SceneCamera;

public class GradientBackground : ICameraBackground
{
    public GradientBackground(Color firstColor, Color secondColor, bool isHorizontalFade)
    {
        FirstColor = firstColor;
        SecondColor = secondColor;
        IsHorizontalFade = isHorizontalFade;
    }

    public Color FirstColor { get; }
    public Color SecondColor { get; }
    public bool IsHorizontalFade { get; }

    public Vector3 GetScreenSpaceColor(Ray ray)
    {
        if (IsHorizontalFade)
        {
            Vector3 dir = Vector3.Normalize(ray.Direction);
            float t = 0.5f * (dir.X + 1f);
            return (1f - t) * FirstColor.ToVector3() + t * SecondColor.ToVector3();
        }
        else
        {
            Vector3 dir = Vector3.Normalize(ray.Direction);
            float t = 0.5f * (dir.Y + 1f);
            return (1f - t) * FirstColor.ToVector3() + t * SecondColor.ToVector3();
        }
    }
}
