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
            return _ranges.Any(range => range.IsInRange(date));
        }

        /// <summary>
        /// Checks whether the ranges are overlapping
        /// </summary>
        public bool IsOverlapping()
        {
            return false;
        }
        
    }
}
