namespace cg_2.Source.Render;

public class RenderServer
{
    public IEnumerable<IRenderable> Instances { get; }

    public RenderServer(IEnumerable<IRenderable> instances) => Instances = instances;

    public void Render()
    {
        foreach (var instance in Instances)
        {
            instance.ShaderProgram.Push();

            // TODO something 

            instance.ShaderProgram.Pop();
        }
    }
}