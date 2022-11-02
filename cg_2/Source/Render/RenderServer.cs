using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_2.Source.Render;

public interface IBaseGraphic
{
    public float DeltaTime { get; set; }
    public IEnumerable<IRenderable>? RenderObjects { get; set; }
    public MainCamera Camera { get; }

    public void Render();
}

public class RenderServer : ReactiveObject, IBaseGraphic
{
    [Reactive] public float DeltaTime { get; set; }
    public IEnumerable<IRenderable>? RenderObjects { get; set; }
    public MainCamera Camera { get; }

    public RenderServer(MainCamera? camera = null) => Camera = camera ?? new(CameraMode.Perspective);

    public void Render()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (RenderObjects == null) return;
        foreach (var renderObject in RenderObjects)
        {
            renderObject.Vao.Bind();
            renderObject.ShaderProgram.Use();
            renderObject.UpdateUniform(Camera);
            GL.DrawArrays(renderObject.PrimitiveType, 0, renderObject.Vertices.Length);
        }
    }
}