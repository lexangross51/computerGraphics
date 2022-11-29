using System.Collections;
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
        baseGraphic.Draw(Plane.SelectedCurves, PrimitiveType.LineStrip);
        baseGraphic.DrawPoints(Plane.ControlPoints.Items);
        baseGraphic.Draw(Plane.Curves, PrimitiveType.LinesAdjacency);
    }

    public Guid FindWrapper(Vector2D point)
    {
        foreach (var wrapper in Wrappers.Items)
        {
            if (wrapper.Points.Any(p => Vector2D.Distance(point, p) < 0.5) ||
                wrapper.ControlPoints.Any(p => Vector2D.Distance(point, p) < 0.5)) return wrapper.Guid;
        }

        return Guid.Empty;
    }
}

public class Plane
{
    public SourceList<Vector2D> ControlPoints { get; } = new();
    public List<Vector2D> SelectedCurves { get; } = new();
    public List<Vector2D> Curves { get; } = new();
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
}