using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace cg_3.Source;

public static class EnumerableExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> collection) => !collection.Any();
}