using System.Drawing;
using BigGustave;

namespace SharpTracer.Core.Renderer;

public class PNGRenderer : IRenderer
{
    public PNGRenderer(int width, int height)
    {
        Width = width;
        Height = height;
        Data = new byte[Width * Height * 4];
    }

    public byte[] Data { get; private set; }
    public int Width { get; }
    public int Height { get; }

    public void SetPixel(int x, int y, Color color)
    {
        y = Height - 1 - y;
        int offset = (y * Width + x) * 4;
        Data[offset] = color.R;
        Data[offset + 1] = color.G;
        Data[offset + 2] = color.B;
        Data[offset + 3] = color.A;
    }

    public Color GetPixel(int x, int y)
    {
        y = Height - 1 - y;
        int offset = (y * Width + x) * 4;
        byte r = Data[offset];
        byte g = Data[offset + 1];
        byte b = Data[offset + 2];
        byte a = Data[offset + 3];
        return Color.FromArgb(a, r, g, b);
    }

    public void Clear() => Data = new byte[Width * Height * 4];

    public void WriteToFile(string fullPath)
    {
        PngBuilder builder = PngBuilder.FromBgra32Pixels(Data, Width, Height);
        using (FileStream fs = new(fullPath, FileMode.Create, FileAccess.Write))
        {
            builder.Save(fs);
        }
    }
}
