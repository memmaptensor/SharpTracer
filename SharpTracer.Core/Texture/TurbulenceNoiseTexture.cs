using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Texture;

public class TurbulenceNoiseTexture : ITexture
{
    public TurbulenceNoiseTexture(FastNoiseLite noise, NoiseData noiseData, Color color)
    {
        Noise = noise;
        NoiseData = noiseData;
        Color = color;

        noise.SetSeed(noiseData.Seed);
        noise.SetFrequency(noiseData.Frequency);
        noise.SetNoiseType(noiseData.NoiseType);
    }

    public FastNoiseLite Noise { get; }
    public NoiseData NoiseData { get; }
    public Color Color { get; }

    public virtual Color FromUV(Vector2 uv, Vector3 pt)
    {
        pt += NoiseData.Offset;
        Vector3 p = new(
            pt.X * NoiseData.Scale.X,
            pt.Y * NoiseData.Scale.Y,
            pt.Z * NoiseData.Scale.Z);
        var turbulence = GetTurbulence(uv, p);
        return (turbulence * Color.ToVector3()).ToColor();
    }

    protected float GetTurbulence(Vector2 uv, Vector3 p)
    {
        var accum = 0f;
        var weight = 1f;
        var tempP = p;

        for (var i = 0; i < NoiseData.Turbulence; i++)
        {
            accum += weight * Noise.GetNoise(tempP.X, tempP.Y, tempP.Z);
            weight *= 0.5f;
            tempP *= 2f;
        }

        return MathF.Abs(accum);
    }
}
