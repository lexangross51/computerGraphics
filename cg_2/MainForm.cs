namespace cg_2;

public enum ProjectionMode : byte
{
    Orthographic,
    Perspestive
}


public partial class MainForm : Form
{
    Camera _mainCamera;

    public MainForm()
    {
        InitializeComponent();
        _mainCamera = new Camera();
    }

    private void GL_OpenGLInitialized(object sender, EventArgs e)
    {
        var gl = GL.OpenGL;

        gl.Enable(OpenGL.GL_DEPTH_TEST);
    }

    private void GL_Resized(object sender, EventArgs e)
    {
        var gl = GL.OpenGL;

        gl.Viewport(0, 0, GL.Width, GL.Height);
        gl.MatrixMode(OpenGL.GL_PROJECTION);
        gl.LoadIdentity();
        gl.Frustum(0, GL.Width, 0, GL.Height, -100, 100);
        //gl.Perspective(70, GL.Width / GL.Height, -10, 100);
        gl.MatrixMode(OpenGL.GL_MODELVIEW);
        gl.LoadIdentity();
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

        for (float i = -1000; i < 1000; i += 0.1f)
        {
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(i, 0.0f, -1000.0f);
            gl.Vertex(i, 0.0f, 1000.0f);
            gl.Vertex(-1000.0f, 0.0f, i);
            gl.Vertex(1000.0f, 0.0f, i);
            gl.End();
        }

        gl.Finish();

        #endregion


    }

    private void GL_MouseMove(object sender, MouseEventArgs e)
    {
        var xPos = (float)e.X;
        var yPos = (float)e.Y;

        _mainCamera.LookAt(xPos, yPos);
    }

    private void GL_MouseClick(object sender, MouseEventArgs e)
    {

    }
}