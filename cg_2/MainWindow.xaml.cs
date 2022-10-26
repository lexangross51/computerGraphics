using Size = System.Windows.Size;

namespace cg_2;

public partial class MainWindow
{
    private readonly MainCamera _camera = new(CameraMode.Perspective);
    private RenderServer _renderServer = default!;
    private IRenderable[] _renderables = default!;
    private ShaderProgram _lightingProgram = default!;
    private readonly Vector3 _lightPos = new(1.2f, 1.0f, 2.0f);
    private float _deltaTime;

    public MainWindow() => InitializeComponent();

    private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.W:
                _camera.Move(CameraMovement.Forward, _deltaTime);
                break;
            case Key.S:
                _camera.Move(CameraMovement.Backward, _deltaTime);
                break;
            case Key.A:
                _camera.Move(CameraMovement.Left, _deltaTime);
                break;
            case Key.D:
                _camera.Move(CameraMovement.Right, _deltaTime);
                break;
            case Key.Space:
                _camera.Move(CameraMovement.Up, _deltaTime);
                break;
            case Key.LeftCtrl:
                _camera.Move(CameraMovement.Down, _deltaTime);
                break;
        }
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

    private void Render(TimeSpan deltaTime)
    {
        _deltaTime = (float)deltaTime.TotalMilliseconds;
        GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _renderServer.Render(_camera);
    }

    private void Initialize(object? sender, EventArgs e)
    {
        var mainSettings = new GLWpfControlSettings { MajorVersion = 3, MinorVersion = 3 };
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new Size(1920, 1080); // TODO (default value is 0 or NaN)

        var width = (float)OpenTkControl.RenderSize.Width;
        var height = (float)OpenTkControl.RenderSize.Height;
    
        _lightingProgram = new(OpenTkControl);
        _lightingProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/lighting.frag");
    
        var projectionMatrix = _camera.CameraMode == CameraMode.Perspective
            ? Matrix4.CreatePerspectiveFieldOfView(0.45f,
                width / height, 0.1f, 100.0f)
            : Matrix4.CreateOrthographic(-width / 50.0f, -height / 50.0f, 0.1f, 100.0f);
        var viewMatrix = Matrix4.LookAt(_camera.Position, _camera.Position + _camera.Front, _camera.Up);
        var modelMatrix = Matrix4.Identity;
    
        _renderables = new IRenderable[]
        {
            new Instance(_lightingProgram, Primitives.Cube, new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix, "model")),
                new Lighting((Color4.Gold, "objectColor"), (new(1.0f, 1.0f, 1.0f), "lightColor"),
                    (_lightPos, "lightPos"))
            }, new()
            {
                WithNormals = true // not implemented
            }, Color4.Coral)
        };
    
        _renderables[0].Initialize(new(VertexAttribType.Float), new VertexBufferObject<float>());
    
        _renderServer = new(_renderables);
    }
}