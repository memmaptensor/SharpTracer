using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Texture;

public class NoiseTexture : ITexture
{
    public NoiseTexture(FastNoiseLite noise, NoiseData noiseData, Color color)
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

    public Color FromUV(Vector2 uv, Vector3 pt)
    {
        Vector3 p = new Vector3(
            pt.X * NoiseData.Scale.X,
            pt.Y * NoiseData.Scale.Y,
            pt.Z * NoiseData.Scale.Z) + NoiseData.Offset;

        return ((Noise.GetNoise(p.X, p.Y, p.Z) * 0.5f + 0.5f) * Color.ToVector3()).ToColor();
    }
}
