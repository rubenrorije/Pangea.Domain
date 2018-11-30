using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.ExtensionMethods
{
    /// <summary>
    /// Convenient methods to have for enumerables
    /// </summary>
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Create an enumerable of enumerables that have a specific size. That is, it will create 'pages' of size pageSize
        /// where the last one can be less than the pageSize but at least has one item.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="items">the enumerable</param>
        /// <param name="pageSize">the size of each page</param>
        /// <returns>An enumerable containing all pages</returns>
        public static IEnumerable<IEnumerable<T>> Page<T>(this IEnumerable<T> items, int pageSize)
        {
            List<T> group = new List<T>();

            var index = 0;

            foreach (var item in items)
            {
                if (index == pageSize)
                {
                    yield return group;
                    index = 0;
                    group = new List<T>(pageSize);
                }
                group.Add(item);
                index++;
            }

            if (group.Count != 0)
            {
                yield return group;
            }
        }
    }
}
