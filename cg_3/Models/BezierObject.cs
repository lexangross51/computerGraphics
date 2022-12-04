namespace cg_3.Models;

public sealed class BezierObject
{
    private readonly Vector2D[] _controlPoints = new Vector2D[4];
    public List<Vector2D> CompletedPoints { get; }

    public Vector2D this[int idx]
    {
        get => _controlPoints[idx];
        set => _controlPoints[idx] = value;
    }

    public BezierObject(Vector2D p0, Vector2D p1, Vector2D p2, Vector2D p3)
    {
        _controlPoints[0] = p0;
        _controlPoints[1] = p1;
        _controlPoints[2] = p2;
        _controlPoints[3] = p3;
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
        return oneMinusT * oneMinusT * oneMinusT * _controlPoints[0] +
               3.0f * oneMinusT * oneMinusT * t * _controlPoints[1] +
               3.0f * oneMinusT * t * t * _controlPoints[2] +
               t * t * t * _controlPoints[3];
    }
}