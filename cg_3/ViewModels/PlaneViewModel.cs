namespace cg_3.ViewModels;

public class PlaneViewModel : ReactiveObject, IViewable
{
    private ObservableAsPropertyHelper<ViewableBezierObject>? _viewableBezierObject;
    public ViewableBezierObject? ViewableBezierObject => _viewableBezierObject?.Value;
    public ReactiveCommand<Vector2D, ViewableBezierObject> CreateObject { get; }
    public Subject<(Vector2D, MouseButtonEventArgs)> RecreateObject { get; }
    public Subject<(Vector2D, MouseEventArgs)> MoveAndDrag { get; }
    public ReactiveCommand<(Vector2D, MouseButtonEventArgs), Unit> AddPoint { get; }
    public ReactiveCommand<(Vector2D, MouseEventArgs), Unit> MovePoint { get; }
    public ReactiveCommand<Vector2D, Unit> RestartObject { get; }
    public Plane Plane { get; }
    public SourceList<BezierWrapper> Wrappers { get; }
    [Reactive] public BezierWrapper? SelectedWrapper { get; set; }
    [Reactive] public bool IsSelectedMode { get; set; }
    public Dragger Dragger { get; }

    public PlaneViewModel()
    {
        Wrappers = new();
        Plane = new();
        RecreateObject = new();
        MoveAndDrag = new();
        Dragger = new();
        CreateObject =
            ReactiveCommand.Create<Vector2D, ViewableBezierObject>(p => new(this, p));
        var canExecute = this.WhenAnyValue(
            t => t.ViewableBezierObject).Select(item => item is not null);
        AddPoint = ReactiveCommand.CreateFromTask<(Vector2D, MouseButtonEventArgs)>(
            async p => await _viewableBezierObject!.Value.AddPointImpl(p.Item1, p.Item2),
            canExecute);
        MovePoint = ReactiveCommand.CreateFromTask<(Vector2D, MouseEventArgs)>(
            async p => await _viewableBezierObject!.Value.MovePointImpl(p.Item1, p.Item2),
            canExecute);
        RestartObject = ReactiveCommand.Create<Vector2D>(p =>
            _viewableBezierObject!.Value.Restart(p));
        RecreateObject.Subscribe(p =>
        {
            if (IsSelectedMode)
            {
                foreach (var segment in Plane.SelectedSegments.Items)
                {
                    if (!segment.CompletedPoints.Any(point => Vector2D.Distance(p.Item1, point) < 1E-01)) continue;
                    Plane.SelectedSegment = segment;
                    break;
                }

                Dragger.Wrapper = SelectedWrapper;
                Dragger.FindPoint(p.Item2, p.Item1);

                return;
            }

            if (ViewableBezierObject?.Wrapper is null)
            {
                CreateObject.Execute(p.Item1)
                    .ToProperty(this, nameof(ViewableBezierObject), out _viewableBezierObject);
            }

            AddPoint.Execute((p.Item1, p.Item2));

            if (ViewableBezierObject!.State == StateViewableObject.Completed)
            {
                RestartObject.Execute(p.Item1).Subscribe();
            }
        });
        MoveAndDrag.Subscribe(p =>
        {
            MovePoint.Execute((p.Item1, p.Item2));
            Dragger.DragPoint(p.Item1, p.Item2);
        });
        this.WhenAnyValue(t => t.IsSelectedMode).Where(value => value).Subscribe(_ =>
        {
            if (ViewableBezierObject?.Wrapper is null) return;
            Plane.SelectedSegments.RemoveKey(ViewableBezierObject.Wrapper.Curve);
            Wrappers!.Remove(SelectedWrapper);
            Plane.SelectedPoints.Clear();
            ViewableBezierObject?.Dispose();
            FindWrapper();
        });
    }

    public void Draw(IBaseGraphic baseGraphic)
    {
        if (IsSelectedMode) return;
        Plane.SelectedSegment = null;

        baseGraphic.Clear();
        baseGraphic.DrawPoints(Plane.SelectedPoints.Items, 6);

        foreach (var curves in Plane.SelectedSegments.Items)
        {
            baseGraphic.Draw(curves.CompletedPoints, PrimitiveType.LineStrip);
        }
    }

    public void DrawSelected(IBaseGraphic baseGraphic)
    {
        if (!IsSelectedMode) return;

        baseGraphic.Clear();
        baseGraphic.DrawPoints(Plane.SelectedPoints.Items, 6, Color4.Coral);

        foreach (var curves in Plane.SelectedSegments.Items.Where(curve => curve != Plane.SelectedSegment))
        {
            baseGraphic.Draw(curves.CompletedPoints, PrimitiveType.LineStrip);
        }

        if (Plane.SelectedSegment is null) return;

        var points = new[]
            { Plane.SelectedSegment[0], Plane.SelectedSegment[1], Plane.SelectedSegment[2], Plane.SelectedSegment[3] };

        baseGraphic.Draw(Plane.SelectedSegment.CompletedPoints, PrimitiveType.LineStrip, Color4.Coral);
        baseGraphic.DrawPoints(points, 6, Color4.Coral);
    }

    public void FindWrapper()
    {
        foreach (var wrapper in Wrappers.Items)
        {
            if (wrapper.Curve == Plane.SelectedSegment) SelectedWrapper = wrapper;
        }
    }
}

public enum StateViewableObject
{
    NotStarted,
    Started,
    Completed
}

public class ViewableBezierObject : ReactiveObject, IDisposable
{
    private readonly PlaneViewModel _planeView;
    private BezierObject? _bezierObject;
    private byte _step;

    public BezierWrapper? Wrapper { get; set; }
    public StateViewableObject State { get; private set; } = StateViewableObject.NotStarted;

    public ViewableBezierObject(PlaneViewModel planeView, Vector2D point)
    {
        _planeView = planeView;
        _bezierObject = new(point, point, point, point);
        Wrapper = new(_bezierObject);
        _planeView.SelectedWrapper = Wrapper;
        _planeView.Wrappers.Add(Wrapper);
        _planeView.Plane.SelectedSegments.AddOrUpdate(_bezierObject);
        _step = 0;
    }

    public Task AddPointImpl(Vector2D point, MouseButtonEventArgs mouseState)
    {
        if (_bezierObject is null || Wrapper is null) return Task.CompletedTask;

        switch (_step)
        {
            case 0:
                State = StateViewableObject.Started;
                Wrapper.P0 = point;
                _planeView.Plane.SelectedPoints.Add(point);
                _step++;
                break;
            case 1:
                Wrapper.P1 = point;
                _step++;
                _planeView.Plane.SelectedPoints.Add(point);
                break;
            case 2:
                Wrapper.P2 = point;
                _step++;
                _planeView.Plane.SelectedPoints.Add(point);
                break;
            case 3:
                Wrapper.P3 = point;
                _planeView.Plane.SelectedPoints.Add(point);
                _step = 0;
                State = StateViewableObject.Completed;
                break;
        }

        return Task.CompletedTask;
    }

    public Task MovePointImpl(Vector2D point, MouseEventArgs mouseState)
    {
        if (_bezierObject is null || Wrapper is null) return Task.CompletedTask;
        if (_step != 0) GenerateSegment();

        switch (_step)
        {
            case 1:
                Wrapper.P3 = point;
                Wrapper.P1 = point;
                break;
            case 2:
                Wrapper.P3 = point;
                Wrapper.P2 = point;
                break;
            default:
                Wrapper.P3 = point;
                break;
        }

        return Task.CompletedTask;
    }

    private void GenerateSegment()
    {
        _bezierObject?.CompletedPoints.Clear();
        _bezierObject?.GenCurve();
    }

    public void Restart(Vector2D point)
    {
        _bezierObject = new(point, point, point, point);
        Wrapper = new(_bezierObject);
        _planeView.Wrappers.Add(Wrapper);
        _planeView.SelectedWrapper = Wrapper;
        _planeView.Plane.SelectedSegments.AddOrUpdate(_bezierObject);
        _planeView.Plane.SelectedPoints.Clear();
        _planeView.Plane.SelectedPoints.Add(point);
        State = StateViewableObject.Started;
        _step = 1;
    }

    public void Dispose()
    {
        _bezierObject = null;
        Wrapper = null;
    }
}

public class Dragger
{
    public BezierWrapper? Wrapper { get; set; }
    public int Point { get; private set; }

    public void FindPoint(MouseEventArgs mouseState, Vector2D point)
    {
        if (mouseState.LeftButton != MouseButtonState.Pressed || Wrapper is null)
        {
            Point = -1;
            return;
        }

        Point = Wrapper.Curve.ControlPoints.Select((p, idx) => (point: p, index: idx))
            .Where(p => Vector2D.Distance(point, p.point) < 1E-01)
            .Select(p => p.index)
            .DefaultIfEmpty(-1).First();
    }

    public void DragPoint(Vector2D point, MouseEventArgs mouseState)
    {
        if (Wrapper is null || Point == -1) return;
        if (mouseState.LeftButton != MouseButtonState.Pressed) return;
        Wrapper.SetPoint(Point, point);
    }
}