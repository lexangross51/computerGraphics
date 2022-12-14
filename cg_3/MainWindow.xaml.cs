using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using cg_3.Source;
using SharpGL;
using SharpGL.WPF;
using Xceed.Wpf.Toolkit;

namespace cg_3;

public partial class MainWindow
{
    private Projection _ortho = new(-20, 20, -20, 20);
    private readonly List<BezierObject> _bezierCurves = new ();
    private readonly List<List<Vector2>> _bezierPoints = new();
    private readonly List<Color> _bezierColors = new();
    private int _currentCurve = -1;
    private int _step;
    private bool _isDrawingMode = true, _canNavigate, _isEditable, _isAbleToMovePoint;
    private Vector2 _fulcrum;
    private OpenGL _glContext;
    private float _xGridSplits, _yGridSplits;
    private readonly List<int> _segmentsCount = new();
    private readonly List<(int, int)> _pairCurvePoint = new();

    public MainWindow()
    {
        _glContext = new OpenGL();

        InitializeComponent();
    }

    #region OpenGL

    private void OnOpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        _glContext = args.OpenGL;

        var width = _glContext.RenderContextProvider.Width;
        var height = _glContext.RenderContextProvider.Height;

        _glContext.Disable(OpenGL.GL_DEPTH_TEST);
        _glContext.ClearColor(1f, 1f, 1f, 1f);
        _glContext.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
        _glContext.MatrixMode(OpenGL.GL_PROJECTION);
        _glContext.LoadIdentity();
        _glContext.Ortho2D(_ortho.Left, _ortho.Right, _ortho.Bottom, _ortho.Top);
        _glContext.MatrixMode(OpenGL.GL_MODELVIEW);
        _glContext.LoadIdentity();
    }

    private void OnOpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        #region Change interface

        CurrentCurve.Maximum = _bezierCurves.Count - 1;
        CurrentCurve.Text = _currentCurve.ToString();

        #endregion
        
        _glContext.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
        _glContext.Enable(OpenGL.GL_POINT_SMOOTH);
        _glContext.MatrixMode(OpenGL.GL_PROJECTION);
        _glContext.LoadIdentity();
        _glContext.Ortho2D(_ortho.Left, _ortho.Right, _ortho.Bottom, _ortho.Top);
        _glContext.MatrixMode(OpenGL.GL_MODELVIEW);
        _glContext.LoadIdentity();
        
        DrawGrid(_ortho.Width / _xGridSplits, _ortho.Height / _yGridSplits);
        DrawAxes(_ortho.Width / _xGridSplits, _ortho.Height / _yGridSplits);

        // Control points
        for (int i = 0; i < _bezierCurves.Count; i++)
        {
            var color = _bezierColors[i];
            
            _glContext.PointSize(5);
            _glContext.Color(color.R, color.G, color.B);
            _glContext.Begin(OpenGL.GL_POINTS);
            
            if (_step != 4 && i == _currentCurve)
            {
                foreach (var point in _bezierCurves[i].ControlPoints)
                {
                    _glContext.Vertex(point.X, point.Y);
                }
            }
            else
            {
                var first = _bezierCurves[i].ControlPoints[0];
                var last = _bezierCurves[i].ControlPoints[^1];
                
                _glContext.Vertex(first.X, first.Y);
                _glContext.Vertex(last.X, last.Y);
            }
            
            _glContext.End();
        }

        // Curves
        for (int i = 0; i < _bezierPoints.Count; i++)
        {
            var color = _bezierColors[i];
            _glContext.Color(color.R, color.G, color.B);
            _glContext.LineWidth(i == _currentCurve ? 5 : 1);
            
            _glContext.Begin(OpenGL.GL_LINE_STRIP);

            foreach (var point in _bezierPoints[i])
            {
                _glContext.Vertex(point.X, point.Y);
            }
                
            _glContext.End();
            _glContext.LineWidth(1);
        }
        
        _glContext.Finish();
    }

    #endregion

    #region Interface

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        var cursorPosition = e.GetPosition(this);
        float xPos = (float)cursorPosition.X - 11;
        float yPos = (float)cursorPosition.Y - 11;
        Vector2 screenPoint = _ortho.ToProjectionCoordinate(xPos, yPos, _glContext.RenderContextProvider);

        xPosition.Text = "X: " + (screenPoint.X) + ": ";
        yPosition.Text = "Y: " + (screenPoint.Y) + ": ";

        if (_isEditable && _isAbleToMovePoint)
        {
            for (int i = 0; i < _pairCurvePoint.Count; i++)
            {
                var icurve = _pairCurvePoint[i].Item1;
                var ipoint = _pairCurvePoint[i].Item2; 
                var bezierCurve = _bezierCurves[icurve];
                
                bezierCurve.UpdateControlPoint(ipoint, screenPoint);
            }
            
            for (int i = 0; i < _pairCurvePoint.Count; i++)
            {
                var icurve = _pairCurvePoint[i].Item1;
                var bezierCurve = _bezierCurves[icurve];
                
                _bezierPoints[icurve].Clear();
                
                for (int j = 0; j <= _segmentsCount[icurve]; j++)
                {
                    var t = (float)j / _segmentsCount[icurve];
                
                    _bezierPoints[icurve].Add(bezierCurve.CurveGen(t));
                }
            }
            
            return;
        }

        if (!_bezierCurves.IsEmpty() && _isDrawingMode)
        {
            BezierObject bezierCurve = _bezierCurves[_currentCurve];
            
            if (bezierCurve.ControlPoints.Length != 0)
            {
                bezierCurve.UpdateControlPoint(_step, new Vector2(screenPoint.X, screenPoint.Y));
                
                _bezierPoints[_currentCurve].Clear();
                
                for (int i = 0; i <= _segmentsCount[_currentCurve]; i++)
                {
                    var t = (float)i / _segmentsCount[_currentCurve];
                
                    _bezierPoints[_currentCurve].Add(bezierCurve.CurveGen(t));
                }
            }   
        }

        if (_canNavigate)
        {
            var xShift = screenPoint.X - _fulcrum.X;
            var yShift = screenPoint.Y - _fulcrum.Y;

            _ortho.Left -= xShift;
            _ortho.Right -= xShift;
            _ortho.Bottom -= yShift;
            _ortho.Top -= yShift;
        }
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var cursorPosition = e.GetPosition(this);
        float xPos = (float)cursorPosition.X - 11;
        float yPos = (float)cursorPosition.Y - 11;
        Vector2 screenPoint = _ortho.ToProjectionCoordinate(xPos, yPos, _glContext.RenderContextProvider);

        if (_isEditable)
        {
            _isAbleToMovePoint = true;
            _pairCurvePoint.Clear();
            
            for (int i = 0; i < _bezierCurves.Count; i++)
            {
                if (_bezierCurves[i].IsSmoothed)
                {
                    _isAbleToMovePoint = false;
                    continue;
                }
                
                var points = _bezierCurves[i].ControlPoints;
            
                for (int j = 0; j < points.Length; j++)
                {
                    var point = points[j];
                    
                    if (Math.Abs(screenPoint.X - point.X) < 1 && Math.Abs(screenPoint.Y - point.Y) < 1)
                    {
                        _isAbleToMovePoint = true;
                        _pairCurvePoint.Add((i, j));

                        if (_pairCurvePoint.Count == 2) return;
                    }
                }
            }

            return;
        }
        
        _isDrawingMode = true;

        if (_step == 0)
        {
            _bezierCurves.Add(new BezierObject());
            _bezierPoints.Add(new List<Vector2>());
            _bezierColors.Add(Color.FromRgb(0, 0, 0));
            _segmentsCount.Add(20);
            _currentCurve = (int)CurrentCurve.Maximum! + 1;
            
            _bezierCurves[_currentCurve].AddControlPoint(new Vector2(screenPoint.X, screenPoint.Y));
            _bezierCurves[_currentCurve].AddControlPoint(new Vector2(screenPoint.X, screenPoint.Y));
            _step++;
        }
        else
        {
            _bezierCurves[_currentCurve].AddControlPoint(new Vector2(screenPoint.X, screenPoint.Y));
            _step++;   
        }

        if (_step == 4)
        {
            _bezierCurves[_currentCurve].DeleteControlPoint(_step);
            _bezierCurves.Add(new BezierObject());
            _bezierPoints.Add(new List<Vector2>());
            _bezierColors.Add(Color.FromRgb(0, 0, 0));
            _currentCurve++;
            _step = 0;

            var currentBezierCurve = _bezierCurves[_currentCurve];
            var previousBezierCurve = _bezierCurves[_currentCurve - 1];

            currentBezierCurve.AddControlPoint(previousBezierCurve.ControlPoints[^1]);
            currentBezierCurve.AddControlPoint(previousBezierCurve.ControlPoints[^1]);
            _segmentsCount.Add(20);
            _step = 1;
        }
    }

    private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        _canNavigate = true;
        var cursorPosition = e.GetPosition(this);
        float xPos = (float)cursorPosition.X;
        float yPos = (float)cursorPosition.Y;
        _fulcrum = _ortho.ToProjectionCoordinate(xPos, yPos, _glContext.RenderContextProvider);
    }
    
    private void OnMiddleMouseButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Middle)
        {
            if (_step > 1 && _isDrawingMode)
            {
                _bezierCurves[_currentCurve].DeleteControlPoint(_step--);
            }
        }
    }

    private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        => _canNavigate = false;

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var cursorPosition = e.GetPosition(this);
        float xPos = (float)cursorPosition.X;
        float yPos = (float)cursorPosition.Y;
        Vector2 screenPoint = _ortho.ToProjectionCoordinate(xPos, yPos, _glContext.RenderContextProvider);

        if (e.Delta > 0)
        {
            Scale(screenPoint, 1.1f);
        }
        else
        {
            Scale(screenPoint, 1.0f / 1.1f);
        }
    }
    
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            if (_step != 4 && _isDrawingMode)
            {
                _isDrawingMode = false;
                _step = 0;
                _bezierCurves.RemoveAt(_currentCurve);
                _bezierPoints.RemoveAt(_currentCurve);
                _segmentsCount.RemoveAt(_currentCurve);
                _currentCurve--;
            }
        }
    }

    private void XGridSplitSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        => _xGridSplits = (float)(sender as Slider)!.Value;

    private void YGridSplitSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        => _yGridSplits = (float)(sender as Slider)!.Value;
    
    private void CurrentCurve_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        => _currentCurve = (int)(sender as IntegerUpDown)!.Value!;

    private void ColorPicker_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        => _bezierColors[_currentCurve] = (Color)(sender as ColorPicker)!.SelectedColor!;

    private void EditMode_OnChecked(object sender, RoutedEventArgs e)
        => _isEditable = (bool)(sender as CheckBox)!.IsChecked!;
    
    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        => _isAbleToMovePoint = false;

    private void SegmentsCount_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (_bezierCurves.IsEmpty()) return;
        
        _segmentsCount[_currentCurve] = (int)(sender as IntegerUpDown)!.Value!;

        var curve = _bezierCurves[_currentCurve];

        _bezierPoints[_currentCurve].Clear();

        for (int j = 0; j <= _segmentsCount[_currentCurve]; j++)
        {
            var t = (float)j / _segmentsCount[_currentCurve];

            _bezierPoints[_currentCurve].Add(curve.CurveGen(t));
        }      
    }

    private void SmoothConnectBtn_OnClick(object sender, RoutedEventArgs e)
    {
        List<Vector2> allControlPoints = new();

        for (int i = 0; i < _bezierCurves.Count; i++)
        {
            if (_bezierCurves[i].IsSmoothed) continue;
            
            var points = _bezierCurves[i].ControlPoints;

            for (int j = 0; j < points.Length - 1; j++)
            {
                allControlPoints.Add(points[j]);
            }

            if (i == _bezierCurves.Count - 1)
            {
                allControlPoints.Add(points[^1]);
            }
        }

        for (int i = 0; i < allControlPoints.Count; i += 3)
        {
            if (i == allControlPoints.Count - 1)
            {
                break;
            }

            Vector2 point3;
            
            if (i + 2 >= allControlPoints.Count)
            {
                allControlPoints.Add(allControlPoints[^1]);
                point3 = allControlPoints[^1];
            }
            else
            {
                point3 = allControlPoints[i + 2];
            }
            
            var point4 = i + 3 >= allControlPoints.Count ? allControlPoints[^1] : allControlPoints[i + 3];
            
            var newPoint = new Vector2((point3.X + point4.X) / 2.0f, (point3.Y + point4.Y) / 2.0f);
            allControlPoints.Insert(i + 3, newPoint);
        }
        
        _bezierCurves.Clear();
        _bezierColors.Clear();
        _segmentsCount.Clear();
        _bezierPoints.Clear();

        for (int i = 0; i < allControlPoints.Count - 3; i += 3)
        {
            var p0 = allControlPoints[i];
            var p1 = allControlPoints[i + 1];
            var p2 = allControlPoints[i + 2];
            var p3 = allControlPoints[i + 3];
        
            _bezierCurves.Add(new BezierObject(p0, p1, p2, p3, true));
            _bezierPoints.Add(new List<Vector2>());
            _bezierColors.Add(Color.FromRgb(0, 0, 0));
            _segmentsCount.Add(20);
        }

        for (int icurve = 0; icurve < _bezierCurves.Count; icurve++)
        {
            var bezierCurve = _bezierCurves[icurve];

            for (int i = 0; i <= _segmentsCount[icurve]; i++)
            {
                var t = (float)i / _segmentsCount[icurve];

                _bezierPoints[icurve].Add(bezierCurve.CurveGen(t));
            }
        }
    }
    
    #endregion

    #region Additional functions

    private void DrawGrid(float xGridStep, float yGridStep)
    {
        _glContext.Enable(OpenGL.GL_LINE_STIPPLE);
        _glContext.LineStipple(1, 0x0101);
        _glContext.Color(0.41f, 0.41f, 0.41f);
        _glContext.Begin(OpenGL.GL_LINES);
        
        for (float axis = _ortho.Left ; axis < _ortho.Right; axis += xGridStep)
        {
            _glContext.Vertex(axis, _ortho.Bottom);
            _glContext.Vertex(axis, _ortho.Top);
        }
        
        for (float axis = _ortho.Bottom; axis < _ortho.Top; axis += yGridStep)
        {
            _glContext.Vertex(_ortho.Left, axis);
            _glContext.Vertex(_ortho.Right, axis);
        }

        _glContext.End();
        _glContext.Disable(OpenGL.GL_LINE_STIPPLE);
    }

    private void DrawAxes(float xGridStep, float yGridStep)
    {
        var xLineLength = _ortho.Width * 0.005f;  // 0.5% of the width
        var yLineLength = _ortho.Height * 0.008f; // 0.8% of the height

        _glContext.Color(0, 0, 0);
        _glContext.Begin(OpenGL.GL_LINES);

        for (float xAxis = _ortho.Left; xAxis < _ortho.Right; xAxis += xGridStep)
        {
            _glContext.Vertex(xAxis, _ortho.Bottom);
            _glContext.Vertex(xAxis, _ortho.Bottom + yLineLength);
        }
        
        for (float yAxis = _ortho.Bottom; yAxis <  _ortho.Top; yAxis += yGridStep)
        {
            _glContext.Vertex(_ortho.Left, yAxis);
            _glContext.Vertex(_ortho.Left + xLineLength, yAxis);
        }
        
        _glContext.End();
                
        // Axes legends
        for (float axis = _ortho.Left + xGridStep; axis < _ortho.Right; axis += xGridStep) 
        {
            var scr = _ortho.ToScreenCoordinates(axis, _ortho.Bottom, _glContext.RenderContextProvider);
            string axisText = axis.ToString("0.0");

            _glContext.DrawText((int)(scr.X - 19), (int)scr.Y + 10, 0f, 0f, 0f, "Arial", 12, axisText);
        }

        var screen = _ortho.ToScreenCoordinates(_ortho.Right, _ortho.Bottom, _glContext.RenderContextProvider);
        _glContext.DrawText((int)(screen.X - 19), (int)screen.Y + 10, 0f, 0f, 0f, "Arial", 14, "X");

        for (float axis = _ortho.Bottom + yGridStep; axis < _ortho.Top; axis += yGridStep)
        {
            var scr = _ortho.ToScreenCoordinates(_ortho.Left, axis, _glContext.RenderContextProvider);
            string axisText = axis.ToString("0.0");

            _glContext.DrawText((int)scr.X + 10, (int)scr.Y, 0f, 0f, 0f, "Arial", 12, axisText);
        }

        screen = _ortho.ToScreenCoordinates(_ortho.Left, _ortho.Top, _glContext.RenderContextProvider);
        _glContext.DrawText((int)screen.X + 10, (int)screen.Y - 14, 0f, 0f, 0f, "Arial", 14, "Y");
    }

    private void Scale(Vector2 pivot, float scale)
    {
        float xStep = Math.Abs(pivot.X * (scale - 1.0f));
        float yStep = Math.Abs(pivot.Y * (scale - 1.0f));

        // Scale axes
        _ortho.Left = _ortho.Left * 1.0f / scale + Math.Sign(_ortho.Left) * xStep; 
        _ortho.Right = _ortho.Right * 1.0f / scale + Math.Sign(_ortho.Right) * xStep; 
        _ortho.Bottom = _ortho.Bottom * 1.0f / scale + Math.Sign(_ortho.Bottom) * yStep; 
        _ortho.Top = _ortho.Top * 1.0f / scale + Math.Sign(_ortho.Top) * yStep;
    }

    #endregion
}