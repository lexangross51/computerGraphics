namespace cg_2.Model.Source.Extensions;

public static class GlmVectorExtensions
{
    public static float Norm(this vec3 vector)
        => (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);

    public static float Norm(this vec4 vector)
        => Norm(new vec3(vector));

    public static vec3 MassCenter(this IEnumerable<vec3> collection)
    {
        float x = 0, y = 0, z = 0;

        var enumerable = collection as vec3[] ?? collection.ToArray();

        foreach (var p in enumerable)
        {
            x += p.x;
            y += p.y;
            z += p.z;
        }

        x /= enumerable.Length;
        y /= enumerable.Length;
        z /= enumerable.Length;

        return new(x, y, z);
    }
}