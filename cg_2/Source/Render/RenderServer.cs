namespace cg_2.Source.Render;

public interface IBaseGraphic
{
    public float DeltaTime { get; }
    public MainCamera Camera { get; }
    
    public void Render(TimeSpan obj);
    public void Draw(IEnumerable<IRenderable> renderObjects);
}

public interface IViewable
{
    public void Draw(IBaseGraphic baseGraphic);
}

public class RenderServer : ReactiveObject, IBaseGraphic
{
    public float DeltaTime { get; private set; }
    public MainCamera Camera { get; }
    public IEnumerable<IRenderable>? RenderObjects { get; set; }

    public RenderServer(MainCamera? camera = null)
    {
        GL.ClearColor(Color.Black);
        GL.Enable(EnableCap.DepthTest);
        Camera = camera ?? new(CameraMode.Perspective);
    }

    public void Draw(IEnumerable<IRenderable> renderObjects)
    {
        RenderObjects = renderObjects;

        foreach (var @object in RenderObjects)
        {
            @object.Initialize(new(VertexAttribType.Float),
                new VertexBufferObject<float>());
        }
    }

    public void Render(TimeSpan deltaTime)
    {
        DeltaTime = (float)deltaTime.TotalMilliseconds;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (RenderObjects is null) return;
        foreach (var renderObject in RenderObjects)
        {
            renderObject.Vao!.Bind();
            renderObject.ShaderProgram.Use();
            renderObject.UpdateUniform(Camera);
            GL.DrawArrays(renderObject.PrimitiveType, 0, renderObject.Vertices.Length);
        }
    }

    public void UpdateUniforms(IUniformContext[] uniformContexts) // TODO
    {
        if (RenderObjects is null) return;

        foreach (var renderObject in RenderObjects)
        {
            renderObject.UniformContext = uniformContexts;
        }
    }
}