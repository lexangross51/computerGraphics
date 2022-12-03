namespace cg_3.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IViewFor<PlaneViewModel>
{
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => throw new NotImplementedException();
    }

    private ViewableBezierObject? _bezierObject;
    public PlaneViewModel? ViewModel { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        PreviewMouseDown += PreviewMouseDownEventHandler;
        KeyDown += WindowKeyDownHandler;
        ViewModel = App.Current.Services.GetRequiredService<PlaneViewModel>();
        var mainSettings = new GLWpfControlSettings();
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new(1920, 1080);

        IBaseGraphic baseGraphic = new RenderServer();
        baseGraphic.Camera.AspectRatio =
            (float)(OpenTkControl.RenderSize.Width / OpenTkControl.RenderSize.Height);

        var observable = Observable.FromEvent<TimeSpan>(
            handler => OpenTkControl.Render += handler,
            handler => OpenTkControl.Render -= handler);

        this.WhenActivated(disposables =>
        {
            observable.Subscribe(ts => baseGraphic.Render(ts)).DisposeWith(disposables);

            this.WhenAnyValue(t => t.ViewModel!.Mode)
                .Subscribe(_ =>
                {
                    ViewModel.HaveViewableObject = false;
                    if (_bezierObject != null) _bezierObject.State = StateViewableObject.NotStarted;
                })
                .DisposeWith(disposables);

            this.WhenAnyValue(t => t.ViewModel!.Mode)
                .Where(mode => mode.HasFlag(Mode.Select) && ViewModel.Wrappers.Count != 0)
                .Subscribe(_ =>
                    ViewModel.Wrappers.Remove(ViewModel!.SelectedWrapper!.Guid))
                .DisposeWith(disposables); // delete last wrapper if was restart

            this.WhenAnyValue(t => t.ViewModel!.SelectedWrapper!.P0,
                    t => t.ViewModel!.SelectedWrapper!.P1,
                    t => t.ViewModel!.SelectedWrapper!.P2,
                    t => t.ViewModel!.SelectedWrapper!.P3)
                .Where(_ => _bezierObject?.State is StateViewableObject.NotStarted).Subscribe(_ =>
                {
                    _bezierObject?.GenerateSegmentWithUpdate(ViewModel!.SelectedWrapper!.Guid);
                    ViewModel.DrawSelected(baseGraphic, ViewModel!.SelectedWrapper!.Guid);
                }).DisposeWith(disposables);

            OpenTkControl.Events().MouseDown.Subscribe(async args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                switch (ViewModel.Mode)
                {
                    case Mode.None:
                        return;
                    case Mode.Select:
                    {
                        // if (_bezierObject?.State == StateViewableObject.NotStarted)
                        // {
                        //     ViewModel.Dragged =
                        //         ViewModel!.SelectedWrapper!.ControlPoints.Select((p, idx) => (point: p, index: idx))
                        //             .Where(p => Vector2D.Distance((x, y), p.point) < 1E-01)
                        //             .Select(p => p.index)
                        //             .DefaultIfEmpty(-1).First();
                        // }

                        var key = ViewModel.FindWrapper((x, y));
                        if (key == Guid.Empty) return;
                        ViewModel.SelectedWrapper = ViewModel.Wrappers.Lookup(key).Value;
                        ViewModel.DrawSelected(baseGraphic, key);
                        return;
                    }
                }

                if (!ViewModel.HaveViewableObject)
                {
                    _bezierObject = new((x, y), ViewModel);
                    ViewModel.HaveViewableObject = true;
                }

                await _bezierObject!.AddPoint((x, y));
                ViewModel.Draw(baseGraphic);

                if (_bezierObject.State != StateViewableObject.Completed) return;
                _bezierObject.Restart((x, y));
            }).DisposeWith(disposables);

            ViewModel.Plane.SelectedPoints.CountChanged.Where(_ => ViewModel.Mode is not Mode.Select)
                .Subscribe(_ => ViewModel.Draw(baseGraphic)).DisposeWith(disposables);

            OpenTkControl.Events().MouseMove.Subscribe(async args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                MousePositionX.Text = x.ToString("G7", CultureInfo.InvariantCulture);
                MousePositionY.Text = y.ToString("G7", CultureInfo.InvariantCulture);

                if (ViewModel.Mode.HasFlag(Mode.Select))
                {
                    ViewModel.Plane.ClearSelected();
                    ViewModel.DrawSelected(baseGraphic, ViewModel!.SelectedWrapper!.Guid);
                    return;
                }

                if (ViewModel.Plane.SelectedPoints.Count == 0) return;
                await _bezierObject!.MovePoint((x, y));
                _bezierObject.GenerateSegment();
                ViewModel.Draw(baseGraphic);
            }).DisposeWith(disposables);

            this.Events().KeyDown
                .Where(x => x.Key == Key.Z && x.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
                .Subscribe(_ =>
                {
                    ViewModel.Cancel.Execute().Subscribe();
                    ViewModel.Draw(baseGraphic);
                })
                .DisposeWith(disposables);
        });
    }

    private static void ClearFocus()
    {
        var elementWithFocus = Keyboard.FocusedElement as UIElement;
        if (elementWithFocus is not System.Windows.Controls.TextBox) return;
        if (Keyboard.FocusedElement == null) return;
        Keyboard.FocusedElement.RaiseEvent(new(LostFocusEvent));
        Keyboard.ClearFocus();
    }

    private static void PreviewMouseDownEventHandler(object sender, MouseButtonEventArgs e) => ClearFocus();

    private static void WindowKeyDownHandler(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) ClearFocus();
    }
}