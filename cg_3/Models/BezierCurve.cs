using cg_3.Source.Vectors;

namespace cg_3.Models;

public class CubicBezier
{
    private readonly Vector2D[] _controlPoints = new Vector2D[4];
    private Vector2D[]? _points;
    public Vector2D this[int idx] => _controlPoints[idx];
    public IEnumerable<Vector2D>? Points => _points;

    public CubicBezier(Vector2D p0, Vector2D p1, Vector2D p2, Vector2D p3)
    {
        _controlPoints[0] = p0;
        _controlPoints[1] = p1;
        _controlPoints[2] = p2;
        _controlPoints[3] = p3;
    }

    public Vector2D GetPoint(float t)
    {
        var oneMinusT = 1.0f - t;
        return oneMinusT * oneMinusT * oneMinusT * _controlPoints[0] +
               3.0f * oneMinusT * oneMinusT * t * _controlPoints[1] +
               3.0f * oneMinusT * t * t * _controlPoints[2] +
               t * t * t * _controlPoints[3];
    }
    
    public void CurveGen(in int segments)
    {
        if (segments <= 0) throw new ArgumentException("More segments need to be set", nameof(segments));

        _points = new Vector2D[segments + 1];
        _points[0] = _controlPoints[0];
        _points[^1] = _controlPoints[3];

        for (int i = 1; i < segments; i++)
        {
            _points[i] = GetPoint((float)i / segments);
        }
    }
}