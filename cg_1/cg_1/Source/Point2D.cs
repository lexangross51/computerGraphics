namespace ComputerGraphics.Source
{
    public struct Point2D
    {
        public short X { get; }
        public short Y { get; }

        public Point2D(short x, short y) => (X, Y) = (x, y);
    }
}
