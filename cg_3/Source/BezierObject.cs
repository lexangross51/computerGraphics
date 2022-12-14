using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;

namespace cg_3.Source;

public class BezierObject : ICloneable
{
    private readonly List<Vector2> _controlPoints = new(4);
    
    public ImmutableArray<Vector2> ControlPoints => _controlPoints.ToImmutableArray();

    public Rectangle BoundingBox { get; private set; }

    public bool IsSmoothed { get; set; }
    public bool IsWasCalculatedBoundingBox { get; set; }

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
    
    public void CalculateBoundingBox() // TODO -> make it easier and avoid code repetition
    {
        (float? solX1, float? solX2) = SolveQuadratic(_controlPoints[0].X, _controlPoints[1].X, _controlPoints[2].X,
            _controlPoints[3].X);
        (float? solY1, float? solY2) = SolveQuadratic(_controlPoints[0].Y, _controlPoints[1].Y, _controlPoints[2].Y,
            _controlPoints[3].Y);

        var minX = Math.Min(_controlPoints[0].X, _controlPoints[3].X);
        var maxX = Math.Max(_controlPoints[0].X, _controlPoints[3].X);

        var minY = Math.Min(_controlPoints[0].Y, _controlPoints[3].Y);
        var maxY = Math.Max(_controlPoints[0].Y, _controlPoints[3].Y);

        if (solX1.HasValue)
        {
            minX = MathF.Min(minX, solX1.Value);
            maxX = MathF.Max(maxX, solX1.Value);
        }

        if (solX2.HasValue)
        {
            minX = MathF.Min(minX, solX2.Value);
            maxX = MathF.Max(maxX, solX2.Value);
        }

        if (solY1.HasValue)
        {
            minY = MathF.Min(minY, solY1.Value);
            maxY = MathF.Max(maxY, solY1.Value);
        }

        if (solY2.HasValue)
        {
            minY = MathF.Min(minY, solY2.Value);
            maxY = MathF.Max(maxY, solY2.Value);
        }

        BoundingBox = new(new(minX, minY), new(maxX, maxY));
    }

    private static (float? solution1, float? solution2) SolveQuadratic(float p0, float p1, float p2, float p3)
    {
        const float geometryEps = 1E-07f;

        float? solution1 = null;
        float? solution2 = null;

        float i = p1 - p0;
        float j = p2 - p1;
        float k = p3 - p2;

        float a = 3 * i - 6 * j + 3 * k;
        float b = 6 * j - 6 * i;
        float c = 3 * i;

        float sqrtPart = b * b - 4 * a * c;

        if (sqrtPart < 0) return (null, null);

        if (MathF.Abs(a) < geometryEps)
        {
            var t = -c / b;
            return t is >= 0 and <= 1 ? (GetBezierFloatValue(t, p0, p1, p2, p3), null) : (null, null);
        }

        float t1 = (-b + MathF.Sqrt(sqrtPart)) / (2 * a);
        float t2 = (-b - MathF.Sqrt(sqrtPart)) / (2 * a);

        if (t1 is >= 0 and <= 1) solution1 = GetBezierFloatValue(t1, p0, p1, p2, p3);
        if (t2 is >= 0 and <= 1) solution2 = GetBezierFloatValue(t2, p0, p1, p2, p3);

        return (solution1, solution2);
    }

    private static float GetBezierFloatValue(float t, float p0, float p1, float p2, float p3)
    {
        var oneMinusT = 1.0f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 +
               3.0f * oneMinusT * oneMinusT * t * p1 +
               3.0f * oneMinusT * t * t * p2 +
               t * t * t * p3;
    }
}    