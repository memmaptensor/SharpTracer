using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Geometry;
using SharpTracer.Core.Renderer;
using SharpTracer.Core.Texture;
using SharpTracer.Core.Utility;

namespace SharpTracer.Core.Material;

public class TexturedMaterial : ITexturedMaterial
{
    public TexturedMaterial(ITexture texture) => Texture = texture;

    public ITexture Texture { get; set; }

    public bool Scatter(Ray ray, HitRecord hit, out Color attenuation, out Ray outRay)
    {
        Vector3 scatterDir = hit.Normals + Vector3.Normalize(Sphere.RandomPointInSphere());
        if (scatterDir.IsNearZero())
        {
            scatterDir = hit.Normals;
        }

        outRay = new Ray(hit.Position, scatterDir, ray.Time);
        attenuation = Texture.FromUV(hit.UV, hit.Position);

        return true;
    }
}
