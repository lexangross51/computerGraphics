namespace cg_2.Source.Render;

public interface IRenderable
{
    public VertexBufferWrapper Vbo { get; }
    public float[] Vertices { get; }
}