using cg_3.Models;
using cg_3.Source.Vectors;
using OpenTK.Mathematics;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_3.ViewModels;

public class BezierWrapper : ReactiveObject
{
    public Guid Guid { get; }
    public List<Vector2D> Points { get; }
    public Vector2D[] ControlPoints { get; }
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
        Points = new();
        ControlPoints = new[] { p0, p1, p2, p3 };

        this.WhenAnyValue(t => t.P0, t => t.P1, t => t.P2, t => t.P3)
            .Subscribe(points =>
            {
                ControlPoints[0] = points.Item1;
                ControlPoints[1] = points.Item2;
                ControlPoints[2] = points.Item3;
                ControlPoints[3] = points.Item4;
            });
    }

    public Vector2D GenCurve(float t) => AsBezierObject().CurveGen(t);

    public BezierObject AsBezierObject() => new(P0, P1, P2, P3);
}