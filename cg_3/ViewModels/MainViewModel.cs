using cg_3.Source.Render;
using cg_3.Source.Vectors;
using ReactiveUI;

namespace cg_3.ViewModels;

public class MainViewModel : ReactiveObject, IViewable
{
    public void Draw(IBaseGraphic baseGraphic)
    {
        Vector2D[] points = {
            new(-0.5f,  0.5f),
            new(0.5f,  0.5f),
            new(0.5f, -0.5f),
            new(-0.5f, -0.5f)
        };
        
        baseGraphic.DrawPoints(points);
    }
}