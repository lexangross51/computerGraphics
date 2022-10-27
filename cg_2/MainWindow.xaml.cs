using System.Windows.Media;
using Color = System.Drawing.Color;

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
    private VertexBufferObject<float> _vbo;
    private VertexArrayObject _lVao;
    private VertexArrayObject _lampVao;

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

        // _renderServer.Render(_camera);

        _lVao.Bind();

        _lightingProgram.Use();

        _lightingProgram.SetUniform("model", Matrix4.Identity);
        _lightingProgram.SetUniform("view", _camera.GetViewMatrix());
        _lightingProgram.SetUniform("projection", _camera.GetProjectionMatrix());

        _lightingProgram.SetUniform("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
        _lightingProgram.SetUniform("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
        _lightingProgram.SetUniform("lightPos", _lightPos);
        _lightingProgram.SetUniform("viewPos", _camera.Position);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        
        _lampVao.Bind();

        _lampProgram.Use();

        _lampProgram.SetUniform("model", Matrix4.CreateScale(0.2f)  * Matrix4.CreateTranslation(_lightPos));
        _lampProgram.SetUniform("view", _camera.GetViewMatrix());
        _lampProgram.SetUniform("projection", _camera.GetProjectionMatrix());

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }

    private void OnInitialize(object? sender, EventArgs e)
    {
        var mainSettings = new GLWpfControlSettings();
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new(1920, 1080);

        GL.ClearColor(Color.RosyBrown);
        GL.Enable(EnableCap.DepthTest);

        var width = (float)OpenTkControl.RenderSize.Width;
        var height = (float)OpenTkControl.RenderSize.Height;
        _camera.AspectRatio = width / height;

        _lightingProgram = new();
        _lightingProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/lighting.frag");

        _lampProgram = new();
        _lampProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/lamp.frag");

        // var projectionMatrix = _camera.GetProjectionMatrix();
        // var viewMatrix = _camera.GetViewMatrix();
        // var modelMatrix = Matrix4.Identity;
        // var modelMatrix1 = Matrix4.CreateScale(0.2f);
        // modelMatrix1 *= Matrix4.CreateTranslation(_lightPos);
        //
        // _renderables = new IRenderable[]
        // {
        //     new Instance(_lightingProgram, Primitives.Cube, new IUniformContext[]
        //     {
        //         new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix, "model")),
        //         new Lighting((Color.DarkGoldenrod, "objectColor"), (new(1.0f), "lightColor"),
        //             (_lightPos, "lightPos"), "viewPos")
        //     }),
        //     new Instance(_lampProgram, Primitives.Cube, new IUniformContext[]
        //     {
        //         new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix1, "model"))
        //     })
        // };

        _vbo = new();
        _vbo.Bind();
        _vbo.BufferData(Primitives.Cube);

        _lVao = new(VertexAttribType.Float);

        _lVao.Bind();
        GL.EnableVertexAttribArray(0);
        // Remember to change the stride as we now have 6 floats per vertex
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

        // We now need to define the layout of the normal so the shader can use it
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float),
            3 * sizeof(float));

        _lampVao = new(VertexAttribType.Float);
        _lampVao.Bind();

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

        // _renderables[0].Vao = _lVao;
        // _renderables[1].Vao = _lampVao;
        //
        // _renderServer = new(_renderables);
    }
}