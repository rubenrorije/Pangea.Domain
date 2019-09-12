using Pangea.Domain.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Create a collection of date ranges
    /// </summary>
    public class DateRangeCollection
        : IEnumerable<DateRange>
    {
        private List<DateRange> _ranges;

        /// <summary>
        /// Create a collection with the given date ranges
        /// </summary>
        /// <param name="ranges"></param>
        public DateRangeCollection(params DateRange[] ranges)
        {
            _ranges = ranges.ToList();
        }

        /// <summary>
        /// The number of date ranges in this collection
        /// </summary>
        public int Count => _ranges.Count;

        /// <inheritdoc/>
        public void Add(DateRange item) => _ranges.Add(item);
        /// <inheritdoc/>
        public void Clear() => _ranges.Clear();

        /// <inheritdoc />
        public IEnumerator<DateRange> GetEnumerator() => _ranges.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Check whether there is one of the ranges for which the date is in the range
        /// </summary>
        /// <param name="date">the date to check</param>
        public bool IsInRange(DateTime date)
        {
            return InRangeIndexed(date) >= 0;
        }

        /// <summary>
        /// Return the index of the first range that contains the date. When no range contains the given date, -1 will be returned
        /// </summary>
        public int InRangeIndexed(DateTime date)
        {
            for (var index = 0; index < _ranges.Count; index++)
            {
                var range = _ranges[index];
                if (range.IsInRange(date)) return index;
            }
            return -1;
        }

        /// <summary>
        /// Checks whether the ranges are overlapping
        /// </summary>
        public bool IsOverlapping()
        {
            var ordered = _ranges.OrderBy(rng => rng, new DateRangeComparer()).ToList();
            for (var index = 0; index < ordered.Count - 1; index++)
            {
                var current = ordered[index];
                var next = ordered[index + 1];

                if (current.OverlapsWith(next)) return true;
            }
            return false;
        }

        /// <summary>
        /// Return a new instance where the ranges are ordered sequentially
        /// </summary>
        /// <returns></returns>
        public DateRangeCollection Ordered()
        {
            return new DateRangeCollection(_ranges.OrderBy(rng => rng, new DateRangeComparer()).ToArray());
        }

        /// <summary>
        /// Get the date range element by index
        /// </summary>
        public DateRange this[int index] => _ranges[index];

        /// <summary>
        /// Returns whether all the date ranges in this collection are adjacent, not necessarily in the right order,
        /// when there is any overlap, the result will be false.
        /// </summary>
        public bool IsSingleChain()
        {
            if (Count <= 1) throw new InvalidOperationException(Resources.DateRangeCollection_NoRanges);

            var ordered = _ranges.OrderBy(rng => rng, new DateRangeComparer()).ToList();
            for (var index = 0; index < ordered.Count - 1; index++)
            {
                var current = ordered[index];
                var next = ordered[index + 1];

                if (!current.IsAdjacentTo(next)) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the date range representing the combined ranges. When the ranges where not adjacent, the result will be <c>Never</c>
        /// </summary>
        /// <returns></returns>
        public DateRange GetSingleChain()
        {
            if (Count <= 1) return DateRange.Never;

            var ordered = _ranges.OrderBy(rng => rng, new DateRangeComparer()).ToList();

            for (var index = 0; index < ordered.Count - 1; index++)
            {
                var current = ordered[index];
                var next = ordered[index + 1];

                if (!current.IsAdjacentTo(next)) return DateRange.Never;
            }

            return new DateRange(ordered[0].Start, ordered.Last().End);

        }
    }
}
