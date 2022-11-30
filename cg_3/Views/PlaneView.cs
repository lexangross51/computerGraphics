using System.Collections;
using System.Collections.ObjectModel;
using cg_3.Source.Render;
using cg_3.Source.Vectors;
using cg_3.ViewModels;
using DynamicData;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ReactiveUI;

namespace cg_3.Views;

public class PlaneView : ReactiveObject, IViewable
{
    public Plane Plane { get; } = new();
    public SourceCache<BezierWrapper, Guid> Wrappers { get; } = new(w => w.Guid);

    public void Draw(IBaseGraphic baseGraphic)
    {
        baseGraphic.Draw(Plane.SelectedSegment, PrimitiveType.LineStrip);
        baseGraphic.DrawPoints(Plane.ControlPoints.Items, 7, Color4.Black);
        baseGraphic.Draw(Plane.Curves, PrimitiveType.LinesAdjacency);
    }

    public Guid FindWrapper(Vector2D point)
    {
        foreach (var wrapper in Wrappers.Items)
        {
            if (wrapper.Points.Any(p => Vector2D.Distance(point, p) < 1E-02) ||
                wrapper.ControlPoints.Any(p => Vector2D.Distance(point, p) < 1E-02)) return wrapper.Guid;
        }

        return Guid.Empty;
    }
}

public class Plane
{
    public SourceList<Vector2D> ControlPoints { get; } = new();
    public List<Vector2D> SelectedSegment { get; } = new();
    public List<Vector2D> Curves { get; } = new();
    public Dictionary<Guid, IEnumerable<Vector2D>> SelectedSegments { get; } = new();
    public Dictionary<Guid, IEnumerable<Vector2D>> SelectedPoints { get; } = new();
}

public class ViewableBezierObject
{
    private readonly PlaneView _planeView;
    private byte _step;

    public BezierWrapper BezierWrapper { get; }

    public ViewableBezierObject(Vector2D point, PlaneView planeView)
    {
        BezierWrapper = new(point, point, point, point);
        _planeView = planeView;
        _planeView.Wrappers.AddOrUpdate(BezierWrapper);
    }

    public Task AddPoint(Vector2D point, ref bool value)
    {
        switch (_step)
        {
            case 0:
                BezierWrapper.P0 = point;
                _planeView.Plane.ControlPoints.Add(point);
                _step++;
                break;
            case 1:
                BezierWrapper.P1 = point;
                _step++;
                _planeView.Plane.ControlPoints.Add(point);
                break;
            case 2:
                BezierWrapper.P2 = point;
                _step++;
                _planeView.Plane.ControlPoints.Add(point);
                break;
            case 3:
                BezierWrapper.P3 = point;
                _planeView.Plane.ControlPoints.Add(point);
                _step = 0;
                value = false;
                break;
        }

        return Task.CompletedTask;
    }

    public Task MovePoint(Vector2D point)
    {
        switch (_step)
        {
            case 1:
                BezierWrapper.P3 = point;
                BezierWrapper.P1 = point;
                break;
            case 2:
                BezierWrapper.P3 = point;
                BezierWrapper.P2 = BezierWrapper.P3;
                break;
            default:
                BezierWrapper.P3 = point;
                break;
        }

        return Task.CompletedTask;
    }

    public void GenerateSegment()
    {
        _planeView.Plane.SelectedSegment.Clear();

        for (int i = 0; i <= 19; i++)
        {
            var t = i / 19.0f;

            _planeView.Plane.SelectedSegment.Add(BezierWrapper.GenCurve(t));
        }
    }
}