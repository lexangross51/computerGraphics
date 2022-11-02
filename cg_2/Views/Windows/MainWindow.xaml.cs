using cg_2.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace cg_2.Views.Windows;

#nullable disable
public partial class MainWindow : IViewFor<MainViewModel>
{
    private readonly MainCamera _camera = new(CameraMode.Perspective);
    private float _deltaTime;

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => throw new NotImplementedException();
    }

    public MainViewModel ViewModel { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = App.Current.Services.GetRequiredService<MainViewModel>();
        var mainSettings = new GLWpfControlSettings();
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new(1920, 1080);
        GL.ClearColor(Color.Black);
        GL.Enable(EnableCap.DepthTest);
        OpenTkControl.Render += ViewModel.DrawingViewModel.OnRender;
        this.Bind(ViewModel, viewModel => viewModel.DrawingViewModel.BaseGraphic.Camera,
            view => view._camera);
        this.WhenAnyValue(view => view.ViewModel.DrawingViewModel.BaseGraphic.DeltaTime,
            deltaTime => _deltaTime = deltaTime).Subscribe();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;

        if (e.KeyboardDevice.IsKeyDown(Key.W)) _camera.Move(CameraMovement.Forward, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.S)) _camera.Move(CameraMovement.Backward, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.A)) _camera.Move(CameraMovement.Left, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.D)) _camera.Move(CameraMovement.Right, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.Space)) _camera.Move(CameraMovement.Up, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)) _camera.Move(CameraMovement.Down, _deltaTime);
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var pos = e.GetPosition(this);

            _camera.LookAt((float)pos.X, (float)pos.Y);
        }
        else
        {
            _camera.FirstMouse = true;
        }
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e) => _camera.Fov -= e.Delta / 100.0f;
}

#nullable restore