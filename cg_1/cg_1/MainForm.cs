using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ComputerGraphics.Source;
using SharpGL;

namespace ComputerGraphics
{
    public partial class MainForm : Form
    {
        private readonly List<List<StripLine>> _lines = new List<List<StripLine>>();
        private readonly List<Point2D> _shifts = new List<Point2D>();
        private readonly StripLine _line = new StripLine();
        private byte _currentSet = 0;
        private byte _currentLine = 0;
        private bool _isDrawingCurrent = false;
        
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

            for (int iset = 0; iset < _lines.Count; iset++)
            {
                for (int iline = 0; iline < _lines[iset].Count; iline++)
                {
                    var line = _lines[iset][iline];

                    gl.PushMatrix();
                    gl.Translate(_shifts[iset].X, _shifts[iset].Y, 0);
                    gl.Color(line.Color.R, line.Color.G, line.Color.B);
                    gl.LineWidth(line.Thickness);
                    gl.Begin(OpenGL.GL_LINE_STRIP);

                    foreach (var p in line.Points)
                    {
                        gl.Vertex(p.X, p.Y);
                    }

                    gl.End();

                    // Текущий набор выделяем точками
                    if (iset == _currentSet)
                    {
                        gl.PointSize(10);

                        // Выделяем "активную" линию
                        if (iline == _currentLine)
                        {
                            gl.Color(1.0f, 0.0f, 0.0f);
                        }
                        else
                        {
                            gl.Color(0.0f, 0.0f, 0.0f);
                            gl.Enable(OpenGL.GL_POINT_SMOOTH);
                        }

                        gl.Begin(OpenGL.GL_POINTS);

                        foreach (var p in line.Points)
                        {
                            gl.Vertex(p.X, p.Y);
                        }

                        gl.End();

                        if (iline != _currentLine)
                        {
                            gl.Disable(OpenGL.GL_POINT_SMOOTH);
                        }
                    }

                    gl.PopMatrix();
                }
            }

            if (_isDrawingCurrent)
            {
                gl.Color(_line.Color.R, _line.Color.G, _line.Color.B);
                gl.LineWidth(_line.Thickness);
                gl.Begin(OpenGL.GL_LINE_STRIP);

                foreach (var p in _line.Points)
                {
                    gl.Vertex(p.X, p.Y);
                }

                gl.End();

                // Сразу выделяем линию точками
                gl.PointSize(10);
                gl.Color(1.0f, 0.0f, 0.0f);
                gl.Begin(OpenGL.GL_POINTS);

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
                if (_lines.IsEmpty())
                {
                    AddSet_Click(sender, e);
                }

                _isDrawingCurrent = true;

                short mouseX = (short)e.X;
                short mouseY = (short)(GL.Height - (short)e.Y);

                _line.Points.Add(new Point2D(mouseX, mouseY));

                if (_line.Points.Count == 1)
                {
                    if (_lines[_currentSet].IsEmpty())
                    {
                        _currentLine = 0;
                    }
                    else
                    {
                        ChangePrimitive.Maximum = _lines[_currentSet].Count;
                        ChangePrimitive.Value = ChangePrimitive.Maximum;
                        _currentLine = (byte)ChangePrimitive.Maximum;
                    }

                    ChangePrimitive.Enabled = true;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if (_line.Points.Count == 0) return;

                _lines[_currentSet].Add(_line.Clone() as StripLine);
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
        // Управление сценой **********************************************
        private void UpBtn_Click(object sender, EventArgs e)
        {
            if (!_isDrawingCurrent && !_lines[_currentSet].IsEmpty()) 
            {
                _shifts[_currentSet] = new Point2D(_shifts[_currentSet].X, (short)(_shifts[_currentSet].Y + 40));
                statusXShiftValue.Text = _shifts[_currentSet].X.ToString();
                statusYShiftValue.Text = _shifts[_currentSet].Y.ToString();
            }
        }
        private void RightBtn_Click(object sender, EventArgs e)
        {
            if (!_isDrawingCurrent && !_lines[_currentSet].IsEmpty())
            {
                _shifts[_currentSet] = new Point2D((short)(_shifts[_currentSet].X + 40), _shifts[_currentSet].Y);
                statusXShiftValue.Text = _shifts[_currentSet].X.ToString();
                statusYShiftValue.Text = _shifts[_currentSet].Y.ToString();
            }
        }
        private void LeftBtn_Click(object sender, EventArgs e)
        {
            if (!_isDrawingCurrent && !_lines[_currentSet].IsEmpty())
            {
                _shifts[_currentSet] = new Point2D((short)(_shifts[_currentSet].X - 40), _shifts[_currentSet].Y);
                statusXShiftValue.Text = _shifts[_currentSet].X.ToString();
                statusYShiftValue.Text = _shifts[_currentSet].Y.ToString();
            }
        }
        private void DownBtn_Click(object sender, EventArgs e)
        {
            if (!_isDrawingCurrent && !_lines[_currentSet].IsEmpty())
            {
                _shifts[_currentSet] = new Point2D(_shifts[_currentSet].X, (short)(_shifts[_currentSet].Y - 40));
                statusXShiftValue.Text = _shifts[_currentSet].X.ToString();
                statusYShiftValue.Text = _shifts[_currentSet].Y.ToString();
            }
        }
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            _shifts[_currentSet] = new Point2D();
            statusXShiftValue.Text = _shifts[_currentSet].X.ToString();
            statusYShiftValue.Text = _shifts[_currentSet].Y.ToString();
        }


        // Управление наборами ********************************************
        private void ChangeSet_ValueChanged(object sender, EventArgs e)
        {
            _currentSet = (byte)ChangeSet.Value;

            if (!_lines.IsEmpty())
            {
                if (_lines[_currentSet].IsEmpty())
                {
                    ChangePrimitive.Maximum = 0;
                }
                else
                {
                    ChangePrimitive.Maximum = _lines[_currentSet].Count - 1;
                }
            }
        }
        private void AddSet_Click(object sender, EventArgs e)
        {
            // Если кнопка "Создать новый набор" нажата до завершения рисования
            // примитива, то принудительно завершаем его рисование
            if (_isDrawingCurrent)
            {
                _lines[_currentSet].Add(_line.Clone() as StripLine);
                _line.Reset();
                _isDrawingCurrent = false;
            }

            // Если нет еще ни одного набора -> создаем его
            if (_lines.IsEmpty())
            {
                ChangeSet.Enabled = true;
                _lines.Add(new List<StripLine>());
                _shifts.Add(new Point2D());
                return;
            }

            // Создать новый набор можно только в том случае, если предшествующий
            // ему набор не пуст
            if (!_lines[_currentSet].IsEmpty())
            {
                _line.Reset();

                _lines.Add(new List<StripLine>());
                _shifts.Add(new Point2D());

                ChangeSet.Maximum = _lines.Count - 1;
                ChangeSet.Value = ChangeSet.Maximum;

                ChangeWidthS.Value = 1;
                ChangePrimitive.Value = 0;
                ChangePrimitive.Maximum = 0;
            }
        }
        private void DeleteSet_Click(object sender, EventArgs e)
        {
            // Удалять можно только если не рисуется примитив
            // либо если есть хотя бы один набор
            if (!_isDrawingCurrent && !_lines.IsEmpty())
            {
                _currentSet = (byte)ChangeSet.Value;

                _lines.RemoveAt(_currentSet);
                _shifts.RemoveAt(_currentSet);

                ChangeSet_ValueChanged(sender, e);

                ChangeSet.Maximum = _lines.Count == 0 ? 0 : _lines.Count - 1;
                ChangeSet.Value = ChangeSet.Maximum;
            }
            
            // Не отображаем "Текущий набор", если их нет
            if (_lines.IsEmpty())
            {
                ChangeSet.Enabled = false;
                ChangePrimitive.Enabled = false;
            }
        }
        private void ChangeWidthS_ValueChanged(object sender, EventArgs e)
        {
            _line.Thickness = (float)ChangeWidthS.Value;

            if (!_lines.IsEmpty())
            {
                foreach (var line in _lines[_currentSet])
                {
                    line.Thickness = _line.Thickness;
                }
            }
        }
        private void ChangeColorS_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();

            _line.Color = colorDialog1.Color;

            if (!_lines.IsEmpty())
            {
                foreach (var line in _lines[_currentSet])
                {
                    line.Color = _line.Color;
                }
            }
        }


        // Управление примитивами *****************************************
        private void ChangeColorP_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();

            _line.Color = colorDialog1.Color;

            if (!_lines.IsEmpty() && !_lines[_currentSet].IsEmpty())
            {
                _lines[_currentSet][_currentLine].Color = colorDialog1.Color;
            }
        }
        private void DeletePrimitive_Click(object sender, EventArgs e)
        {
            if (!_lines.IsEmpty())
            {
                _lines[_currentSet].RemoveAt(_currentLine);
                ChangePrimitive.Value = ChangePrimitive.Value == 0 ? 0 : --ChangePrimitive.Value;
                ChangePrimitive.Maximum = ChangePrimitive.Maximum == 0 ? 0 : --ChangePrimitive.Maximum;
            }
            else return;

            if (_lines[_currentSet].IsEmpty())
            {
                DeleteSet_Click(sender, e);
            }
        }
        private void ChangePrimitive_ValueChanged(object sender, EventArgs e)
        {
            _currentLine = (byte)ChangePrimitive.Value;
        }
        private void ChangeWidthP_ValueChanged(object sender, EventArgs e)
        {
            _line.Thickness = (float)ChangeWidthP.Value;

            if (!_lines.IsEmpty() && !_lines[_currentSet].IsEmpty())
            {
                _lines[_currentSet][_currentLine].Thickness = (float)ChangeWidthP.Value;
            }
        }
    }
}