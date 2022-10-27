namespace cg_2.Source.Render;

public class RenderServer
{
    public IEnumerable<IRenderable> Instances { get; }

    public RenderServer(IEnumerable<IRenderable> instances) => Instances = instances;

    public void Render(MainCamera camera)
    {
        foreach (var instance in Instances)
        {
            instance.Vao.Bind();
            instance.ShaderProgram.Use();
            instance.UpdateUniform(camera);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }
}