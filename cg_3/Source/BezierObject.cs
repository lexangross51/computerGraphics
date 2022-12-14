using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;

namespace cg_3.Source;

public class BezierObject : ICloneable
{
    private readonly List<Vector2> _controlPoints = new(4);
    
    public ImmutableArray<Vector2> ControlPoints => _controlPoints.ToImmutableArray();

    public bool IsSmoothed { get; set; } = false;

    public BezierObject() { }

    public BezierObject(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, bool isSmoothed = false)
    {
        _controlPoints.Add(p0);
        _controlPoints.Add(p1);
        _controlPoints.Add(p2);
        _controlPoints.Add(p3);
        IsSmoothed = isSmoothed;
    }

    public void AddControlPoint(Vector2 point) => _controlPoints.Add(point);
    public void AddControlPoint(int idx, Vector2 point) => _controlPoints.Insert(idx, point);

    public void DeleteControlPoint(int idx) => _controlPoints.RemoveAt(idx);
    
    public void UpdateControlPoint(int pointIdx, Vector2 newPoint)
        => _controlPoints[pointIdx] = newPoint;

    public Vector2 CurveGen(float t)
    {
        var oneMinusT = 1.0f - t;

        return _controlPoints.Count switch
        {
            2 => oneMinusT * _controlPoints[0] + t * _controlPoints[1],
            3 => oneMinusT * oneMinusT * _controlPoints[0] +
                 2 * t * oneMinusT * _controlPoints[1] + t * t * _controlPoints[2],
            4 => oneMinusT * oneMinusT * oneMinusT * _controlPoints[0] +
                 3.0f * oneMinusT * oneMinusT * t * _controlPoints[1] +
                 3.0f * oneMinusT * t * t * _controlPoints[2] +
                 t * t * t * _controlPoints[3],
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public object Clone() => MemberwiseClone();
}    