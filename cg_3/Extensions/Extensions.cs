namespace cg_3.Extensions;

public static class Extensions
{
    public static Vector2D ToScreenCoordinates(this MouseEventArgs mouseEventArgs, GLWpfControl control, Projection projection)
    {
        var point = mouseEventArgs.GetPosition(control);

        var x = (float)(projection.Left + projection.Width * point.X / control.RenderSize.Width);
        var y = (float)(projection.Top - projection.Height * point.Y / control.RenderSize.Height);

        return (x, y);
    }
}