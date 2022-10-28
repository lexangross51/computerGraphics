namespace cg_2.Source.Wrappers;

public record struct Vertex
{
    public Vector3 Position { get; set; }
    public Vector3 Normal { get; set; }

    public static int Size => (3 + 3) * 4;

    public Vertex(Vector3 position, Vector3 normal)
    {
        Position = position;
        Normal = normal;
    }
}