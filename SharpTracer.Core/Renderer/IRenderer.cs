using System.Drawing;

namespace SharpTracer.Core.Renderer;

public interface IRenderer
{
    public int Width { get; }
    public int Height { get; }
    public void SetPixel(int x, int y, Color color);
    public Color GetPixel(int x, int y);
    public void Clear();
}
