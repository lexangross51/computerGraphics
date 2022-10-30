using cg_2.Infrastructure.Commands;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cg_2.ViewModels;

public class GlControlViewModel : ObservableObject
{
    private readonly Camera _camera = new();
    private ICommand? _onRenderCommand;
    private ICommand? _onInitializeCommand;
    private ICommand? _onMoveMouseCommand;

    public OpenGLControl? GlControl { get; }
    public ICommand OnRenderCommand => _onRenderCommand ??= new LambdaCommand(OnRenderCommandExecuted);
    public ICommand OnInitializeCommand => _onInitializeCommand ??= new LambdaCommand(OnInitializeCommandExecuted);
    public ICommand OnMoveMouseCommand => _onMoveMouseCommand ??= new LambdaCommand(OnMouseMoveCommandExecuted);

    public GlControlViewModel()
    {
        GlControl = new();
        // GlControl.OpenGLInitialized += OnInitializeCommandExecuted(new OpenGLRoutedEventArgs(), GlControl.OpenGL));
    }

    public void OnRenderCommandExecuted()
    {
    }

    public void OnInitializeCommandExecuted(object parameter)
    {
        if (parameter is not OpenGLRoutedEventArgs args) return;

        var gl = args.OpenGL;
        var gl1 = GlControl.OpenGL;
        gl.Enable(OpenGL.GL_DEPTH_TEST);
        gl.Enable(OpenGL.GL_DOUBLEBUFFER);
    }

    public void OnMouseMoveCommandExecuted(object parameter)
    {
        if (parameter is not MouseEventArgs e) return;

        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var pos = e.GetPosition((IInputElement)e.Source);

            _camera.LookAt((float)pos.X, (float)pos.Y);
        }
        else
        {
            _camera.FirstMouse = true;
        }
    }
}