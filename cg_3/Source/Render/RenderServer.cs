namespace cg_3.Source.Render;

public interface IBaseGraphic
{
    MainCamera Camera { get; }
    Projection Projection { get; }

    void Clear();
    void Render(TimeSpan deltaTime);
    void Draw(RenderUnit renderUnit);
    void Draw(IEnumerable<Vector2D> points, PrimitiveType primitiveType, Color4? color = null);
    void DrawPoints(IEnumerable<Vector2D> points, int pointSize = 2, Color4? color = null);
}

public interface IViewable
{
    public void Draw(IBaseGraphic baseGraphic);
}

public class RenderServer : ReactiveObject, IBaseGraphic
{
    private readonly List<IRenderable> _renderables;
    private readonly List<IRenderable> _planeContext;
    public float DeltaTime { get; private set; }
    public MainCamera Camera { get; }
    public Projection Projection { get; }

    public RenderServer(MainCamera? camera = null)
    {
        GL.ClearColor(Color4.WhiteSmoke);
        GL.Enable(EnableCap.ProgramPointSize);
        GL.Enable(EnableCap.LineSmooth);
        Camera ??= new(CameraMode.Perspective);
        Projection = new(-20.0f, 20.0f, -20.0f, 20.0f);
        _renderables = new();
        _planeContext = new();

        this.WhenAnyValue(t => t.Projection.Width, t => t.Projection.Height)
            .Subscribe(_ => RedrawAxes());
    }

    public void Clear() => _renderables.Clear();

    public void Draw(RenderUnit renderUnit) => _renderables.Add(renderUnit);

    public void Draw(IEnumerable<Vector2D> points, PrimitiveType primitiveType, Color4? color = null)
    {
        RenderUnit @object = new RenderObject(primitiveType: primitiveType, uniformContexts: new IUniformContext[]
        {
            new Transformation
            {
                View = (Matrix4.Identity, "view"),
                Projection = (
                    Matrix4.CreateOrthographicOffCenter(Projection.Left, Projection.Right, Projection.Bottom,
                        Projection.Top, -1.0f, 1.0f), "projection"),
                Model = (Matrix4.Identity, "model")
            },
            new ColorUniform
            {
                Context = (color ?? Color4.Black, "color")
            }
        });
        @object.Initialize(points);
        _renderables.Add(@object);
    }

    public void DrawPoints(IEnumerable<Vector2D> points, int pointsSize = 2, Color4? color = null)
    {
        ShaderProgram shaderProgram = new();
        shaderProgram.Initialize("Source/Shaders/pointsShader.vert", "Source/Shaders/shader.frag");

        RenderUnit @object = new RenderObject(shaderProgram: shaderProgram, uniformContexts: new IUniformContext[]
        {
            new Transformation
            {
                View = (Matrix4.Identity, "view"),
                Projection = (
                    Matrix4.CreateOrthographicOffCenter(Projection.Left, Projection.Right, Projection.Bottom,
                        Projection.Top, -1.0f, 1.0f), "projection"),
                Model = (Matrix4.Identity, "model")
            },
            new ColorUniform
            {
                Context = (color ?? Color4.Black, "color")
            },
            new PointSizeUniform
            {
                Context = (pointsSize, "size")
            }
        });
        @object.Initialize(points);
        _renderables.Add(@object);
    }

    public void Render(TimeSpan deltaTime)
    {
        DeltaTime = (float)deltaTime.TotalMilliseconds;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        foreach (var renderable in _planeContext)
        {
            renderable.ShaderProgram!.Use();
            renderable.UpdateUniforms(Camera);
            renderable.Vao.Bind();
            GL.DrawArrays(renderable.PrimitiveType, 0, renderable.VerticesSize);
        }

        if (!_renderables.Any()) return;

        foreach (var renderable in _renderables)
        {
            renderable.ShaderProgram!.Use();
            renderable.UpdateUniforms(Camera);
            renderable.Vao.Bind();
            GL.DrawArrays(renderable.PrimitiveType, 0, renderable.VerticesSize);
        }
    }

    private void RedrawAxes()
    {
        const float offset = 0.2f;
        const int partitions = 20;

        _planeContext.Clear();

        var stepX = Projection.Width / partitions;
        var stepY = Projection.Height / partitions;

        var bottomPoints = new Vector2D[partitions];
        var topPoints = new Vector2D[partitions];
        var leftPoints = new Vector2D[partitions];
        var rightPoints = new Vector2D[partitions];

        bottomPoints[0] = (Projection.Left + offset + stepX, Projection.Bottom + offset);
        topPoints[0] = (Projection.Left + offset + stepX, Projection.Top);
        leftPoints[0] = (Projection.Left + offset, Projection.Bottom + offset + stepY);
        rightPoints[0] = (Projection.Right, Projection.Bottom + offset + stepY);

        for (int i = 1; i < partitions; i++)
        {
            bottomPoints[i] = bottomPoints[i - 1] + (stepX, 0.0f);
            topPoints[i] = topPoints[i - 1] + (stepX, 0.0f);
            leftPoints[i] = leftPoints[i - 1] + (0.0f, stepY);
            rightPoints[i] = rightPoints[i - 1] + (0.0f, stepY);
        }

        var linesOrdinatePoints = new Vector2D[bottomPoints.Length + topPoints.Length + 1];
        var linesAbscissaPoints = new Vector2D[leftPoints.Length + rightPoints.Length + 1];
        int k = 0;
        int j = 0;

        for (int i = 0; i < bottomPoints.Length; i++)
        {
            linesOrdinatePoints[k++] = bottomPoints[i];
            linesOrdinatePoints[k++] = topPoints[i];
            linesAbscissaPoints[j++] = leftPoints[i];
            linesAbscissaPoints[j++] = rightPoints[i];
        }

        var linesPoints = new Vector2D[]
        {
            (Projection.Left + offset, Projection.Bottom + offset), (Projection.Left + offset, Projection.Top),
            (Projection.Left + offset, Projection.Bottom + offset), (Projection.Right, Projection.Bottom + offset),
        };

        RenderUnit fstObject = new RenderObject(primitiveType: PrimitiveType.Lines,
            uniformContexts: new IUniformContext[]
            {
                new Transformation
                {
                    View = (Matrix4.Identity, "view"),
                    Projection = (
                        Matrix4.CreateOrthographicOffCenter(Projection.Left, Projection.Right, Projection.Bottom,
                            Projection.Top, -1.0f, 1.0f), "projection"),
                    Model = (Matrix4.Identity, "model")
                },
                new ColorUniform
                {
                    Context = (Color4.Black, "color")
                }
            });
        fstObject.Initialize(linesPoints);

        RenderUnit sndObject = new RenderObject(primitiveType: PrimitiveType.Lines,
            uniformContexts: new IUniformContext[]
            {
                new Transformation
                {
                    View = (Matrix4.Identity, "view"),
                    Projection = (
                        Matrix4.CreateOrthographicOffCenter(Projection.Left, Projection.Right, Projection.Bottom,
                            Projection.Top, -1.0f, 1.0f), "projection"),
                    Model = (Matrix4.Identity, "model")
                },
                new ColorUniform
                {
                    Context = (Color4.LightGray, "color")
                }
            });
        sndObject.Initialize(linesOrdinatePoints);
        
        RenderUnit thdObject = new RenderObject(primitiveType: PrimitiveType.Lines,
            uniformContexts: new IUniformContext[]
            {
                new Transformation
                {
                    View = (Matrix4.Identity, "view"),
                    Projection = (
                        Matrix4.CreateOrthographicOffCenter(Projection.Left, Projection.Right, Projection.Bottom,
                            Projection.Top, -1.0f, 1.0f), "projection"),
                    Model = (Matrix4.Identity, "model")
                },
                new ColorUniform
                {
                    Context = (Color4.LightGray, "color")
                }
            });
        thdObject.Initialize(linesAbscissaPoints);

        _planeContext.Add(thdObject);
        _planeContext.Add(sndObject);
        _planeContext.Add(fstObject);
    }
}