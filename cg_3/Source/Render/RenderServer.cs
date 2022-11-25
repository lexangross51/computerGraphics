using System.Drawing;
using cg_3.Source.Vectors;
using OpenTK.Graphics.OpenGL4;
using ReactiveUI;

namespace cg_3.Source.Render;

public interface IBaseGraphic
{
    public float DeltaTime { get; }
    
    public void Render(TimeSpan obj);
    public void DrawPoints(IEnumerable<Vector2D> points);
}

public interface IViewable
{
    public void Draw(IBaseGraphic baseGraphic);
}

public class RenderServer : ReactiveObject, IBaseGraphic
{
    public float DeltaTime { get; private set; }

    // public MainCamera Camera { get; }
    public IEnumerable<Vector2D>? Points { get; set; }

    // public RenderServer(MainCamera? camera = null)
    // {
    //     GL.ClearColor(Color.Black);
    //     GL.Enable(EnableCap.DepthTest);
    //     Camera = camera ?? new(CameraMode.Perspective);
    // }

    public RenderServer()
    {
        GL.ClearColor(Color.Black);
        GL.Enable(EnableCap.DepthTest);
    }

    public void DrawPoints(IEnumerable<Vector2D> renderObjects)
    {
    }

    public void Render(TimeSpan deltaTime)
    {
        DeltaTime = (float)deltaTime.TotalMilliseconds;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


    }
}