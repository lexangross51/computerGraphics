using System;
using System.Numerics;
using SharpGL.RenderContextProviders;

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

    public Vector2 ToProjectionCoordinate(float x, float y, IRenderContextProvider contextProvider)
        => new(Left + Width * x / contextProvider.Width, Top - Height * y / contextProvider.Height);

    public Vector2 ToScreenCoordinates(float x, float y, IRenderContextProvider contextProvider)
        => new((x - Left) * contextProvider.Width / Width, (y - Bottom) * contextProvider.Height / Height);
}