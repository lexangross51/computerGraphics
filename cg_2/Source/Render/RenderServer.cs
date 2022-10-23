namespace cg_2.Source.Render;

public class RenderServer
{
    public IEnumerable<IRenderable> Instances { get; }

    public RenderServer(IEnumerable<IRenderable> instances) => Instances = instances;

    public void Load()
    {
        foreach (var instance in Instances)
        {
            instance.Initialize(new(), new(new()));
        }
    }

    public void Render(MainCamera camera)
    {
        foreach (var instance in Instances)
        {
            instance.ShaderProgram.Push();

            instance.UpdateUniform(camera);

            instance.Vao.Bind(instance.ShaderProgram.CurrentOpenGLContext);
            instance.ShaderProgram.CurrentOpenGLContext.DrawArrays(OpenGL.GL_TRIANGLES, 0,
                36); // TODO подсчитать кол-во вершин
            instance.Vao.Unbind(instance.ShaderProgram.CurrentOpenGLContext);

            instance.ShaderProgram.Pop();
        }
    }
}