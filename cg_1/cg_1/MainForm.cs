using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using cg_1.src;

namespace cg_1
{
    public partial class MainForm : Form
    {
        private List<StripLine> _lines = new List<StripLine> ();
        private StripLine _line = new StripLine();



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

        private void GL_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            OpenGL gl = GL.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
            gl.Color(0, 0, 0);


            gl.Begin(OpenGL.GL_LINE_STRIP);
            for (int i = 0; i < _line.Points.Count; i++)
            {
                gl.Vertex(_line.Points[i].X, _line.Points[i].Y);
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
    }
}
