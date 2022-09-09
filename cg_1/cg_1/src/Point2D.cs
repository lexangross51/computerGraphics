using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cg_1.src
{
    public struct Point2D
    {
        public short X { get; set; }
        public short Y { get; set; }

        public Point2D(short x, short y) => (X, Y) = (x, y);
    }
}
