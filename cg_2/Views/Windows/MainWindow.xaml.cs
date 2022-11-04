using cg_2.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace cg_2.Views.Windows;

#nullable disable
public partial class MainWindow : IViewFor<MainViewModel>
{
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
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        var deltaTime = ViewModel.DrawingViewModel.BaseGraphic.DeltaTime;

        if (e.KeyboardDevice.IsKeyDown(Key.W))
            ViewModel.DrawingViewModel.BaseGraphic.Camera.Move(CameraMovement.Forward, deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.S))
            ViewModel.DrawingViewModel.BaseGraphic.Camera.Move(CameraMovement.Backward, deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.A))
            ViewModel.DrawingViewModel.BaseGraphic.Camera.Move(CameraMovement.Left, deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.D))
            ViewModel.DrawingViewModel.BaseGraphic.Camera.Move(CameraMovement.Right, deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.Space))
            ViewModel.DrawingViewModel.BaseGraphic.Camera.Move(CameraMovement.Up, deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
            ViewModel.DrawingViewModel.BaseGraphic.Camera.Move(CameraMovement.Down, deltaTime);
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var pos = e.GetPosition(this);

            ViewModel.DrawingViewModel.BaseGraphic.Camera.LookAt((float)pos.X, (float)pos.Y);
        }
        else
        {
            ViewModel.DrawingViewModel.BaseGraphic.Camera.FirstMouse = true;
        }
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        => ViewModel.DrawingViewModel.BaseGraphic.Camera.Fov -= e.Delta / 100.0f;
}

#nullable restore