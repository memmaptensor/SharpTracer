using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Texture;

public class MarbleTexture : TurbulenceNoiseTexture
{
    public MarbleTexture(FastNoiseLite noise, NoiseData noiseData, Color color) : base(noise, noiseData, color)
    {
    }

    public override Color FromUV(Vector2 uv, Vector3 pt)
    {
        pt += NoiseData.Offset;
        float turbulence = GetTurbulence(uv, pt);
        return ((1f + MathF.Sin(NoiseData.Scale.Z * pt.Z + 10f * turbulence)) * 0.5f * Color.ToVector3()).ToColor();
    }
}
