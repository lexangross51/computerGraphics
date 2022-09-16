using System;
using System.Drawing;
using System.Collections.Generic;

namespace ComputerGraphics.Source
{
    public class StripLine : ICloneable
    {
        public List<Point2D> Points { get; private set; }
        public Color Color { get; set; }
        public float Thickness { get; set; }
        public ushort Stipple { get; set; }

        public StripLine()
        {
            Points = new List<Point2D>();
            Color = new Color();
            Thickness = 1.0f;
            Stipple = 0xFFFF;
        }

        public object Clone() => new StripLine
        {
            Points = new List<Point2D>(Points),
            Color = Color,
            Thickness = Thickness,
            Stipple = Stipple
        };

        public void Reset()
        {
            Points.Clear();
            Color = new Color();
            Thickness = 1.0f;
            Stipple = 0xFFFF;
        }
    }
}