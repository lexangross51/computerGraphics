using System.Reactive;
using System.Reactive.Linq;
using cg_3.Models;
using cg_3.Source.Render;
using cg_3.Source.Vectors;
using cg_3.Views;
using DynamicData;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_3.ViewModels;

public class PlaneViewModel : ReactiveObject
{
    private readonly ObservableAsPropertyHelper<bool> _isDrawingMode;
    private readonly ObservableAsPropertyHelper<bool> _isSelectSegmentMode;
    public ReactiveCommand<Unit, bool> SetDrawingMode { get; }
    public ReactiveCommand<Unit, bool> SetSelectSegmentMode { get; }
    public bool IsDrawingMode => _isDrawingMode.Value;
    public bool IsSelectSegmentMode => _isSelectSegmentMode.Value;
    [Reactive] public BezierWrapper? SelectedWrapper { get; set; }
    public Plane Plane { get; } = new();
    public SourceCache<BezierWrapper, Guid> Wrappers { get; } = new(w => w.Guid);
    public bool HaveViewableObject { get; set; }
    public ReactiveCommand<Unit, Unit> Cancel { get; }

    public PlaneViewModel()
    {
        Cancel = ReactiveCommand.Create(() =>
        {
            Plane.ClearSelected();
            Wrappers.Edit(wrappers =>
            {
                var list = wrappers.Items.ToList();
                wrappers.Clear();
                wrappers.AddOrUpdate(list.SkipLast(1));
            });
        });

        SetDrawingMode = ReactiveCommand.CreateFromObservable<Unit, bool>(_ => Observable.Return(!IsDrawingMode));
        SetDrawingMode.ToProperty(this, t => t.IsDrawingMode, out _isDrawingMode);
        SetSelectSegmentMode =
            ReactiveCommand.CreateFromObservable<Unit, bool>(_ => Observable.Return(!IsSelectSegmentMode));
        SetSelectSegmentMode.ToProperty(this, t => t.IsSelectSegmentMode, out _isSelectSegmentMode);
        Wrappers.Connect().OnItemAdded(wrapper => SelectedWrapper = wrapper).Subscribe();
    }

    public void Draw(IBaseGraphic baseGraphic)
    {
        baseGraphic.Clear();
        baseGraphic.Draw(Plane.SelectedSegment, PrimitiveType.LineStrip, Color4.Coral);
        baseGraphic.DrawPoints(Plane.SelectedPoints.Items, pointSize: 6);

        foreach (var wrapper in Wrappers.Items)
        {
            baseGraphic.Draw(wrapper.Points, PrimitiveType.LineStrip);
        }
    }

    public void DrawSelected(IBaseGraphic baseGraphic, Guid key)
    {
        baseGraphic.Clear();
        baseGraphic.Draw(Plane.SelectedSegment, PrimitiveType.LineStrip, Color4.Coral);
        baseGraphic.DrawPoints(Plane.SelectedPoints.Items, pointSize: 6);

        foreach (var wrapper in Wrappers.Items.Where(wrapper => wrapper.Guid != key))
        {
            baseGraphic.Draw(wrapper.Points, PrimitiveType.LineStrip);
        }

        var selected = Wrappers.Lookup(key);

        baseGraphic.Draw(selected.Value.Points, PrimitiveType.LineStrip, Color4.Coral);
        baseGraphic.DrawPoints(selected.Value.ControlPoints, color: Color4.Coral,
            pointSize: 6); // TODO figure out blend 
    }

    public Guid FindWrapper(Vector2D point)
    {
        foreach (var wrapper in Wrappers.Items)
        {
            if (wrapper.Points.Any(p => Vector2D.Distance(point, p) < 1E-01)) return wrapper.Guid;
        }

        return Guid.Empty;
    }
}

public enum StateViewableObject
{
    NotStarted,
    Started,
    Completed
}

public class ViewableBezierObject
{
    private readonly PlaneViewModel _planeView;
    private byte _step;

    public BezierWrapper BezierWrapper { get; private set; }
    public StateViewableObject State { get; private set; } = StateViewableObject.NotStarted;

    public ViewableBezierObject(Vector2D point, PlaneViewModel planeView)
    {
        BezierWrapper = new(point, point, point, point);
        _planeView = planeView;
        _planeView.Wrappers.AddOrUpdate(BezierWrapper);
        _step = 0;
    }

    public Task AddPoint(Vector2D point)
    {
        switch (_step)
        {
            case 0:
                State = StateViewableObject.Started;
                BezierWrapper.P0 = point;
                _planeView.Plane.SelectedPoints.Add(point);
                _step++;
                break;
            case 1:
                BezierWrapper.P1 = point;
                _step++;
                _planeView.Plane.SelectedPoints.Add(point);
                break;
            case 2:
                BezierWrapper.P2 = point;
                _step++;
                _planeView.Plane.SelectedPoints.Add(point);
                break;
            case 3:
                BezierWrapper.P3 = point;
                _planeView.Plane.SelectedPoints.Add(point);
                _step = 0;
                _planeView.Wrappers.Lookup(BezierWrapper.Guid).Value.Points
                    .AddRange(_planeView.Plane.SelectedSegment);
                State = StateViewableObject.Completed;
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

    public void Restart(Vector2D point)
    {
        BezierWrapper = new(point, point, point, point);
        _planeView.Wrappers.AddOrUpdate(BezierWrapper);
        State = StateViewableObject.NotStarted;
        _planeView.Plane.ClearSelected();
        _planeView.Plane.SelectedPoints.Add(point);
        State = StateViewableObject.Started;
        _step = 1;
    }
}