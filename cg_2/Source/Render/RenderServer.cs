namespace cg_2.Source.Render;

public class RenderServer
{
    public IEnumerable<IRenderable> Instances { get; }

    public RenderServer(IEnumerable<IRenderable> instances) => Instances = instances;

    // TODO -> момент с Update()
    public void Render()
    {
        foreach (var instance in Instances)
        {
            instance.Vao.Bind( instance.ShaderProgram.CurrentOpenGLContext);
            instance.ShaderProgram.CurrentOpenGLContext.DrawArrays(OpenGL.GL_TRIANGLES, 0, 36);
            instance.Vao.Unbind( instance.ShaderProgram.CurrentOpenGLContext);
        }
    }
}