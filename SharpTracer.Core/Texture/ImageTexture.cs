using System.Drawing;
using System.Numerics;
using SharpTracer.Core.Utility;
using SimpleImageIO;

namespace SharpTracer.Core.Texture;

public class ImageTexture : ITexture, IDisposable
{
    public ImageTexture(string filename) => Image = new RgbImage(filename);

    public RgbImage Image { get; }

    public void Dispose() => Image?.Dispose();

    public Color FromUV(Vector2 uv, Vector3 p)
    {
        if (Image is null)
        {
            return Color.DeepPink;
        }

        // Flip Y coords
        uv.Y = 1f - uv.Y;

        // UV range -> [0, 1]
        int texX = (int)Remap.Map(uv.X, 0f, 1f, 0, Image.Width - 1);
        int texY = (int)Remap.Map(uv.Y, 0f, 1f, 0, Image.Height - 1);
        RgbColor color = Image.GetPixel(texX, texY);
        return ColorHelper.FromRGBAF(color.R, color.G, color.B);
    }
}
