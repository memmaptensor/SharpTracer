using System.Drawing;
using System.Numerics;

namespace SharpTracer.Core.Texture;

public class ColorTexture : ITexture
{
    public ColorTexture(Color color) => Color = color;

    public Color Color { get; }

    public Color FromUV(Vector2 uv, Vector3 p) => Color;
}
