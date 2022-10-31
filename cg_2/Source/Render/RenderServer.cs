using ReactiveUI;

namespace cg_2.Source.Render;

public interface IBaseGraphic
{
    public float DeltaTime { get; set; }
    public IEnumerable<IRenderable>? RenderObjects { set; }

    public void Render(MainCamera camera);
}

public class RenderServer : ReactiveObject, IBaseGraphic
{
    private float _deltaTime;

    public float DeltaTime
    {
        get => _deltaTime;
        set => this.RaiseAndSetIfChanged(ref _deltaTime, value);
    }

    public IEnumerable<IRenderable>? RenderObjects { get; set; }

    public void Render(MainCamera camera)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (RenderObjects == null) return;
        foreach (var renderObject in RenderObjects)
        {
            renderObject.Vao.Bind();
            renderObject.ShaderProgram.Use();
            renderObject.UpdateUniform(camera);
            GL.DrawArrays(renderObject.PrimitiveType, 0, renderObject.Vertices.Length);
        }
    }
}