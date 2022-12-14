using System.Numerics;

namespace cg_3.Source;

public readonly record struct Rectangle(Vector2 LeftBottom, Vector2 RightTop)
{
    public Vector2 RightBottom { get; } = new(RightTop.X, LeftBottom.Y);
    public Vector2 LeftTop { get; } = new(LeftBottom.X, RightTop.Y);
}