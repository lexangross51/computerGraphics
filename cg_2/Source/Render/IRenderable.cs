namespace cg_2.Source.Render;

public interface IRenderable
{
    public ShaderProgramWrapper ShaderProgram { get; }
    public VertexBufferArray Vao { get; }
    public float[] Vertices { get; }
    public IUniformContext[] UniformContext { get; }
    public MaterialColor MaterialColor { get; }

    public void Initialize(VertexBufferArray vao, VertexBufferWrapper vbo);

    public void UpdateUniform(MainCamera camera);
}