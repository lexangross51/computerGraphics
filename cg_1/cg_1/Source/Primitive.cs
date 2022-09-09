using System.Collections.Generic;

namespace ComputerGraphics.Source
{
    public class StripLine
    {
        public List<Point2D> Points { get; set; }
        public Color Color { get; set; }

        public StripLine()
        {
            Points = new List<Point2D>();
            Color = new Color();
        }
    }

    public struct Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public Color(byte r = 0, byte g = 0, byte b = 0) 
            => (R, G, B) = (r, g, b);
    }
}
