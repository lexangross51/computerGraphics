using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ComputerGraphics.Source;
using SharpGL;

namespace ComputerGraphics
{
    public partial class MainForm : Form
    {
        private List<StripLine> _lines = new List<StripLine> ();
        private readonly StripLine _line = new StripLine();

        public MainForm()
        {
            InitializeComponent();
        }

        private void GL_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = GL.OpenGL;

            gl.Disable(OpenGL.GL_DEPTH_TEST);
            gl.ClearColor(1f, 1f, 1f, 1f);
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT);

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Ortho2D(0, GL.Width, 0, GL.Height);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
        }

        private void GL_OpenGLDraw(object sender, RenderEventArgs args)
        {
            OpenGL gl = GL.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
            gl.Color(0, 0, 0);



            for (int i = 0; i < _lines.Count; i++)
            {
                gl.Begin(OpenGL.GL_LINE_STRIP);

                for (int j = 0; j < _lines[i].Points.Count; j++)
                {
                    gl.Vertex(_lines[i].Points[j].X, _lines[i].Points[j].Y);
                }

                gl.End();
            }



            gl.Begin(OpenGL.GL_LINE_STRIP);
            foreach (var p in _line.Points)
            {
                gl.Vertex(p.X, p.Y);
            }
            gl.End();



            gl.Finish();

        }

        private void GL_Resized(object sender, EventArgs e)
        {
            GL_OpenGLInitialized(sender, e);
        }

        private void GL_MouseClick(object sender, MouseEventArgs e)
        {
            short mouseX = (short)e.X;
            short mouseY = (short)(GL.Height - (short)e.Y);

            _line.Points.Add(new Point2D(mouseX, mouseY));
        }

        private void GL_KeyDown(object sender, KeyEventArgs e)
        {
            _lines.Add(_line.Clone() as StripLine);
            _line.Points.Clear();
        }

        private void GL_MouseMove(object sender, MouseEventArgs e)
        {
            short xPos = (short)e.X;
            short yPos = (short)e.Y;

            statusXPosValue.Text = xPos.ToString();
            statusYPosValue.Text = yPos.ToString();
        }
    }
}
