using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Fluent
{
    /// <summary>
    /// Fluent interface to create DateRanges easily
    /// </summary>
    public static class DateRangeExtensions
    {
        /// <summary>
        /// Create a daterange from the given dates
        /// </summary>
        public static DateRange Until(this DateTime start, int endYear, int endMonth, int endDay)
        {
            return start.Until(new DateTime(endYear, endMonth, endDay));
        }

        /// <summary>
        /// Create a daterange from the given dates
        /// </summary>
        public static DateRange Until(this DateTime start, DateTime end)
        {
            return new DateRange(start, end);
        }

        /// <summary>
        /// Create a new DateRange starting at the given date, going on forever
        /// </summary>
        public static DateRange UntilForever(this DateTime start)
        {
            return new DateRange(start, null);
        }

    }
}
