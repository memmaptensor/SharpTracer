using System.Drawing;
using System.Numerics;

namespace SharpTracer.Core.Texture;

public class CheckerboardTexture : ITexture
{
    public CheckerboardTexture(Color odd, Color even)
    {
        Odd = new ColorTexture(odd);
        Even = new ColorTexture(even);
    }

    public CheckerboardTexture(ITexture odd, ITexture even)
    {
        Odd = odd;
        Even = even;
    }

    public ITexture Odd { get; }
    public ITexture Even { get; }

    public Color FromUV(Vector2 uv, Vector3 p)
    {
        float sines = MathF.Sin(10f * p.X) * MathF.Sin(10f * p.Y) * MathF.Sin(10f * p.Z);

        if (sines < 0)
        {
            return Odd.FromUV(uv, p);
        }

        return Even.FromUV(uv, p);
    }
}
