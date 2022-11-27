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
    public void DrawPoints(IEnumerable<Vector2D> points);
}

public interface IViewable
{
    public void Draw(IBaseGraphic baseGraphic);
}

public class RenderServer : ReactiveObject, IBaseGraphic
{
    private IUniformContext? _uniformContext;
    private VertexArrayObject? _vao;
    private ShaderProgram? _shaderProgram;
    public float DeltaTime { get; private set; }

    public MainCamera Camera { get; }
    public IEnumerable<Vector2D>? Points { get; set; }

    public RenderServer(MainCamera? camera = null)
    {
        GL.ClearColor(Color.White);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.ProgramPointSize);
        GL.Enable(EnableCap.LineSmooth);
        Camera ??= new(CameraMode.Perspective);
    }

    public void DrawPoints(IEnumerable<Vector2D> points)
    {
        _vao = new(VertexAttribType.Float);
        var vbo = new VertexBufferObject<float>();
        var vector2Ds = points as Vector2D[] ?? points.ToArray();
        Points = vector2Ds;

        vbo.Bind();
        vbo.BufferData(vector2Ds.ToArray());

        _vao.Bind();

        GL.VertexArrayAttribBinding(_vao.Handle, 0, 0);
        GL.EnableVertexArrayAttrib(_vao.Handle, 0);
        GL.VertexArrayAttribFormat(_vao.Handle, 0, 2, _vao.VertexAttributeType, false, 0);
        GL.VertexArrayVertexBuffer(_vao.Handle, 0, vbo.Handle, IntPtr.Zero, Vector2D.Size);

        _shaderProgram = new();
        _shaderProgram.Initialize("Source/Shaders/shader.vert", "Source/Shaders/shader.frag",
            "Source/Shaders/shader.geom");

        var projectionMatrix = Matrix4.Identity;
        var viewMatrix = Matrix4.Identity;
        var modelMatrix = Matrix4.Identity;

        _uniformContext = new Transformation
        {
            View = (viewMatrix, "view"),
            Projection = (projectionMatrix, "projection"),
            Model = (modelMatrix, "model")
        };
    }
    

    public void Render(TimeSpan deltaTime)
    {
        DeltaTime = (float)deltaTime.TotalMilliseconds;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (Points is null) return;

        _shaderProgram!.Use();
        _uniformContext!.Update(_shaderProgram, Camera);
        _vao!.Bind();
        GL.DrawArrays(PrimitiveType.LineStrip, 0, Points.Count());
    }
}