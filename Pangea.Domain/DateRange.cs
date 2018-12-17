using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Pangea.Domain
{
    /// <summary>
    /// Represent a date range of two dates.
    /// This range can be bounded, by a start or end date, or indefinite.
    /// Can be used for instance to model functionality about having values that change over time in a LOB app
    /// </summary>
    [Serializable]
    public struct DateRange
        : IEquatable<DateRange>
        , IFormattable
        , IXmlSerializable
    {
        private readonly bool _isFilled;

        /// <summary>
        /// return whether the range is an empty range which does not contain any dates.
        /// </summary>
        public bool IsEmpty => !_isFilled;

        /// <summary>
        /// The beginning date of the range, when null the start is not bounded, which effectively means <see cref="DateTime.MinValue"/>
        /// </summary>
        public DateTime? Start { get; }

        private DateTime SafeStart => Start == null ? DateTime.MinValue : Start.Value;

        /// <summary>
        /// The end date of the range, when null the start is not bounded, which effectively means <see cref="DateTime.MaxValue"/>
        /// </summary>
        public DateTime? End { get; }

        private DateTime SafeEnd => End == null ? DateTime.MaxValue : End.Value;

        /// <summary>
        /// The Date range that represents an empty range. No dates are within this date range
        /// </summary>
        public static DateRange Never => new DateRange();

        /// <summary>
        /// The Date range that represents an unbounded range. all dates are within this date range
        /// </summary>
        public static DateRange Always => new DateRange(null, null);

        /// <summary>
        /// Create a Date range based on the start and end date. When the start or end date is null, this means that that part 
        /// of the range will be unbounded. Which effectively means MinValue / MaxValue
        /// </summary>
        public DateRange(DateTime? start, DateTime? end)
        {
            if (start != null && end != null && end < start) throw new ArgumentOutOfRangeException(nameof(end), "Cannot create a date range with a smaller end date than the start date");
            Start = start?.Date;
            End = end?.Date;
            _isFilled = true;
        }

        /// <summary>
        /// Create a DateRange with the given dates
        /// </summary>
        /// <param name="start">The start of the range</param>
        /// <param name="end">The end of the range</param>
        public DateRange(DateTime start, DateTime end)
        {
            if (end < start) throw new ArgumentOutOfRangeException(nameof(end), "Cannot create a date range with a smaller end date than the start date");
            Start = start.Date;
            End = end.Date;
            _isFilled = true;
        }


        /// <summary>
        /// Is the given date within the date range. Both dates are inclusive
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsInRange(DateTime date)
        {
            return
                _isFilled &&
                (Start == null || Start <= date.Date) &&
                (End == null || End >= date.Date);
        }

        /// <summary>
        /// Create a date range that spans the whole year for the given date
        /// </summary>
        public static DateRange Year(DateTime date)
        {
            return Year(date.Year);
        }

        /// <summary>
        /// Create a date range that spans the whole given year
        /// </summary>
        public static DateRange Year(int year)
        {
            return Years(year, 1);
        }

        /// <summary>
        /// Create a date range starting at January 1 of the given year, until december 31st of the last year.
        /// </summary>
        /// <param name="startingYear">The year to start at January 1</param>
        /// <param name="numberOfYears">The number of years, the minimum value is 1</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startingYear"/> is non-positive or <paramref name="numberOfYears"/> is non-positive</exception>
        public static DateRange Years(int startingYear, int numberOfYears)
        {
            return new DateRange(new DateTime(startingYear, 1, 1), new DateTime(startingYear + numberOfYears - 1, 12, 31));
        }

        /// <summary>
        /// Create a range that only spans today
        /// </summary>
        /// <returns>The range that starts and ends with today</returns>
        public static DateRange Today()
        {
            return Day(DateTime.Today);
        }

        /// <summary>
        /// Create a range that only spans the given day
        /// </summary>
        /// <returns>The range that starts and ends with the given day</returns>
        public static DateRange Day(DateTime date)
        {
            return Days(date, 1);
        }

        /// <summary>
        /// Create a range that only spans the start date and the number of days after the start date
        /// </summary>
        /// <returns>The range that starts with the given day and ends with the number of days</returns>
        public static DateRange Days(DateTime start, int numberOfDays)
        {
            return new DateRange(start, start.AddDays(numberOfDays - 1));
        }

        /// <summary>
        /// Combines two date ranges if the two ranges are adjacent. Otherwise an exception will be thrown.
        /// </summary>
        public static DateRange operator +(DateRange lhs, DateRange rhs)
        {
            return lhs.Add(rhs);
        }

        /// <summary>
        /// Combines two date ranges if the two ranges are adjacent. Otherwise an exception will be thrown.
        /// </summary>
        public DateRange Add(DateRange other)
        {
            if (Start != null && other.End != null && Start.Value == other.End.Value.AddDays(1))
            {
                return new DateRange(other.Start, End);
            }
            else if (other.Start != null && End != null && other.Start.Value == End.Value.AddDays(1))
            {
                return new DateRange(Start, other.End);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(other), "Cannot combine the ranges because the ranges are not adjacent");
            }
        }

        /// <summary>
        /// Subtract the right hand side from the left hand side. This only works when the ranges have the same end date.
        /// Otherwise an exception will be thrown.
        /// </summary>
        public static DateRange operator -(DateRange lhs, DateRange rhs)
        {
            return lhs.Subtract(rhs);
        }

        /// <summary>
        /// Subtract the right hand side from the left hand side. This only works when the ranges have the same end date.
        /// Otherwise an exception will be thrown.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The range could not be subtracted, because the two ranges do not have a common start or end date</exception>
        public DateRange Subtract(DateRange other)
        {
            if (Equals(other)) return new DateRange();
            if (End == other.End) return new DateRange(Start, other.Start.Value.AddDays(-1));
            if (Start == other.Start) return new DateRange(other.End.Value.AddDays(1), End);

            throw new ArgumentOutOfRangeException(nameof(other), "Cannot subtract the ranges, because the end dates do not match");
        }

        /// <summary>
        /// Returns whether the two ranges are adjacent to one and other. 
        /// The order in which the ranges are specified does not matter.
        /// </summary>
        /// <param name="other">One of the ranges to compare.</param>
        /// <returns>True when the ranges are next to eachother</returns>
        public bool IsAdjacentTo(DateRange other)
        {
            if (!_isFilled) return false;
            if (!other._isFilled) return false;

            if (Start != null && other.End != null && Start.Value == other.End.Value.AddDays(1)) return true;
            if (other.Start != null && End != null && other.Start.Value == End.Value.AddDays(1)) return true;

            return false;
        }

        /// <inheritdoc/>
        public static bool operator ==(DateRange lhs, DateRange rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <inheritdoc/>
        public static bool operator !=(DateRange lhs, DateRange rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + _isFilled.GetHashCode();
                hash = hash * 23 + Start?.GetHashCode() ?? 0;
                hash = hash * 23 + End?.GetHashCode() ?? 0;
                return hash;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is DateRange && Equals((DateRange)obj);
        }

        /// <inheritdoc/>
        public bool Equals(DateRange other)
        {
            return
                _isFilled == other._isFilled &&
                Start == other.Start &&
                End == other.End;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <inheritdoc/>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var safeFormat = format ?? "G";

            if (safeFormat == "G") safeFormat = "d";

            if (!_isFilled) return "Never";
            if (Start == null && End == null) return "Always";
            if (Start == null) return "≤ " + End.Value.ToString(safeFormat, formatProvider);
            if (End == null) return "≥ " + Start.Value.ToString(safeFormat, formatProvider);

            return
                Start.Value.ToString(safeFormat, formatProvider) +
                " - " +
                End.Value.ToString(safeFormat, formatProvider);
        }

        /// <inheritdoc/>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <inheritdoc/>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var startValue = reader.MoveToAttribute("start") ? reader.Value : null;
            var endValue = reader.MoveToAttribute("end") ? reader.Value : null;
            var filledValue = reader.MoveToAttribute("isFilled") ? reader.Value : null;

            var start = string.IsNullOrEmpty(startValue) ? (DateTime?)null : DateTime.ParseExact(startValue, "o", CultureInfo.InvariantCulture);
            var end = string.IsNullOrEmpty(endValue) ? (DateTime?)null : DateTime.ParseExact(endValue, "o", CultureInfo.InvariantCulture);
            if (!bool.TryParse(filledValue, out var filled)) filled = false;
            if (!filled)
            {
                Unsafe.AsRef(this) = default;
            }
            else
            {
                Unsafe.AsRef(this) = new DateRange(start, end);
            }
            reader.Skip();
        }

        /// <inheritdoc/>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            var startValue = Start?.ToString("o", CultureInfo.InvariantCulture);
            var endValue = End?.ToString("o", CultureInfo.InvariantCulture);

            if (startValue != null) writer.WriteAttributeString("start", startValue);
            if (endValue != null) writer.WriteAttributeString("end", endValue);
            writer.WriteAttributeString("isFilled", _isFilled.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Returns whether the two date ranges are overlapping
        /// </summary>
        public bool OverlapsWith(DateRange other)
        {
            if (IsEmpty) return false;
            if (other.IsEmpty) return false;

            if (SafeStart <= other.SafeStart && SafeEnd >= other.SafeEnd) return true; // surrounded (1)
            if (other.SafeStart <= SafeStart && other.SafeEnd >= SafeEnd) return true; // surrounded (2)
            if (SafeStart <= other.SafeStart && SafeEnd >= other.SafeStart) return true; // other.SafeStart within this.Range
            if (other.SafeStart <= SafeStart && other.SafeEnd >= SafeStart) return true; // SafeStart within other.Range

            return false;
        }

        /// <summary>
        /// Deconstruct the date range into (start, end)
        /// </summary>
        public ValueTuple<DateTime?, DateTime?> Deconstruct()
        {
            return (Start, End);
        }
    }
}
