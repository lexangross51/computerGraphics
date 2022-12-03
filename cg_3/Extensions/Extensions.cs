namespace cg_3.Extensions;

public static class Extensions
{
    public static Vector2D ToScreenCoordinates(this MouseEventArgs mouseEventArgs, GLWpfControl control)
    {
        var point = mouseEventArgs.GetPosition(control);

        var x = (float)(-1.0f + 2 * point.X / control.RenderSize.Width);
        var y = (float)(1.0f - 2 * point.Y / control.RenderSize.Height);

        return (x, y);
    }
}