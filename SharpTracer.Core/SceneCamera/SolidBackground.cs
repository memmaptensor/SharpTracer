using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.SceneCamera;

public class SolidBackground : ICameraBackground
{
    public SolidBackground(Color color) => Color = color;

    public Color Color { get; }

    public Vector3 GetScreenSpaceColor(Ray ray) => Color.ToVector3();
}
