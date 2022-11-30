using System;
using System.Numerics;

namespace cg_3.Source;

public struct Projection
{
    public float Left { get; set; }
    public float Right { get; set; }
    public float Bottom { get; set; }
    public float Top { get; set; }

    public Projection(float left, float right, float bottom, float top)
        => (Left, Right, Bottom, Top) = (left, right, bottom, top);
    
    public float Width => Right - Left;
    public float Height => Top - Bottom;

    public Vector2 ToScreenCoordinate(float x, float y)
        => new Vector2(Left + Width * x, Top - Height * y);
}