namespace cg_2.Source.Extensions;

public static class GlmVectorExtensions
{
    public static Vector3 MassCenter(this IEnumerable<Vector3> collection)
    {
        float x = 0, y = 0, z = 0;

        var enumerable = collection as Vector3[] ?? collection.ToArray();

        foreach (var p in enumerable)
        {
            x += p.X;
            y += p.Y;
            z += p.Z;
        }

        x /= enumerable.Length;
        y /= enumerable.Length;
        z /= enumerable.Length;

        return new(x, y, z);
    }
}