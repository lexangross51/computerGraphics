namespace cg_2.Source.Render;

public interface IRenderable
{
    public ShaderProgramWrapper ShaderProgram { get; }
    public VertexBufferArray Vao { get; }
    public float[] Vertices { get; }
    public MaterialColor MaterialColor { get; }
}