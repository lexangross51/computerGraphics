namespace cg_3.Models;

public class Plane : ReactiveObject
{
    public SourceCache<BezierObject, BezierObject> SelectedSegments { get; }
    public SourceList<Vector2D> SelectedPoints { get; }
    [Reactive] public BezierObject? SelectedSegment { get; set; }

    public Plane()
    {
        SelectedSegments = new(obj => obj);
        SelectedPoints = new(); // p0, p1, p2, p3
    }
}