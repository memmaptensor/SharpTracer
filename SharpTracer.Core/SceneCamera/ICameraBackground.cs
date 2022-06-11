using System.Numerics;
using SharpTracer.Core.Renderer;

namespace SharpTracer.Core.SceneCamera;

public interface ICameraBackground
{
    public Vector3 GetScreenSpaceColor(Ray ray);
}
