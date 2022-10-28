namespace cg_2.Source.Render;

public class RenderServer
{
    public IEnumerable<IRenderable> RenderObjects { get; }

    public RenderServer(IEnumerable<IRenderable> renderObjects) => RenderObjects = renderObjects;

    public void Render(MainCamera camera)
    {
        foreach (var renderObject in RenderObjects)
        {
            renderObject.Vao.Bind();
            renderObject.ShaderProgram.Use();
            renderObject.UpdateUniform(camera);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }
}