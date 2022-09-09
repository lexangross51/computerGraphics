using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;

namespace cg_1.src
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
        byte R { get; set; }
        byte G { get; set; }
        byte B { get; set; }

        public Color(byte r = 0, byte g = 0, byte b = 0) 
            => (R, G, B) = (r, g, b);
    }
}
