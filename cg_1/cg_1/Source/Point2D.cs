using System;

namespace ComputerGraphics.Source
{
    public struct Point2D
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point2D(float x, float y) => (X, Y) = (x, y);

        public static Point2D operator +(Point2D first, Point2D second) =>
            new Point2D(first.X + second.X, first.Y + second.Y);

        public static Point2D operator -(Point2D first, Point2D second) =>
            new Point2D(first.X - second.X, first.Y - second.Y);
    }
}
