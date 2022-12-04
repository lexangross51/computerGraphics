namespace cg_3.Models;

public sealed class BezierObject
{
    public Vector2D[] ControlPoints { get; } = new Vector2D[4];
    public List<Vector2D> CompletedPoints { get; }

    public Vector2D this[int idx]
    {
        get => ControlPoints[idx];
        set => ControlPoints[idx] = value;
    }

    public BezierObject(Vector2D p0, Vector2D p1, Vector2D p2, Vector2D p3)
    {
        ControlPoints[0] = p0;
        ControlPoints[1] = p1;
        ControlPoints[2] = p2;
        ControlPoints[3] = p3;
        CompletedPoints = new();
    }

    public void GenCurve()
    {
        for (int i = 0; i <= 20; i++)
        {
            var t = i / 20.0f;
            CompletedPoints.Add(GetPoint(t));
        }
    }

    private Vector2D GetPoint(float t)
    {
        var oneMinusT = 1.0f - t;
        return oneMinusT * oneMinusT * oneMinusT * ControlPoints[0] +
               3.0f * oneMinusT * oneMinusT * t * ControlPoints[1] +
               3.0f * oneMinusT * t * t * ControlPoints[2] +
               t * t * t * ControlPoints[3];
    }
}