using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace cg_2.Views.Windows;

#nullable disable
public partial class MainWindow : IViewFor<MainViewModel>
{
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => throw new NotImplementedException();
    }

    public IBaseGraphic BaseGraphic { get; }
    public MainViewModel ViewModel { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = App.Current.Services.GetRequiredService<MainViewModel>();
        var mainSettings = new GLWpfControlSettings();
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new(1920, 1080);

        BaseGraphic = new RenderServer(new(CameraMode.Perspective));
        BaseGraphic.Camera.AspectRatio =
            (float)(OpenTkControl.RenderSize.Width / OpenTkControl.RenderSize.Height);
        
        ViewModel.Draw(BaseGraphic); // initialize objects
        
        var observable = Observable.FromEvent<TimeSpan>(
            handler => OpenTkControl.Render += handler,
            handler => OpenTkControl.Render -= handler);

        this.WhenActivated(disposables 
            => observable.Subscribe(ts => BaseGraphic.Render(ts)).DisposeWith(disposables));
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        var deltaTime = BaseGraphic.DeltaTime;

        if (e.KeyboardDevice.IsKeyDown(Key.W))
            BaseGraphic.Camera.Move(CameraMovement.Forward, deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.S))
            BaseGraphic.Camera.Move(CameraMovement.Backward, deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.A))
            BaseGraphic.Camera.Move(CameraMovement.Left, deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.D))
            BaseGraphic.Camera.Move(CameraMovement.Right, deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.Space))
            BaseGraphic.Camera.Move(CameraMovement.Up, deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
            BaseGraphic.Camera.Move(CameraMovement.Down, deltaTime);
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var pos = e.GetPosition(this);

            BaseGraphic.Camera.LookAt((float)pos.X, (float)pos.Y);
        }
        else
        {
            BaseGraphic.Camera.FirstMouse = true;
        }
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        => BaseGraphic.Camera.Fov -= e.Delta / 100.0f;
}

#nullable restore