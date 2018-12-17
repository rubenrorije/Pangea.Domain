using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Compare two dates to have a definitive ordering, by ordering first by start and then by end. 
    /// </summary>
    public class DateRangeComparer : IComparer<DateRange>
    {
        /// <summary>
        /// Returns 0 when the dates are equal, otherwise compare first the start dates and then the end dates
        /// </summary>
        public int Compare(DateRange x, DateRange y)
        {
            var result = Nullable.Compare(x.Start, y.Start);
            if (result != 0) return result;

            return Nullable.Compare(x.End, y.End);
        }
    }
}
