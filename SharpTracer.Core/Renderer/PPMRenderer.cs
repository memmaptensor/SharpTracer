using System.Drawing;
using System.Text;

namespace SharpTracer.Core.Renderer;

public class PPMRenderer : IRenderer
{
    public PPMRenderer(int width, int height)
    {
        Width = width;
        Height = height;
        Data = new byte[Width * Height * 3];

        string header = $"P6\n{Width} {Height}\n255\n";
        Header = Encoding.ASCII.GetBytes(header);
    }

    public byte[] Header { get; }
    public byte[] Data { get; private set; }
    public int Width { get; }
    public int Height { get; }

    public void SetPixel(int x, int y, Color color)
    {
        y = Height - 1 - y;
        int offset = (y * Width + x) * 3;
        Data[offset] = color.R;
        Data[offset + 1] = color.G;
        Data[offset + 2] = color.B;
    }

    public Color GetPixel(int x, int y)
    {
        y = Height - 1 - y;
        int offset = (y * Width + x) * 3;
        byte r = Data[offset];
        byte g = Data[offset + 1];
        byte b = Data[offset + 2];
        return Color.FromArgb(r, g, b);
    }

    public void Clear() => Data = new byte[Width * Height * 3];

    public async Task WriteToFile(string fullPath)
    {
        using (FileStream fs = new(fullPath, FileMode.Create, FileAccess.Write))
        {
            await fs.WriteAsync(Header, 0, Header.Length);
            await fs.WriteAsync(Data, 0, Data.Length);
        }
    }
}
