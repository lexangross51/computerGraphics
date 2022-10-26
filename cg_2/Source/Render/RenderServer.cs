namespace cg_2.Source.Render;

public class RenderServer
{
    public IEnumerable<IRenderable> Instances { get; }

    public RenderServer(IEnumerable<IRenderable> instances) => Instances = instances;

    public void Render(MainCamera camera)
    {
        foreach (var instance in Instances)
        {
            instance.ShaderProgram.Use();

            instance.UpdateUniform(camera);

            instance.Vao.Bind();
            GL.DrawArrays(instance.PrimitiveType, 0, 36);
        }
    }
}