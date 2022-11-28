using cg_3.Models;
using cg_3.Source.Vectors;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_3.ViewModels;

public class BezierWrapper : ReactiveObject
{
    public Guid Guid { get; }
    [Reactive] public Vector2D P0 { get; set; }
    [Reactive] public Vector2D P1 { get; set; }
    [Reactive] public Vector2D P2 { get; set; }
    [Reactive] public Vector2D P3 { get; set; }

    public BezierWrapper(Vector2D p0, Vector2D p1, Vector2D p2, Vector2D p3)
    {
        P0 = p0;
        P1 = p1;
        P2 = p2;
        P3 = p3;
        Guid = Guid.NewGuid();
    }

    public Vector2D GenCurve(float t) => AsBezierObject().CurveGen(t);

    public BezierObject AsBezierObject() => new(P0, P1, P2, P3);
}