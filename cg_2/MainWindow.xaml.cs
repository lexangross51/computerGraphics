namespace cg_2;

public partial class MainWindow
{
    private readonly Camera _mainCamera;
    private readonly VertexBuffer _vbo = new();
    private readonly VertexBufferArray _vao = new();
    private readonly ShaderProgram _shaderProgram = new();
    private vec3[] _cubePositions = default!;
    private readonly Stopwatch _stopWatch;
    private double _deltaTime, _lastFrame;

    public MainWindow()
    {
        InitializeComponent();
        _mainCamera = new Camera();
        _deltaTime = 0.0f;
        _lastFrame = 0.0f;
        _stopWatch = new Stopwatch();
        _stopWatch.Start();
    }

    private void OpenGLControl_OnOpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        var currentFrame = _stopWatch.ElapsedMilliseconds;
        _deltaTime = (currentFrame - _lastFrame) / 1000.0;
        _lastFrame = currentFrame;

        var gl = args.OpenGL;
        var width = gl.RenderContextProvider.Width;
        var height = gl.RenderContextProvider.Height;

        gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        _shaderProgram.Push(gl, null);

        var projection = glm.perspective(45.0f, width / (float)height, 0.1f, 100.0f);
        var view = glm.lookAt(_mainCamera.Position, _mainCamera.Position + _mainCamera.Front, _mainCamera.Up);

        var modelLoc = _shaderProgram.GetUniformLocation("model");
        var viewLoc = _shaderProgram.GetUniformLocation("view");
        var projectionLoc = _shaderProgram.GetUniformLocation("projection");

        gl.UniformMatrix4(viewLoc, 1, false, view.to_array());
        gl.UniformMatrix4(projectionLoc, 1, false, projection.to_array());

        _vao.Bind(gl);

        for (var i = 0; i < 10; i++)
        {
            var model = mat4.identity();
            model = glm.translate(model, _cubePositions[i]);

            var angle = 20.0f * i;
            model = glm.rotate(model, angle, new vec3(1.0f, 0.3f, 0.5f));
            gl.UniformMatrix4(modelLoc, 1, false, model.to_array());

            gl.DrawArrays(OpenGL.GL_TRIANGLES, 0, 36);
        }

        _shaderProgram.Pop(gl, null);
        _vao.Unbind(gl);
    }

    private void OpenGLControl_OnOpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        var gl = args.OpenGL;

        gl.Enable(OpenGL.GL_DEPTH_TEST);

        VertexShader vertexShader = new();
        vertexShader.CreateInContext(gl);
        vertexShader.LoadSource("Source/Shaders/shader.vert");

        FragmentShader fragmentShader = new();
        fragmentShader.CreateInContext(gl);
        fragmentShader.LoadSource("Source/Shaders/shader.frag");

        vertexShader.Compile();
        fragmentShader.Compile();

        _shaderProgram.CreateInContext(gl);
        _shaderProgram.AttachShader(vertexShader);
        _shaderProgram.AttachShader(fragmentShader);
        _shaderProgram.Link();

        fragmentShader.DestroyInContext(gl);
        vertexShader.DestroyInContext(gl);

        #region Вершины примитива

        float[] vertices =
        {
            -0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
            0.5f, -0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            0.5f,  0.5f, -0.5f, 0.0f, 0.0f, 1.0f,
            0.5f,  0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, 1.0f,

            -0.5f, -0.5f,  0.5f, 1.0f, 0.0f, 0.0f,
            0.5f, -0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
            0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f,
            0.5f,  0.5f,  0.5f, 1.0f, 0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f, 0.0f, 0.0f, 1.0f,

            -0.5f,  0.5f,  0.5f, 1.0f, 0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
            -0.5f, -0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f,

            0.5f,  0.5f,  0.5f, 1.0f, 0.0f, 0.0f,
            0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            0.5f, -0.5f, -0.5f, 0.0f, 0.0f, 1.0f,
            0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
            0.5f, -0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
            0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f,

            -0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
            0.5f, -0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            0.5f, -0.5f,  0.5f, 0.0f, 0.0f, 1.0f,
            0.5f, -0.5f,  0.5f, 1.0f, 0.0f, 0.0f,
            -0.5f, -0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
            0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f,
            0.5f,  0.5f,  0.5f, 1.0f, 0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, 0.0f, 0.0f, 1.0f,
        };

        _cubePositions = new vec3[]
        {
            new( 0.0f,  0.0f,  0.0f),
            new( 2.0f,  5.0f, -15.0f),
            new(-1.5f, -2.2f, -2.5f),
            new(-3.8f, -2.0f, -12.3f),
            new( 2.4f, -0.4f, -3.5f),
            new(-1.7f,  3.0f, -7.5f),
            new( 1.3f, -2.0f, -2.5f),
            new( 1.5f,  2.0f, -2.5f),
            new( 1.5f,  0.2f, -1.5f),
            new(-1.3f,  1.0f, -1.5f)
        };

        #endregion

        _vao.Create(gl);
        _vao.Bind(gl);

        _vbo.Create(gl);
        _vbo.Bind(gl);
        gl.BufferData(OpenGL.GL_ARRAY_BUFFER, vertices, OpenGL.GL_STATIC_DRAW);
        gl.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, 6 * sizeof(float), IntPtr.Zero);
        gl.EnableVertexAttribArray(0);
        gl.VertexAttribPointer(1, 3, OpenGL.GL_FLOAT, false, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
        gl.EnableVertexAttribArray(1);
        _vbo.Unbind(gl);

        _vao.Unbind(gl);
    }

    private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.W:
                _mainCamera.Move(TranslateDirection.Forward, _deltaTime);
                break;
            case Key.S:
                _mainCamera.Move(TranslateDirection.Back, _deltaTime);
                break;
            case Key.A:
                _mainCamera.Move(TranslateDirection.Left, _deltaTime);
                break;
            case Key.D:
                _mainCamera.Move(TranslateDirection.Right, _deltaTime);
                break;
        }
    }

    private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var pos = e.GetPosition(this);

            _mainCamera.LookAt((float)pos.X, (float)pos.Y);
        }
        else
        {
            _mainCamera.FirstMouse = true;
        }
    }
}
