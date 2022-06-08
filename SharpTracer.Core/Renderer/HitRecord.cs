using System.Numerics;
using SharpTracer.Core.Material;

namespace SharpTracer.Core.Renderer;

public struct HitRecord
{
    public IMaterial Material;
    public Vector3 Position;
    public Vector3 Normals;
    public double T;
    public bool IsFrontFace;

    public HitRecord(IMaterial material, Vector3 position, Vector3 normals, double t)
    {
        Material = material;
        Position = position;
        Normals = normals;
        T = t;
        IsFrontFace = false;
    }

    public void SetFaceNormal(Ray ray, Vector3 outwardNormal)
    {
        IsFrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0f;
        Normals = IsFrontFace ? outwardNormal : -outwardNormal;
    }
}
