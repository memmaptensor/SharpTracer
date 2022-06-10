using SharpTracer.Core.Texture;

namespace SharpTracer.Core.Material;

public interface ITexturedMaterial : IMaterial
{
    public ITexture Texture { get; set; }
}
