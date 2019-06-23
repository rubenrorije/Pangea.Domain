using Pangea.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// Convenient extension methods to have similar functions for Percentage that are available for int, decimal, etc.
    /// </summary>
    public static class PercentageExtensions
    {

        /// <summary>
        /// Returns the maximum percentage from the given percentages
        /// </summary>
        /// <returns>The maximum percentage from the given percentages</returns>
        public static Percentage Max(this IEnumerable<Percentage> source)
        {
            return source.Max(prc => prc);
        }
        /// <summary>
        /// Returns the maximum percentage from the given source by selecting the right percentage using the selector function.
        /// </summary>
        /// <returns>The maximum percentage from the source items</returns>
        public static Percentage Max<TSource>(this IEnumerable<TSource> source, Func<TSource,Percentage> selector)
        {
            return source.Select(selector).OrderByDescending(percentage => percentage.Value).First();
        }


        /// <summary>
        /// Returns the minimum percentage from the given percentages
        /// </summary>
        /// <returns>The minimum percentage from the given percentages</returns>
        public static Percentage Min(this IEnumerable<Percentage> source)
        {
            return source.Min(prc => prc);
        }
        /// <summary>
        /// Returns the minimum percentage from the given source by selecting the right percentage using the selector function.
        /// </summary>
        /// <returns>The minimum percentage from the source items</returns>
        public static Percentage Min<TSource>(this IEnumerable<TSource> source, Func<TSource, Percentage> selector)
        {
            return source.Select(selector).OrderBy(percentage => percentage.Value).First();
        }

    }
}
