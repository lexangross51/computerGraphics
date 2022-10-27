namespace cg_2.Source.Wrappers;

public readonly record struct Vertex
{
    private readonly Vector3 _position;
    private readonly Vector3 _normal;
    private readonly Vector2 _texture;

    public int Size => (3 + 3 + 2) * 4;

    public Vertex(Vector3 position, Vector3 normal, Vector2 texture)
    {
        _position = position;
        _normal = normal;
        _texture = texture;
    }
}