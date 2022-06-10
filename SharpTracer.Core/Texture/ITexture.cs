using System.Drawing;
using System.Numerics;

namespace SharpTracer.Core.Texture;

public interface ITexture
{
    public Color FromUV(Vector2 uv, Vector3 p);
}
