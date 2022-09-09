using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cg_1.src
{
    public struct Point2D
    {
        short X { get; set; }
        short Y { get; set; }

        public Point2D(short x, short y) => (X, Y) = (x, y);
    }
}
