namespace cg_2.Source.Render;

public interface IRenderable
{
    public ShaderProgramWrapper ShaderProgram { get; }
    public float[] Vertices { get; }
}