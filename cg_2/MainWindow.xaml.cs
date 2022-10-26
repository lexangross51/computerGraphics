namespace cg_2;

public partial class MainWindow
{
    private readonly MainCamera _camera = new(CameraMode.Perspective);
    private float _deltaTime;

    public MainWindow()
    {
        InitializeComponent();
        var mainSettings = new GLWpfControlSettings {MajorVersion = 4, MinorVersion = 6};
        OpenTkControl.Start(mainSettings);
    }

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
    }

    private void Initialize(object? sender, EventArgs e)
    {
        // var width = (float)Control.Width;
        // var height = (float)Control.Height;
        //
        // var projectionMatrix = _isPerspective
        //     ? Matrix4.CreatePerspectiveFieldOfView(45.0f, width / (float)height, 0.1f, 100.0f)
        //     : glm.ortho(-width / 50f, width / 50f, -height / 50f, height / 50f, 0.1f, 100);
        // var viewMatrix = glm.lookAt(_camera.Position, _camera.Position + _camera.Front, _camera.Up);
        // var modelMatrix = mat4.identity();
        //
        // _renderables = new IRenderable[]
        // {
        //     new SolidInstance(_lightingProgram, Primitives.Cube, new IUniformContext[]
        //     {
        //         new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix, "model")),
        //         new Lighting(new(new(Color.Coral), "objectColor"), (new(1.0f, 1.0f, 1.0f), "lightColor"),
        //             (_lightPos, "lightPos"))
        //     })
        // };
        //
        // _renderServer = new(_renderables);
        // _renderServer.Load();
    }
}