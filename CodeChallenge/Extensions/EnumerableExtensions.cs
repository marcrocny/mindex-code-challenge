using System.Collections.Generic;
using System.Linq;

namespace CodeChallenge.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Left-edge null-enumerable protection.
        /// </summary>
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> source) => source ?? Enumerable.Empty<T>();
    }
}
