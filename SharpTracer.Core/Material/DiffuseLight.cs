using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Texture;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Material;

public class DiffuseLight : IMaterial
{
    public DiffuseLight(Vector3 albedo) => Albedo = albedo;

    public DiffuseLight(Vector3 albedo, ITexture emissionMap)
    {
        Albedo = albedo;
        EmissionMap = emissionMap;
    }

    public Vector3 Albedo { get; }
    public ITexture EmissionMap { get; }

    public bool Scatter(Ray ray, HitRecord hit, out Color attenuation, out Ray outRay)
    {
        attenuation = default;
        outRay = default;
        return false;
    }

    public Vector3 Emitted(Vector2 uv, Vector3 p)
    {
        if (EmissionMap is null)
        {
            return Albedo;
        }

        return EmissionMap.FromUV(uv, p).ToVector3() * Albedo;
    }
}
