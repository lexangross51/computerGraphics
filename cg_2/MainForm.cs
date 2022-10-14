namespace cg_2;

public enum ProjectionMode : byte
{
    Orthographic,
    Perspestive
}


public partial class MainForm : Form
{
    Camera _mainCamera;
    private bool _isRotatableCamera;

    public MainForm()
    {
        InitializeComponent();
        _mainCamera = new Camera();
        _isRotatableCamera = false;
    }

    private void GL_OpenGLInitialized(object sender, EventArgs e)
    {
        var gl = GL.OpenGL;

        gl.Enable(OpenGL.GL_DEPTH_TEST);
    }

    private void GL_Resized(object sender, EventArgs e)
    {
        var gl = GL.OpenGL;

        gl.MatrixMode(OpenGL.GL_PROJECTION);
        gl.LoadIdentity();
        gl.Frustum(0, GL.Width, 0, GL.Height, -10000, 1000);
        //gl.Perspective(70, GL.Width / GL.Height, -10, 100);
        gl.MatrixMode(OpenGL.GL_MODELVIEW);
    }

    private void GL_OpenGLDraw(object sender, RenderEventArgs args)
    {
        var gl = GL.OpenGL;

        gl.ClearColor(1, 1, 1, 1);
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        gl.LoadIdentity();

        var eye = _mainCamera.Position;
        var view = _mainCamera.View;
        var up = _mainCamera.Up;

        gl.LookAt(eye.X, eye.Y, eye.Z, view.X, view.Y, view.Z, up.X, up.Y, up.Z);

        #region Отрисовка сетки

        gl.Color((byte)0, (byte)0, (byte)0);

        for (float i = -100; i < 100; i += 0.1f)
        {
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(i, 0.0f, -100.0f);
            gl.Vertex(i, 0.0f, 100.0f);
            gl.Vertex(-100.0f, 0.0f, i);
            gl.Vertex(100.0f, 0.0f, i);
            gl.End();
        }

        gl.Begin(OpenGL.GL_QUADS);
        gl.Vertex(-0.1, 0.0f, 0.1);
        gl.Vertex(0.1, 0.0f, 0.1);
        gl.Vertex(0.1f, 0.0f, -0.1);
        gl.Vertex(-0.1, 0.0f, -0.1);

        gl.Vertex(-0.1, 0.2, 0.1);
        gl.Vertex(0.1, 0.2, 0.1);
        gl.Vertex(0.1f, 0.2, -0.1);
        gl.Vertex(-0.1, 0.2, -0.1);

        gl.Vertex(0.1, 0, -0.1);
        gl.Vertex(0.1, 0, 0.1);
        gl.Vertex(0.1f, 0.2, 0.1);
        gl.Vertex(0.1, 0.2, -0.1);

        gl.Vertex(-0.1, 0, -0.1);
        gl.Vertex(-0.1, 0, 0.1);
        gl.Vertex(-0.1f, 0.2, 0.1);
        gl.Vertex(-0.1, 0.2, -0.1);

        gl.Vertex(-0.1, 0, 0.1);
        gl.Vertex(0.1, 0, 0.1);
        gl.Vertex(0.1f, 0.2, 0.1);
        gl.Vertex(-0.1, 0.2, 0.1);

        gl.Vertex(-0.1, 0, 0.1);
        gl.Vertex(0.1, 0, 0.1);
        gl.Vertex(0.1f, 0.2, 0.1);
        gl.Vertex(-0.1, 0.2, 0.1);

        gl.End();

        gl.Begin(OpenGL.GL_LINES);
        gl.Color((byte)255, (byte)0, (byte)0);
        gl.Vertex(-5, 0, 0);
        gl.Vertex(5, 0, 0);

        gl.Color((byte)0, (byte)255, (byte)0);
        gl.Vertex(0, -5, 0);
        gl.Vertex(0, 5, 0);

        gl.Color((byte)0, (byte)0, (byte)255);
        gl.Vertex(0, 0, -5);
        gl.Vertex(0, 0, 5);
        gl.End();

        gl.Finish();

        #endregion


    }

    #region События клавиатуры
    private void GL_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == 'w')
        {
            _mainCamera.Translate(TranslateDirection.Forward);
        }
        if (e.KeyChar == 's')
        {
            _mainCamera.Translate(TranslateDirection.Back);
        }
        if (e.KeyChar == 'a')
        {
            _mainCamera.Translate(TranslateDirection.Left);
        }
        if (e.KeyChar == 'd')
        {
            _mainCamera.Translate(TranslateDirection.Right);
        }
    }

    #endregion

    #region События мыши
    private void GL_MouseMove(object sender, MouseEventArgs e)
    {
        var xPos = (float)e.X;
        var yPos = (float)e.Y;

        if (_isRotatableCamera)
        {
            _mainCamera.LookAt(xPos, yPos);
        }
    }

    private void GL_MouseClick(object sender, MouseEventArgs e)
    {

    }

    private void GL_MouseEnter(object sender, EventArgs e) { }

    private void GL_MouseLeave(object sender, EventArgs e) { }

    private void GL_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _isRotatableCamera = true;
        }
    }

    private void GL_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _isRotatableCamera = false;
        }
    }

    #endregion
}