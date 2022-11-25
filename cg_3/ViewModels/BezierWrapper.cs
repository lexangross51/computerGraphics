using cg_3.Models;
using cg_3.Source.Vectors;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_3.ViewModels;

public class BezierWrapper : ReactiveObject
{
    private readonly CubicBezier _bezierCurve;

    [Reactive] public Vector2D P0 { get; set; }
    [Reactive] public Vector2D P1 { get; set; }
    [Reactive] public Vector2D P2 { get; set; }
    [Reactive] public Vector2D P3 { get; set; }
    
    public BezierWrapper(CubicBezier bezierCurve) => _bezierCurve = bezierCurve;
}