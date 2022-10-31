using ReactiveUI;

namespace cg_2.ViewModels;

public class DrawingViewModel : ReactiveObject
{
    public IBaseGraphic BaseGraphic { get; }
    public MainCamera Camera { get; }

    public DrawingViewModel(IBaseGraphic baseGraphic)
    {
        Camera = new(CameraMode.Perspective);
        BaseGraphic = baseGraphic;
    }

    public void OnRender(TimeSpan deltaTime)
    {
        BaseGraphic.DeltaTime = (float)deltaTime.TotalMilliseconds;
        BaseGraphic.Render(Camera);
    }
}