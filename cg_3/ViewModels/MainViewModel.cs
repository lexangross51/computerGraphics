using cg_3.Source.Render;
using cg_3.Source.Vectors;
using OpenTK.Mathematics;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_3.ViewModels;

public class MainViewModel : ReactiveObject, IViewable
{
    [Reactive] public BezierWrapper Wrapper { get; set; }
    [Reactive] public string Title { get; set; } = "Title";
    [Reactive] public Vector2D Vec { get; set; }

    public void Draw(IBaseGraphic baseGraphic)
    {
        Wrapper = new(new
            (
                new(-0.8f, 0.0f),
                new(-0.5f, 0.5f),
                new(0.0f, 0.5f),
                new(0.5f, 0.0f))
        );

        Vec = new(1.0f, 1.0f);

        Wrapper.GenCurve();
        baseGraphic.DrawPoints(Wrapper.Points!);
    }
}