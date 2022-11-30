using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using GlmNet;

namespace cg_3.Source;

public class BezierObject : ICloneable
{
    private readonly List<Vector2> _controlPoints = new(4);

    public ImmutableArray<Vector2> ControlPoints => _controlPoints.ToImmutableArray();

    public Vector2 this[int idx] => _controlPoints[idx];

    public void AddControlPoint(Vector2 point) => _controlPoints.Add(point);

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

    public void Scale(Vector2 pivot, float scale)
    {
        float xStep = pivot.X * scale - pivot.X;
        float yStep = pivot.Y * scale - pivot.Y;
        
        for (int i = 0; i < _controlPoints.Count; i++)
        {
            var x = _controlPoints[i].X * scale - xStep;
            var y = _controlPoints[i].Y * scale - yStep;

            _controlPoints[i] = new Vector2(x, y);
        }
    }

    public object Clone() => MemberwiseClone();
}    