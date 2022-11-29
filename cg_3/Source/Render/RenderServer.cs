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
    float DeltaTime { get; }
    MainCamera Camera { get; }

    void Render(TimeSpan obj);
    void Draw(RenderUnit renderUnit);
    void Draw(IEnumerable<Vector2D> points, PrimitiveType primitiveType);
    void DrawPoints(IEnumerable<Vector2D> points);
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

    public void Draw(RenderUnit renderUnit) => _renderables.Add(renderUnit);

    public void Draw(IEnumerable<Vector2D> points, PrimitiveType primitiveType)
    {
        RenderUnit @object = new RenderObject(primitiveType: primitiveType);
        @object.Initialize(points);
        _renderables.Add(@object);
    }

    public void DrawPoints(IEnumerable<Vector2D> points)
    {
        ShaderProgram shaderProgram = new();
        shaderProgram.Initialize("Source/Shaders/shader1.vert", "Source/Shaders/shader.frag");

        RenderUnit @object = new RenderObject(shaderProgram: shaderProgram);
        @object.Initialize(points);

        _renderables.Add(@object);
    }

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