namespace cg_2;

public partial class MainWindow
{
    private readonly MainCamera _camera = new(CameraMode.Perspective);
    private RenderServer _renderServer = default!;
    private IRenderable[] _renderables = default!;
    private ShaderProgram _lightingProgram = default!;
    private ShaderProgram _lampProgram = default!;
    private readonly Vector3 _lightPos = new(0.0f, 2.0f, 0.0f);
    private float _deltaTime;

    public MainWindow() => InitializeComponent();

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyboardDevice.IsKeyDown(Key.W)) _camera.Move(CameraMovement.Forward, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.S)) _camera.Move(CameraMovement.Backward, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.A)) _camera.Move(CameraMovement.Left, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.D)) _camera.Move(CameraMovement.Right, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.Space)) _camera.Move(CameraMovement.Up, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)) _camera.Move(CameraMovement.Down, _deltaTime);
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var pos = e.GetPosition(this);

            _camera.LookAt((float)pos.X, (float)pos.Y);
        }
        else
        {
            _camera.FirstMouse = true;
        }
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e) => _camera.Fov -= e.Delta / 100.0f;

    private void OnRender(TimeSpan deltaTime)
    {
        _deltaTime = (float)deltaTime.TotalMilliseconds;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _renderServer.Render(_camera);
    }

    private void OnInitialize(object? sender, EventArgs e)
    {
        var mainSettings = new GLWpfControlSettings();
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new(1920, 1080);

        GL.ClearColor(Color.Black);
        GL.Enable(EnableCap.DepthTest);

        var width = (float)OpenTkControl.RenderSize.Width;
        var height = (float)OpenTkControl.RenderSize.Height;
        _camera.AspectRatio = width / height;

        _lightingProgram = new();
        _lightingProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/pointLight.frag");

        _lampProgram = new();
        _lampProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/lamp.frag");

        var projectionMatrix = _camera.GetProjectionMatrix();
        var viewMatrix = _camera.GetViewMatrix();
        var modelMatrix = Matrix4.CreateTranslation(new(0.0f, -2.0f, 0.0f));
        var modelMatrix1 = Matrix4.CreateScale(0.2f);
        modelMatrix1 *= Matrix4.CreateTranslation(_lightPos);
        var model2 = Matrix4.CreateTranslation(new(-1.0f, -2.0f, 1.0f));

        _renderables = new IRenderable[]
        {
            new RenderObject(_lightingProgram, Primitives.Cube(0.5f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix, "model")),
                Lighting.BrightLight((_lightPos, "light.position"), "viewPos"),
                Material.GoldMaterial
            }),
            new RenderObject(_lampProgram, Primitives.Cube(0.5f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix1, "model"))
            }),
            new RenderObject(_lightingProgram, Primitives.Cube(0.5f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (model2, "model")),
                Lighting.BrightLight((_lightPos, "light.position"), "viewPos"),
                Material.GoldMaterial
            })
        };

        foreach (var renderable in _renderables)
        {
            renderable.Initialize(new VertexArrayObject(VertexAttribType.Float),
                new VertexBufferObject<float>());
        }

        _renderServer = new(_renderables);
    }
}