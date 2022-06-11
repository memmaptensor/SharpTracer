using System.Numerics;

namespace SharpTracer.Core.Renderer;

public class HitTransformer : IHittable
{
    public HitTransformer(IHittable original, Matrix4x4 transform)
    {
        Original = original;
        Transform = transform;
    }

    public Matrix4x4 Transform { get; }
    public IHittable Original { get; }

    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hit)
    {
        Matrix4x4.Invert(Transform, out Matrix4x4 invertedTransform);
        Vector3 movedRayOrigin = Vector3.Transform(ray.Origin, invertedTransform);
        Vector3 movedRayDirection = Vector3.TransformNormal(ray.Direction, invertedTransform);
        Ray movedRay = new(movedRayOrigin, movedRayDirection, ray.Time);
        if (!Original.Hit(movedRay, tMin, tMax, ref hit))
        {
            return false;
        }

        hit.Position = Vector3.Transform(hit.Position, Transform);
        hit.SetFaceNormal(movedRay, Vector3.TransformNormal(hit.Normals, Transform));

        return true;
    }

    public AABB BoundingBox(float time0, float time1)
    {
        AABB originalBox = Original.BoundingBox(time0, time1);
        if (originalBox is null)
        {
            return null;
        }

        Vector3[] transformedPoints =
        {
            Vector3.Transform(originalBox.Min, Transform), Vector3.Transform(originalBox.Max, Transform),
            Vector3.Transform(new Vector3(originalBox.Min.X, originalBox.Min.Y, originalBox.Max.Z), Transform),
            Vector3.Transform(new Vector3(originalBox.Min.X, originalBox.Max.Y, originalBox.Min.Z), Transform),
            Vector3.Transform(new Vector3(originalBox.Max.X, originalBox.Min.Y, originalBox.Min.Z), Transform),
            Vector3.Transform(new Vector3(originalBox.Min.X, originalBox.Max.Y, originalBox.Max.Z), Transform),
            Vector3.Transform(new Vector3(originalBox.Max.X, originalBox.Max.Y, originalBox.Min.Z), Transform),
            Vector3.Transform(new Vector3(originalBox.Max.X, originalBox.Min.Y, originalBox.Max.Z), Transform)
        };

        Vector3 min = transformedPoints[0], max = transformedPoints[0];
        for (int i = 1; i < transformedPoints.Length; i++)
        {
            min = Vector3.Min(min, transformedPoints[i]);
            max = Vector3.Max(max, transformedPoints[i]);
        }

        AABB boundingBox = new(min, max);
        return boundingBox;
    }
}
