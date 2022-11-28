using System.Drawing;
using cg_3.Source.Camera;
using cg_3.Source.Vectors;
using cg_3.Source.Wrappers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ReactiveUI;

namespace cg_3.Source.Render;

public interface IBaseGraphic
{
    public float DeltaTime { get; }
    public MainCamera Camera { get; }

    public void Render(TimeSpan obj);
    public void DrawLines(IEnumerable<Vector2D> points);
    public void DrawPoints(IEnumerable<Vector2D> points);
}

public interface IViewable
{
    public void Draw(IBaseGraphic baseGraphic);
}

public class RenderServer : ReactiveObject, IBaseGraphic
{
    private readonly List<IRenderable> _renderables;
    public float DeltaTime { get; private set; }
    public MainCamera Camera { get; }

    public RenderServer(MainCamera? camera = null)
    {
        GL.ClearColor(Color.White);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.ProgramPointSize);
        GL.Enable(EnableCap.LineSmooth);
        Camera ??= new(CameraMode.Perspective);
        _renderables = new();
    }

    public void DrawPoints(IEnumerable<Vector2D> points)
        => _renderables.Add(new RenderObject().Initialize(points));

    public void DrawLines(IEnumerable<Vector2D> points)
        => _renderables.Add(new RenderObject(primitiveType: PrimitiveType.LineStrip).Initialize(points));

    public void Render(TimeSpan deltaTime)
    {
        DeltaTime = (float)deltaTime.TotalMilliseconds;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (!_renderables.Any()) return;

        foreach (var renderable in _renderables)
        {
            renderable.ShaderProgram!.Use();
            renderable.UniformContext.Update(renderable.ShaderProgram, Camera);
            renderable.Vao.Bind();
            GL.DrawArrays(renderable.PrimitiveType, 0, renderable.VerticesSize);
        }
    }
}