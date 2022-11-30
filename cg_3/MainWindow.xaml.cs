using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using cg_3.Source;
using SharpGL;
using SharpGL.WPF;

namespace cg_3;

public partial class MainWindow
{
    private Projection _ortho = new(-21, 21, -21, 21);
    private readonly List<BezierObject> _bezierCurves = new ();
    private readonly List<List<Vector2>> _bezierPoints = new();
    private int _currentCurve = -1;
    private int _currentPoint = 0;
    private int _step;
    private bool _isDrawingMode = true;
    //private bool _isViewMode = false;

    private OpenGL _glContext;
    
    public MainWindow()
    {
        _glContext = new OpenGL();

        InitializeComponent();
        //DrawingModeButton.IsChecked = true;
    }

    #region OpenGL

    private void OnOpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        _glContext = args.OpenGL;
        // var width = _glContext.RenderContextProvider.Width;
        // var height = _glContext.RenderContextProvider.Height;
        
        _glContext.Disable(OpenGL.GL_DEPTH_TEST);
        _glContext.ClearColor(0.88f, 0.88f, 0.88f, 1f);
        _glContext.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
        
        //_glContext.Viewport(0, 0, width, height);
        _glContext.MatrixMode(OpenGL.GL_PROJECTION);
        _glContext.LoadIdentity();
        _glContext.Ortho2D(_ortho.Left, _ortho.Right, _ortho.Bottom, _ortho.Top);
        _glContext.MatrixMode(OpenGL.GL_MODELVIEW);
        _glContext.LoadIdentity();
    }

    private void OnOpenGLResized(object sender, OpenGLRoutedEventArgs args)
        => OnOpenGLInitialized(sender, args);
    
    private void OnOpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        _glContext.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
        
        DrawGrid(2, 4);
        DrawAxes(2, 4);
        
        _glContext.Color(1f, 0f, 0f);
        
        // Control points
        _glContext.PointSize(5);
        _glContext.Begin(OpenGL.GL_POINTS);
        
        foreach (var point in _bezierCurves.Select(curves => curves.ControlPoints)
                     .SelectMany(controlPoints => controlPoints))
        {
            _glContext.Vertex(point.X, point.Y);
        }
        
        _glContext.End();

        // Curves
        foreach (var curvePoints in _bezierPoints)
        {
            _glContext.Begin(OpenGL.GL_LINE_STRIP);

            foreach (var point in curvePoints)
            {
                _glContext.Vertex(point.X, point.Y);
            }
                
            _glContext.End();
        }
        
        _glContext.Flush();
    }

    #endregion

    #region Interface

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        var width = _glContext.RenderContextProvider.Width;
        var height = _glContext.RenderContextProvider.Height;
        var cursorPosition = e.GetPosition(this);
        float xPos = (float)cursorPosition.X / width;
        float yPos = (float)cursorPosition.Y / height;
        Vector2 screenPoint = _ortho.ToScreenCoordinate(xPos, yPos);

        if (!_bezierCurves.IsEmpty() && _isDrawingMode)
        {
            BezierObject bezierCurve = _bezierCurves[_currentCurve];
            
            if (bezierCurve.ControlPoints.Length != 0)
            {
                bezierCurve.UpdateControlPoint(_step, new Vector2(screenPoint.X, screenPoint.Y));
                
                _bezierPoints[_currentCurve].Clear();
                
                for (int i = 0; i <= 20; i++)
                {
                    var t = i / 20.0f;
                
                    _bezierPoints[_currentCurve].Add(bezierCurve.CurveGen(t));
                }
            }   
        }
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _isDrawingMode = true;
        
        var width = _glContext.RenderContextProvider.Width;
        var height = _glContext.RenderContextProvider.Height;
        var cursorPosition = e.GetPosition(this);
        float xPos = (float)cursorPosition.X / width;
        float yPos = (float)cursorPosition.Y / height;
        Vector2 screenPoint = _ortho.ToScreenCoordinate(xPos, yPos);

        if (_step == 0)
        {
            _bezierCurves.Add(new BezierObject());
            _bezierPoints.Add(new List<Vector2>());
            _currentCurve++;
            
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
            _currentCurve++;
            _step = 0;

            var currentBezierCurve = _bezierCurves[_currentCurve];
            var previousBezierCurve = _bezierCurves[_currentCurve - 1];

            currentBezierCurve.AddControlPoint(previousBezierCurve.ControlPoints[^1]);
            currentBezierCurve.AddControlPoint(previousBezierCurve.ControlPoints[^1]);
            _step = 1;
        }
    }

    private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_step > 1 && _isDrawingMode)
        {
            _bezierCurves[_currentCurve].DeleteControlPoint(_step--);
        }
    }
    
    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var width = _glContext.RenderContextProvider.Width;
        var height = _glContext.RenderContextProvider.Height;
        var cursorPosition = e.GetPosition(this);
        float xPos = (float)cursorPosition.X / width;
        float yPos = (float)cursorPosition.Y / height;
        Vector2 screenPoint = _ortho.ToScreenCoordinate(xPos, yPos);

        if (!_isDrawingMode)
        {
            if (e.Delta > 0)
            {   
                Scale(screenPoint, 1.1f);

                foreach (var curve in _bezierCurves)
                {
                    curve.Scale(screenPoint, 1.1f);
                }
            }
            else
            {
                Scale(screenPoint, 0.9f);
                
                foreach (var curve in _bezierCurves)
                {
                    curve.Scale(screenPoint, 0.9f);
                }
            }
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
                _currentCurve--;
            }
        }
    }
    
    private void RadioButton_OnChecked(object sender, RoutedEventArgs e)
    {
        RadioButton radioButton = (sender as RadioButton)!;

        if (radioButton.Name == "DrawingModeButton")
        {
            _isDrawingMode = true;
            //_isViewMode = false;
        }
        if (radioButton.Name == "ViewModeButton")
        {
            _isDrawingMode = false;
            //_isViewMode = true;
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
        
        for (float xAxis = _ortho.Left + 1; xAxis < _ortho.Right; xAxis += xGridStep)
        {
            _glContext.Vertex(xAxis, _ortho.Bottom);
            _glContext.Vertex(xAxis, _ortho.Top);
        }
        
        for (float yAxis = _ortho.Bottom + 1; yAxis < _ortho.Top; yAxis += yGridStep)
        {
            _glContext.Vertex(_ortho.Left, yAxis);
            _glContext.Vertex(_ortho.Right, yAxis);
        }

        _glContext.End();
        _glContext.Disable(OpenGL.GL_LINE_STIPPLE);
    }

    private void DrawAxes(float xGridStep, float yGridStep)
    {
        var xLineLength = _ortho.Width / (5.0f * _ortho.Width);
        var yLineLength = _ortho.Height / (3.0f * _ortho.Height);
        
        _glContext.Color(0, 0, 0);
        _glContext.Begin(OpenGL.GL_LINES);
        _glContext.Vertex(_ortho.Left + 1, _ortho.Bottom + 1);
        _glContext.Vertex(_ortho.Left + 1, _ortho.Top);
        _glContext.Vertex(_ortho.Left + 1, _ortho.Bottom + 1);
        _glContext.Vertex(_ortho.Right, _ortho.Bottom + 1);

        for (float xAxis = _ortho.Left + 1 + xGridStep; xAxis < _ortho.Right; xAxis += xGridStep)
        {
            _glContext.Vertex(xAxis, _ortho.Bottom + 1 - yLineLength);
            _glContext.Vertex(xAxis, _ortho.Bottom + 1 + yLineLength);
        }
        
        for (float yAxis = _ortho.Bottom + 1 + yGridStep; yAxis <  _ortho.Top; yAxis += yGridStep)
        {
            _glContext.Vertex(_ortho.Left + 1 - xLineLength, yAxis);
            _glContext.Vertex(_ortho.Left + 1 + xLineLength, yAxis);
        }
        
        _glContext.End();
    }

    private void Scale(Vector2 pivot, float scale)
    {
        float xStep = pivot.X * scale - pivot.X;
        float yStep = pivot.Y * scale - pivot.Y;

        foreach (var t in _bezierPoints)
        {
            for (int j = 0; j < t.Count; j++)
            {
                var x = t[j].X * scale - xStep;
                var y = t[j].Y * scale - yStep;

                t[j] = new Vector2(x, y);
            }
        }
    }

    #endregion
}