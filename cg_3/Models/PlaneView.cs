using System.Collections.ObjectModel;
using System.Reactive;
using cg_3.Source.Render;
using cg_3.Source.Vectors;
using cg_3.ViewModels;
using DynamicData;
using ReactiveUI;

namespace cg_3.Models;

public class PlaneView : IViewable
{
    public SourceCache<BezierWrapper, BezierWrapper> WrappersAsSourceCache { get; } = new(w => w);
    public Plane Plane { get; }
    public ReactiveCommand<BezierWrapper, Unit> AddWrapper { get; }

    public PlaneView()
    {
        Plane = new();
        AddWrapper = ReactiveCommand.Create<BezierWrapper>(wrapper => WrappersAsSourceCache.AddOrUpdate(wrapper));
    }

    public void Draw(IBaseGraphic baseGraphic) => baseGraphic.DrawPoints(Plane.Points.Items);
}

public class Plane
{
    public SourceCache<Vector2D, Vector2D> Points { get; } = new(p => p);
}

public class ViewableBezierObject
{
    private readonly PlaneView _planeView;
    public BezierWrapper BezierWrapper { get; set; }

    public ViewableBezierObject(PlaneView planeView, Vector2D point)
    {
        _planeView = planeView;
        BezierWrapper = new(point, point, point, point);
    }

    public void AddPoint(Vector2D point)
    {
        BezierWrapper.P0 = point;
        // _planeView.Plane.AddPoint(point);
    }

    public void MovePoint(Vector2D point)
    {
        BezierWrapper.P3 = point;
        // _planeView.Plane.AddPoint(point);
    }
}