using cg_3.Source.Vectors;
using DynamicData;

namespace cg_3.Models;

public class Plane
{
    public SourceList<Vector2D> SelectedPoints { get; } = new();
    public List<Vector2D> SelectedSegment { get; } = new();

    public void ClearSelected()
    {
        SelectedPoints.Clear();
        SelectedSegment.Clear();
    }
}