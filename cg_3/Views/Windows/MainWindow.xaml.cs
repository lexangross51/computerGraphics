namespace cg_3.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IViewFor<PlaneViewModel>
{
#nullable disable

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => throw new NotImplementedException();
    }

    public PlaneViewModel ViewModel { get; set; }

#nullable restore

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

            ViewModel.Plane.SelectedPoints.Connect().OnItemAdded(_ => ViewModel.Draw(baseGraphic)).Subscribe()
                .DisposeWith(disposables);

            OpenTkControl.Events().MouseDown
                .Select(args => (args.ToScreenCoordinates(OpenTkControl), MouseButtonState.Pressed))
                .Subscribe(ViewModel.RecreateObject).DisposeWith(disposables);

            OpenTkControl.Events().MouseMove
                .Select(args => (args.ToScreenCoordinates(OpenTkControl), MouseButtonState.Pressed))
                .Subscribe(parameters =>
                {
                    MousePositionX.Text = parameters.Item1.X.ToString("G7", CultureInfo.InvariantCulture);
                    MousePositionY.Text = parameters.Item1.Y.ToString("G7", CultureInfo.InvariantCulture);

                    ViewModel.MovePoint.Execute((parameters.Item1, parameters.Item2));
                    ViewModel.Draw(baseGraphic);
                }).DisposeWith(disposables);
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