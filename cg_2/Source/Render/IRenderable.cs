namespace cg_2.Source.Render;

public interface IRenderable
{
    public ShaderProgram ShaderProgram { get; }
    public VertexArrayObject? Vao { get; }
    public Vertex[] Vertices { get; }
    public IUniformContext[] UniformContext { get; }
    public PrimitiveType PrimitiveType { get; }

    public void Initialize<T>(VertexArrayObject vao, VertexBufferObject<T> vbo) where T : unmanaged;

    public void UpdateUniform(MainCamera camera);
}