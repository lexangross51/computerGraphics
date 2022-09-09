using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ComputerGraphics.Source;
using SharpGL;

namespace ComputerGraphics
{
    public partial class MainForm : Form
    {
        private readonly List<StripLine> _lines = new List<StripLine>();
        private readonly StripLine _line = new StripLine();
        private byte _currentSet = 0;
        private bool _isDrawingCurrent = true;


        public MainForm() => InitializeComponent();

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

        private void GL_Resized(object sender, EventArgs e)
        {
            GL_OpenGLInitialized(sender, e);
        }

        private void GL_OpenGLDraw(object sender, RenderEventArgs args)
        {
            OpenGL gl = GL.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT);


            foreach (var line in _lines)
            {
                gl.Color(_lines[i].Color.R, _lines[i].Color.G, _lines[i].Color.B);

                if (line.Set == _currentSet)
                {
                    gl.PointSize(10);
                    gl.Enable(OpenGL.GL_POINT_SMOOTH);
                    gl.Begin(OpenGL.GL_POINTS);

                    foreach (var p in line.Points)
                    {
                        gl.Vertex(p.X, p.Y);
                    }

                    gl.End();
                }

                gl.LineWidth(line.Thickness);
                gl.Begin(OpenGL.GL_LINE_STRIP);

                foreach (var p in line.Points)
                {
                    gl.Vertex(p.X, p.Y);
                }

                gl.End();
                gl.PopMatrix();
            }

            if (_isDrawingCurrent)
            {
                gl.PointSize(10);
                gl.Color(_line.Color.R, _line.Color.G, _line.Color.B);
                gl.Enable(OpenGL.GL_POINT_SMOOTH);
                gl.Begin(OpenGL.GL_POINTS);

                foreach (var p in _line.Points)
                {
                    gl.Vertex(p.X, p.Y);
                }

                gl.End();
                gl.Disable(OpenGL.GL_POINT_SMOOTH);

                gl.Begin(OpenGL.GL_LINE_STRIP);

                foreach (var p in _line.Points)
                {
                    gl.Vertex(p.X, p.Y);
                }

                gl.End();
            }

            gl.Finish();
        }

        private void GL_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDrawingCurrent = true;

                short mouseX = (short)e.X;
                short mouseY = (short)(GL.Height - (short)e.Y);

                _line.Points.Add(new Point2D(mouseX, mouseY));
            }

            if (e.Button == MouseButtons.Right)
            {
                _lines.Add(_line.Clone() as StripLine);
                _line.Points.Clear();
                _isDrawingCurrent = false;
            }
        }

        private void GL_MouseMove(object sender, MouseEventArgs e)
        {
            short xPos = (short)e.X;
            short yPos = (short)e.Y;

            statusXPosValue.Text = xPos.ToString();
            statusYPosValue.Text = yPos.ToString();
        }


        // Панель управления **********************************************
        // Управление сценой
        private void UpBtn_Click(object sender, EventArgs e)
        {
            _shiftY += 40;
        }
        private void RightBtn_Click(object sender, EventArgs e)
        {
            _shiftX += 40;
        }
        private void LeftBtn_Click(object sender, EventArgs e)
        {
            _shiftX -= 40;
        }
        private void DownBtn_Click(object sender, EventArgs e)
        {
            _shiftY -= 40;
        }
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            _shiftX = 0;
            _shiftY = 0;
        }

        private void ChangePrimitive_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DeleteSet_Click(object sender, EventArgs e)
        {
            _currentSet = (byte)ChangeSet.Value;

            for (int i = 0; i < _lines.Count; i++)
            {
                if (_lines[i].Set == _currentSet)
                {
                    _lines.RemoveAt(i);
                    i = i == 0 ? 0 : --i;
                }

                if (_lines[i].Set > _currentSet)
                {
                    _lines[i].Set--;
                }
            }

            ChangeSet.Maximum--;
        }

        private void AddSet_Click(object sender, EventArgs e)
        {
            ChangeSet.Value = ++ChangeSet.Maximum;
            _currentSet = (byte)ChangeSet.Value;
            _line.Set = _currentSet;
        }








        // Управление примитивами
        private void ChangePrimitive_ValueChanged(object sender, EventArgs e)
        {

        }


        

    }
}