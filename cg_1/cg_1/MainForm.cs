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

namespace cg_1
{
    public partial class MainForm : Form
    {
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

        }

        private void GL_Resized(object sender, EventArgs e)
        {
            GL_OpenGLInitialized(sender, e);
        }
    }
}
