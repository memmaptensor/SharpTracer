using System.Numerics;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Texture;

public struct NoiseData
{
    public NoiseData(
        FastNoiseLite.NoiseType noiseType,
        int seed,
        float frequency,
        Vector3 scale,
        Vector3 offset,
        int turbulence = 1)
    {
        NoiseType = noiseType;
        Seed = seed;
        Frequency = frequency;
        Scale = scale;
        Offset = offset;
        Turbulence = turbulence;
    }

    public FastNoiseLite.NoiseType NoiseType;
    public int Seed;
    public float Frequency;
    public Vector3 Scale;
    public Vector3 Offset;
    public int Turbulence;
}
