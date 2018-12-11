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

        /// <summary>
        /// The end date of the range, when null the start is not bounded, which effectively means <see cref="DateTime.MaxValue"/>
        /// </summary>
        public DateTime? End { get; }

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
            if (start != null && end != null && end < start) throw new ArgumentOutOfRangeException("Cannot create a date range with a smaller end date than the start date");
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
            if (end < start) throw new ArgumentOutOfRangeException("Cannot create a date range with a smaller end date than the start date");
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
            if (lhs.Start != null && rhs.End != null && lhs.Start.Value == rhs.End.Value.AddDays(1))
            {
                return new DateRange(rhs.Start, lhs.End);
            }
            else if (rhs.Start != null && lhs.End != null && rhs.Start.Value == lhs.End.Value.AddDays(1))
            {
                return new DateRange(lhs.Start, rhs.End);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Cannot combine the ranges because the ranges are not adjacent");
            }
        }

        /// <summary>
        /// Subtract the right hand side from the left hand side. This only works when the ranges have the same end date.
        /// Otherwise an exception will be thrown.
        /// </summary>
        public static DateRange operator -(DateRange lhs, DateRange rhs)
        {
            if (lhs.Equals(rhs)) return new DateRange();
            if (lhs.End == rhs.End) return new DateRange(lhs.Start, rhs.Start.Value.AddDays(-1));
            if (lhs.Start == rhs.Start) return new DateRange(rhs.End.Value.AddDays(1), lhs.End);

            throw new ArgumentOutOfRangeException("Cannot subtract the ranges, because the end dates do not match");
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
            format = format ?? "G";

            if (format == "G") format = "d";

            if (!_isFilled) return "Never";
            if (Start == null && End == null) return "Always";
            if (Start == null) return "≤ " + End.Value.ToString(format, formatProvider);
            if (End == null) return "≥ " + Start.Value.ToString(format, formatProvider);

            return
                Start.Value.ToString(format, formatProvider) +
                " - " +
                End.Value.ToString(format, formatProvider);
        }

        /// <inheritdoc/>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc/>
        public void ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException();

            reader.MoveToContent();

            var value = reader.ReadElementContentAsString();
            var splitted = value.Split("|".ToCharArray());
            var start = ParseXmlValue(splitted[0]);
            var end = ParseXmlValue(splitted[1]);

            Unsafe.AsRef(this) = new DateRange(start, end);
        }

        private static DateTime? ParseXmlValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                return DateTime.ParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }
        }

        /// <inheritdoc/>
        public void WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException();

            var startValue = Start?.ToString("o", CultureInfo.InvariantCulture) ?? string.Empty;
            var endValue = End?.ToString("o", CultureInfo.InvariantCulture) ?? string.Empty;
            writer.WriteElementString("value", $"{startValue}|{endValue}");
        }
    }
}
