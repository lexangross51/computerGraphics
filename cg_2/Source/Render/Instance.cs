namespace cg_2.Source.Render;

public class Instance : IRenderable
{
    public VertexBufferWrapper Vbo { get; }
    public float[] Vertices { get; }

    public Instance(VertexBufferWrapper vbo, float[] vertices)
        => (Vbo, Vertices) = (vbo, vertices);
}