using Buffer = System.Buffer;
using Size = System.Windows.Size;

namespace cg_2;

public partial class MainWindow
{
    private readonly MainCamera _camera = new(CameraMode.Perspective);
    private RenderServer _renderServer = default!;
    private IRenderable[] _renderables = default!;
    private ShaderProgram _lightingProgram = default!;
    private ShaderProgram _lampProgram = default!;
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

    private void OnRender(TimeSpan deltaTime)
    {
        _deltaTime = (float)deltaTime.TotalMilliseconds;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _renderServer.Render(_camera);
    }

    private void OnInitialize(object? sender, EventArgs e)
    {
        var mainSettings = new GLWpfControlSettings { MajorVersion = 3, MinorVersion = 3 };
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new Size(1920, 1080); // TODO (default value is 0 or NaN)

        GL.ClearColor(Color4.RosyBrown);
        GL.Enable(EnableCap.DepthTest);

        var width = (float)OpenTkControl.RenderSize.Width;
        var height = (float)OpenTkControl.RenderSize.Height;
        _camera.AspectRatio = width / height;

        _lightingProgram = new(OpenTkControl);
        _lightingProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/lighting.frag");

        _lampProgram = new(OpenTkControl);
        _lampProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/lamp.frag");

        var projectionMatrix = _camera.GetProjectionMatrix();
        var viewMatrix = _camera.GetViewMatrix();
        var modelMatrix = Matrix4.Identity;
        var modelMatrix1 = Matrix4.CreateScale(0.2f);
        modelMatrix1 *= Matrix4.CreateTranslation(_lightPos.X, _lightPos.Y, _lightPos.Z);

        _renderables = new IRenderable[]
        {
            new Instance(_lightingProgram, Primitives.Cube, new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix, "model")),
                new Lighting((new Color4(1.0f, 0.0f, 0.0f, 1.0f), "objectColor"), (new(1.0f), "lightColor"),
                    (_lightPos, "lightPos"))
            }),
            new Instance(_lampProgram, Primitives.Cube, new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix1, "model"))
            })
        };

        foreach (var renderable in _renderables)
        {
            renderable.Initialize(new(VertexAttribType.Float), new VertexBufferObject<float>());
        }

        _renderServer = new(_renderables);
    }
}