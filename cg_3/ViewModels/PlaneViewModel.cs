namespace cg_3.ViewModels;

public enum Mode
{
    Draw, // default mode
    Select
}

public class PlaneViewModel : ReactiveObject, IViewable
{
    private ObservableAsPropertyHelper<ViewableBezierObject>? _viewableBezierObject;
    private readonly ObservableAsPropertyHelper<Mode> _mode;
    public Subject<(Vector2D, MouseButtonState)> RecreateObject { get; }
    public Mode Mode => _mode.Value;
    public ViewableBezierObject? ViewableBezierObject => _viewableBezierObject?.Value;
    public ReactiveCommand<string, Mode> SetMode { get; }
    public ReactiveCommand<Vector2D, ViewableBezierObject> CreateObject { get; }
    public ReactiveCommand<(Vector2D, MouseButtonState), Unit> AddPoint { get; }
    public ReactiveCommand<(Vector2D, MouseButtonState), Unit> MovePoint { get; }
    public ReactiveCommand<Vector2D, Unit> RestartObject { get; }
    public Plane Plane { get; }
    public SourceList<BezierWrapper> Wrappers { get; }
    [Reactive] public BezierWrapper? SelectedWrapper { get; set; }

    public PlaneViewModel()
    {
        Wrappers = new();
        Plane = new();
        RecreateObject = new();
        SetMode = ReactiveCommand.Create<string, Mode>(str => (Mode)Enum.Parse(typeof(Mode), str));
        SetMode.ToProperty(this, nameof(Mode), out _mode);
        CreateObject =
            ReactiveCommand.Create<Vector2D, ViewableBezierObject>(p => new(this, p));
        var canExecute = this.WhenAnyValue(
            t => t.ViewableBezierObject).Select(item => item is not null);
        AddPoint = ReactiveCommand.CreateFromTask<(Vector2D, MouseButtonState)>(
            async p => await _viewableBezierObject!.Value.AddPointImpl(p.Item1, p.Item2),
            canExecute);
        MovePoint = ReactiveCommand.CreateFromTask<(Vector2D, MouseButtonState)>(
            async p => await _viewableBezierObject!.Value.MovePointImpl(p.Item1, p.Item2),
            canExecute);
        RestartObject = ReactiveCommand.Create<Vector2D>(p =>
            _viewableBezierObject!.Value.Restart(p));
        RecreateObject.Subscribe(p =>
        {
            if (ViewableBezierObject is null)
            {
                CreateObject.Execute(p.Item1).ToProperty(this, nameof(ViewableBezierObject), out _viewableBezierObject);
            }

            AddPoint.Execute((p.Item1, p.Item2));

            if (ViewableBezierObject!.State == StateViewableObject.Completed)
            {
                RestartObject.Execute(p.Item1).Subscribe();
            }
        });
        Plane.SelectedSegments.Connect().Subscribe(_ => FindWrapper());
    }

    public void Draw(IBaseGraphic baseGraphic)
    {
        baseGraphic.Clear();
        baseGraphic.DrawPoints(Plane.SelectedPoints.Items, 6, Color4.Coral);

        foreach (var curves in Plane.SelectedSegments.Items.SkipLast(1))
        {
            baseGraphic.Draw(curves.CompletedPoints, PrimitiveType.LineStrip);
        }

        var selected = Plane.SelectedSegments.Items.LastOrDefault();
        if (selected is null) return;
        baseGraphic.Draw(selected.CompletedPoints, PrimitiveType.LineStrip, Color4.Coral);
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

public class ViewableBezierObject : ReactiveObject
{
    private readonly PlaneViewModel _planeView;
    private BezierObject _bezierObject;
    private BezierWrapper _wrapper;
    private byte _step;

    public StateViewableObject State { get; private set; } = StateViewableObject.NotStarted;

    public ViewableBezierObject(PlaneViewModel planeView, Vector2D point)
    {
        _planeView = planeView;
        _bezierObject = new(point, point, point, point);
        _wrapper = new(_bezierObject);
        _planeView.SelectedWrapper = _wrapper;
        _planeView.Wrappers.Add(_wrapper);
        _planeView.Plane.SelectedSegments.AddOrUpdate(_bezierObject);
        _step = 0;
    }

    public Task AddPointImpl(Vector2D point, MouseButtonState mouseState)
    {
        switch (_step)
        {
            case 0:
                State = StateViewableObject.Started;
                _wrapper.P0 = point;
                _planeView.Plane.SelectedPoints.Add(point);
                _step++;
                break;
            case 1:
                _wrapper.P1 = point;
                _step++;
                _planeView.Plane.SelectedPoints.Add(point);
                break;
            case 2:
                _wrapper.P2 = point;
                _step++;
                _planeView.Plane.SelectedPoints.Add(point);
                break;
            case 3:
                _wrapper.P3 = point;
                _planeView.Plane.SelectedPoints.Add(point);
                _step = 0;
                State = StateViewableObject.Completed;
                break;
        }

        return Task.CompletedTask;
    }

    public Task MovePointImpl(Vector2D point, MouseButtonState mouseState)
    {
        if (_step != 0) GenerateSegment();

        switch (_step)
        {
            case 1:
                _wrapper.P3 = point;
                _wrapper.P1 = point;
                break;
            case 2:
                _wrapper.P3 = point;
                _wrapper.P2 = point;
                break;
            default:
                _wrapper.P3 = point;
                break;
        }

        return Task.CompletedTask;
    }

    private void GenerateSegment()
    {
        _bezierObject.CompletedPoints.Clear();
        _bezierObject.GenCurve();
    }

    public void Restart(Vector2D point)
    {
        _bezierObject = new(point, point, point, point);
        _wrapper = new(_bezierObject);
        _planeView.Wrappers.Add(_wrapper);
        _planeView.Plane.SelectedSegments.AddOrUpdate(_bezierObject);
        _planeView.Plane.SelectedPoints.Clear();
        _planeView.Plane.SelectedPoints.Add(point);
        State = StateViewableObject.Started;
        _step = 1;
    }
}