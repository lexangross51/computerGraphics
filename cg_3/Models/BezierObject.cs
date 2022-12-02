namespace cg_3.Models;

public class BezierObject
{
    private readonly Vector2D[] _controlPoints = new Vector2D[4];
    public Vector2D this[int idx] => _controlPoints[idx];

    public BezierObject(Vector2D p0, Vector2D p1, Vector2D p2, Vector2D p3)
    {
        _controlPoints[0] = p0;
        _controlPoints[1] = p1;
        _controlPoints[2] = p2;
        _controlPoints[3] = p3;
    }

    public Vector2D CurveGen(float t)
    {
        var oneMinusT = 1.0f - t;
        return oneMinusT * oneMinusT * oneMinusT * _controlPoints[0] +
               3.0f * oneMinusT * oneMinusT * t * _controlPoints[1] +
               3.0f * oneMinusT * t * t * _controlPoints[2] +
               t * t * t * _controlPoints[3];
    }
}    