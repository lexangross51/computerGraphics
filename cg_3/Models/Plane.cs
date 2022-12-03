namespace cg_3.Models;

public class Plane
{
    public SourceCache<BezierObject, BezierObject> SelectedSegments { get; }
    public SourceList<Vector2D> SelectedPoints { get; }

    public Plane()
    {
        SelectedSegments = new(obj => obj);
        SelectedPoints = new(); // p0, p1, p2, p3
        SelectedPoints.CountChanged.Where(t => t == 3).Subscribe(_ => SelectedPoints.Clear());
    }
}