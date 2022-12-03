using System.Diagnostics;

namespace cg_3.ViewModels;

public enum Mode
{
    Select
}

public class PlaneViewModel : ReactiveObject, IViewable
{
    private readonly ObservableAsPropertyHelper<ViewableBezierObject>? _viewableBezierObject;
    private readonly ObservableAsPropertyHelper<Mode> _mode;
    public Mode Mode => _mode.Value;
    public ViewableBezierObject? ViewableBezierObject => _viewableBezierObject?.Value;
    public ReactiveCommand<string, Mode> SetMode { get; }
    public ReactiveCommand<Unit, ViewableBezierObject> CreateObject { get; }
    public ReactiveCommand<(Vector2D, MouseButtonState), Unit> AddPoint { get; }
    public ReactiveCommand<(Vector2D, MouseButtonState), Unit> MovePoint { get; }
    public Plane Plane { get; }
    public SourceList<BezierWrapper> Wrappers { get; }
    [Reactive] public BezierWrapper? SelectedWrapper { get; set; }

    public PlaneViewModel()
    {
        Wrappers = new();
        Plane = new();
        SetMode = ReactiveCommand.Create<string, Mode>(str => (Mode)Enum.Parse(typeof(Mode), str));
        SetMode.ToProperty(this, nameof(Mode), out _mode);
        CreateObject =
            ReactiveCommand.Create<Unit, ViewableBezierObject>(_ => new(this, Vector2D.Zero));
        CreateObject.ToProperty(this, nameof(ViewableBezierObject), out _viewableBezierObject);
        var canExecute = this.WhenAnyValue(
            t => t.ViewableBezierObject).Select(item => item is not null);
        AddPoint = ReactiveCommand.CreateFromTask<(Vector2D, MouseButtonState)>(
            async p => await _viewableBezierObject.Value.AddPointImpl(p.Item1, p.Item2),
            canExecute);
        MovePoint = ReactiveCommand.CreateFromTask<(Vector2D, MouseButtonState)>(
            async p => await _viewableBezierObject.Value.MovePointImpl(p.Item1, p.Item2),
            canExecute);
        Plane.SelectedSegments.Connect().Subscribe(_ => FindWrapper());
    }

    public void Draw(IBaseGraphic baseGraphic)
    {
        baseGraphic.Clear();
        baseGraphic.DrawPoints(Plane.SelectedPoints.Items, 6);

        foreach (var curves in Plane.SelectedSegments.Items)
        {
            baseGraphic.Draw(curves.CompletedPoints.Items, PrimitiveType.LineStrip);
        }
    }

    public void DrawSelected(IBaseGraphic baseGraphic)
    {
    }

    private void FindWrapper()
    {
        foreach (var wrapper in Wrappers.Items)
        {
            foreach (var bezierObject in Plane.SelectedSegments.Items)
            {
                if (wrapper.Curve == bezierObject)
                {
                    SelectedWrapper = wrapper;
                }
            }
        }
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
    private BezierObject? _bezierObject;
    private byte _step;

    public StateViewableObject State { get; private set; } = StateViewableObject.NotStarted;

    public ViewableBezierObject(PlaneViewModel planeView, Vector2D point)
    {
        _planeView = planeView;
        _bezierObject = new(point, point, point, point);
        BezierWrapper wrapper = new(_bezierObject);
        _planeView.SelectedWrapper = wrapper;
        _planeView.Wrappers.Add(wrapper);
        _planeView.Plane.SelectedSegments.AddOrUpdate(_bezierObject);
        _step = 0;
    }

    public Task AddPointImpl(Vector2D point, MouseButtonState mouseState)
    {
        if (_bezierObject is null) return Task.CompletedTask;

        switch (_step)
        {
            case 0:
                State = StateViewableObject.Started;
                _bezierObject[0] = point;
                _planeView.Plane.SelectedPoints.Add(point);
                _step++;
                break;
            case 1:
                _bezierObject[1] = point;
                _step++;
                _planeView.Plane.SelectedPoints.Add(point);
                break;
            case 2:
                _bezierObject[2] = point;
                _step++;
                _planeView.Plane.SelectedPoints.Add(point);
                break;
            case 3:
                _bezierObject[3] = point;
                _planeView.Plane.SelectedPoints.Add(point);
                _step = 0;
                State = StateViewableObject.Completed;
                break;
        }

        return Task.CompletedTask;
    }

    public Task MovePointImpl(Vector2D point, MouseButtonState mouseState)
    {
        if (_bezierObject is null) return Task.CompletedTask;

        GenerateSegment();

        switch (_step)
        {
            case 1:
                _bezierObject[3] = point;
                _bezierObject[1] = point;
                break;
            case 2:
                _bezierObject[3] = point;
                _bezierObject[2] = point;
                break;
            default:
                _bezierObject[3] = point;
                break;
        }

        return Task.CompletedTask;
    }

    public void GenerateSegment()
    {
        _bezierObject?.CompletedPoints.Clear();
        _bezierObject?.GenCurve();
    }

    public void Restart(Vector2D point)
    {
        _bezierObject = new(point, point, point, point);
        BezierWrapper bezierWrapper = new(_bezierObject);
        _planeView.Wrappers.Add(bezierWrapper);
        State = StateViewableObject.NotStarted;
        _planeView.Plane.SelectedPoints.Add(point);
        State = StateViewableObject.Started;
        _step = 1;
    }
}

// public class ViewableBezierObject
// {
//     private readonly PlaneViewModel _planeView;
//     private byte _step;
//
//     public BezierWrapper BezierWrapper { get; private set; }
//     public StateViewableObject State { get; set; } = StateViewableObject.NotStarted;
//
//     public ViewableBezierObject(PlaneViewModel planeView)
//     {
//         BezierWrapper = new(point, point, point, point);
//         _planeView = planeView;
//         _planeView.Wrappers.AddOrUpdate(BezierWrapper);
//         _step = 0;
//     }
//
//     public Task AddPoint(Vector2D point)
//     {
//         switch (_step)
//         {
//             case 0:
//                 State = StateViewableObject.Started;
//                 BezierWrapper.P0 = point;
//                 _planeView.Plane.SelectedPoints.Add(point);
//                 _step++;
//                 break;
//             case 1:
//                 BezierWrapper.P1 = point;
//                 _step++;
//                 _planeView.Plane.SelectedPoints.Add(point);
//                 break;
//             case 2:
//                 BezierWrapper.P2 = point;
//                 _step++;
//                 _planeView.Plane.SelectedPoints.Add(point);
//                 break;
//             case 3:
//                 BezierWrapper.P3 = point;
//                 _planeView.Plane.SelectedPoints.Add(point);
//                 _step = 0;
//                 BezierWrapper.Points.AddRange(_planeView.Plane.SelectedSegment);
//                 State = StateViewableObject.Completed;
//                 break;
//         }
//
//         return Task.CompletedTask;
//     }
//
//     public Task MovePoint(Vector2D point)
//     {
//         switch (_step)
//         {
//             case 1:
//                 BezierWrapper.P3 = point;
//                 BezierWrapper.P1 = point;
//                 break;
//             case 2:
//                 BezierWrapper.P3 = point;
//                 BezierWrapper.P2 = BezierWrapper.P3;
//                 break;
//             default:
//                 BezierWrapper.P3 = point;
//                 break;
//         }
//
//         return Task.CompletedTask;
//     }
//
//     public void GenerateSegment()
//     {
//         _planeView.Plane.SelectedSegment.Clear();
//
//         for (int i = 0; i <= 19; i++)
//         {
//             var t = i / 19.0f;
//
//             _planeView.Plane.SelectedSegment.Add(BezierWrapper.GenCurve(t));
//         }
//     }
//
//     public void GenerateSegmentWithUpdate(Guid key)
//     {
//         var points = new List<Vector2D>();
//         var selected = _planeView.Wrappers.Lookup(key);
//
//         if (!selected.HasValue) return;
//
//         for (int i = 0; i <= 19; i++)
//         {
//             var t = i / 19.0f;
//
//             points.Add(selected.Value.GenCurve(t));
//         }
//
//         _planeView.Wrappers.Edit(wrappers =>
//         {
//             var lookup = wrappers.Lookup(key);
//             lookup.Value.Points = points;
//         });
//     }
//
//     public void UpdateControlPoint(Guid key, Vector2D point)
//     {
//         _planeView.Wrappers.Edit(wrappers =>
//         {
//             var lookup = wrappers.Lookup(key);
//             lookup.Value.ControlPoints[_planeView.Dragged] = point;
//         });
//     }
//
//     public void Restart(Vector2D point)
//     {
//         BezierWrapper = new(point, point, point, point);
//         _planeView.Wrappers.AddOrUpdate(BezierWrapper);
//         State = StateViewableObject.NotStarted;
//         _planeView.Plane.ClearSelected();
//         _planeView.Plane.SelectedPoints.Add(point);
//         State = StateViewableObject.Started;
//         _step = 1;
//     }
// }