using cg_3.Source.Render;
using ReactiveUI;

namespace cg_3.ViewModels;

public class MainViewModel : ReactiveObject, IViewable
{
    public void Draw(IBaseGraphic baseGraphic)
    {
        var wrapper = new BezierWrapper(new
            (
                new(-0.8f, 0.0f),
                new(-0.5f, 0.5f),
                new(0.0f, -0.5f),
                new(0.5f, 0.0f))
        );

        wrapper.GenCurve();
        baseGraphic.DrawPoints(wrapper.Points!);
    }
}