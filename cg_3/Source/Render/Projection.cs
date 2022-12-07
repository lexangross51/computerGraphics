namespace cg_3.Source.Render;

public class Projection
{
    public float Left { get; set; }
    public float Right { get; set; }
    public float Bottom { get; set; }
    public float Top { get; set; }
    public float Width => Right - Left;
    public float Height => Top - Bottom;

    public Projection(float left, float right, float bottom, float top)
    {
        Left = left;
        Right = right;
        Bottom = bottom;
        Top = top;
    }
}