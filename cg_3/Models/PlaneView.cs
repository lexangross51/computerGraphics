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
    private readonly ReadOnlyObservableCollection<BezierWrapper> _wrappers;
    private readonly SourceCache<BezierWrapper, BezierWrapper> _wrappersAsSourceCache = new(w => w);
    public Plane Plane { get; }
    public ReadOnlyObservableCollection<BezierWrapper> Wrappers => _wrappers;
    public ReactiveCommand<BezierWrapper, Unit> AddWrapper { get; }

    public PlaneView()
    {
        Plane = new();
        _wrappersAsSourceCache.Connect().Bind(out _wrappers).Subscribe();
        AddWrapper = ReactiveCommand.Create<BezierWrapper>(wrapper => _wrappersAsSourceCache.AddOrUpdate(wrapper));
    }
    
    public void Draw(IBaseGraphic baseGraphic) => baseGraphic.DrawPoints(Plane.Points);
}

public class Plane
{
    private readonly ReadOnlyObservableCollection<Vector2D> _points;
    private readonly SourceCache<Vector2D, Vector2D> _pointsAsSourceCache = new(p => p);
    public ReadOnlyObservableCollection<Vector2D> Points => _points;

    public Plane() => _pointsAsSourceCache.Connect().Bind(out _points).Subscribe();

    public void AddPoint(Vector2D point) => _pointsAsSourceCache.AddOrUpdate(point);
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
        _planeView.Plane.AddPoint(point);
    }

    public void MovePoint(Vector2D point)
    {
        BezierWrapper.P3 = point;
        // _planeView.Plane.AddPoint(point);
    }
}