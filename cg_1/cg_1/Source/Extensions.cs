using System.Collections.Generic;
using System.Linq;

namespace ComputerGraphics.Source
{
    public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> collection) => !collection.Any();
    }
}