namespace cg_2.Source.Wrappers;

public readonly record struct Vertex
{
    private readonly Vector3 _position;
    private readonly Vector3 _normal;

    public static int Size => (3 + 3) * 4;

    public Vertex(Vector3 position, Vector3 normal)
    {
        _position = position;
        _normal = normal;
    }
}