using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Source
{
    public static class ListExtensinos
    {
        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count == 0;
        }
    }
}
