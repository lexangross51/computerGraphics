﻿namespace ComputerGraphics
{
    public struct Point2D
    {
        public short X { get; set; }
        public short Y { get; set; }

        public Point2D(short x, short y) => (X, Y) = (x, y);
    }
}