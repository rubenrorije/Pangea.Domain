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
    /// Representing a percentage, can be used for easy calculations with percentages.
    /// </summary>
    public struct Percentage :
        IFormattable,
        IEquatable<Percentage>,
        IComparable<int>,
        IComparable<decimal>,
        IComparable<double>,
        IComparable,
        IComparable<Percentage>,
        IXmlSerializable
    {
        private readonly decimal _value;

        /// <summary>
        /// The actual percentage value
        /// </summary>
        public decimal Value => _value;

        /// <summary>
        /// The percentage value, represented as a fraction
        /// </summary>
        public decimal Fraction => _value / 100m;

        /// <summary>
        /// The default 100%
        /// </summary>
        public static readonly Percentage Hundred = new Percentage(100);

        /// <summary>
        /// Create a percentage from the given decimal percentage.
        /// Assumes the percentage given is the actual percentage, not the fraction
        /// </summary>
        /// <param name="percentage">The percentage, not the fraction</param>
        public Percentage(decimal percentage)
        {
            if (percentage < 0) throw new ArgumentOutOfRangeException(nameof(percentage));
            _value = percentage;
        }

        /// <summary>
        /// Create a percentage from the given double percentage.
        /// Assumes the percentage given is the actual percentage, not the fraction
        /// </summary>
        /// <param name="percentage">The percentage, not the fraction</param>
        public Percentage(double percentage)
        {
            if (percentage < 0) throw new ArgumentOutOfRangeException(nameof(percentage));
            _value = Convert.ToDecimal(percentage);
        }

        /// <summary>
        /// Create a percentage from the given decimal percentage.
        /// Assumes the percentage given is the actual percentage, not the fraction
        /// </summary>
        /// <param name="percentage">The percentage, not the fraction</param>
        public Percentage(int percentage)
        {
            if (percentage < 0) throw new ArgumentOutOfRangeException(nameof(percentage));
            _value = percentage;
        }

        /// <summary>
        /// Create a percentage based on the given fraction. E.g. to construct 5% the fraction must be 0.05
        /// </summary>
        /// <param name="fraction">The fraction that represents the percentage</param>
        /// <returns>The percentage</returns>
        public static Percentage FromFraction(double fraction)
        {
            return new Percentage(fraction * 100);
        }

        /// <summary>
        /// Create a percentage based on the given fraction. E.g. to construct 5% the fraction must be 0.05
        /// </summary>
        /// <param name="fraction">The fraction that represents the percentage</param>
        /// <returns>The percentage</returns>
        public static Percentage FromFraction(decimal fraction)
        {
            return new Percentage(fraction * 100);
        }

        /// <summary>
        /// Return the string representation of this Percentage.
        /// Shorthand for calling ToString(null) or ToString("G");
        /// </summary>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Return the string representation of this percentage. The given format and formatProvider
        /// will be used to format the number.
        /// </summary>
        /// <param name="format">The format to use for the number</param>
        /// <param name="formatProvider">The formatprovider that is used to represent the number</param>
        /// <returns>The string representation of the percentage</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider) + " %";
        }

        /// <summary>
        /// Return the string representation of this percentage. The given format and formatProvider
        /// will be used to format the number.
        /// </summary>
        /// <param name="format">The format to use for the number</param>
        /// <returns>The string representation of the percentage</returns>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Create a percentage value from the given nominator and denominator.
        /// This makes calculating for instance the current percentage completed of a given list
        /// by calling Percentage.From(currentIndex, list.Count)
        /// </summary>
        /// <param name="nominator">the current index</param>
        /// <param name="denominator">the total count</param>
        /// <returns></returns>
        public static Percentage From(int nominator, int denominator)
        {
            if (nominator <= 0) throw new ArgumentOutOfRangeException(nameof(nominator));
            if (denominator <= 0) throw new ArgumentOutOfRangeException(nameof(denominator));
            return FromFraction(nominator / (denominator * 1m));
        }

        /// <inheritdoc />
        public static bool operator ==(Percentage lhs, Percentage rhs)
        {
            return lhs._value == rhs._value;
        }

        /// <inheritdoc />
        public static bool operator !=(Percentage lhs, Percentage rhs)
        {
            return !(lhs == rhs);
        }

        /// <inheritdoc />
        public static bool operator ==(Percentage lhs, int rhs)
        {
            return lhs._value == rhs;
        }

        /// <inheritdoc />
        public static bool operator !=(Percentage lhs, int rhs)
        {
            return lhs._value != rhs;
        }

        /// <inheritdoc />
        public static bool operator ==(int lhs, Percentage rhs)
        {
            return lhs == rhs._value;
        }

        /// <inheritdoc />
        public static bool operator !=(int lhs, Percentage rhs)
        {
            return lhs != rhs._value;
        }
        /// <inheritdoc />
        public static bool operator ==(Percentage lhs, decimal rhs)
        {
            return lhs._value == rhs;
        }

        /// <inheritdoc />
        public static bool operator !=(Percentage lhs, decimal rhs)
        {
            return lhs._value != rhs;
        }

        /// <inheritdoc />
        public static bool operator ==(decimal lhs, Percentage rhs)
        {
            return lhs == rhs._value;
        }

        /// <inheritdoc />
        public static bool operator !=(decimal lhs, Percentage rhs)
        {
            return lhs != rhs._value;
        }
        /// <inheritdoc />
        public static bool operator ==(Percentage lhs, double rhs)
        {
            return lhs._value == Convert.ToDecimal(rhs);
        }

        /// <inheritdoc />
        public static bool operator !=(Percentage lhs, double rhs)
        {
            return lhs._value != Convert.ToDecimal(rhs);
        }

        /// <inheritdoc />
        public static bool operator ==(double lhs, Percentage rhs)
        {
            return Convert.ToDecimal(lhs) == rhs._value;
        }

        /// <inheritdoc />
        public static bool operator !=(double lhs, Percentage rhs)
        {
            return Convert.ToDecimal(lhs) != rhs._value;
        }

        /// <summary>
        /// Add the percentage to the given value. E.g. 1 + 10% = 1.1
        /// </summary>
        /// <param name="value">the number to add the percentage to</param>
        /// <param name="percentage">The percentage to add</param>
        /// <returns>The result</returns>
        public static decimal operator +(int value, Percentage percentage)
        {
            return value * (1 + percentage.Fraction);
        }

        /// <summary>
        /// Add the percentage to the given value. E.g. 1 + 10% = 1.1
        /// </summary>
        /// <param name="value">the number to add the percentage to</param>
        /// <param name="percentage">The percentage to add</param>
        /// <returns>The result</returns>
        public static decimal operator +(decimal value, Percentage percentage)
        {
            return value * (1 + percentage.Fraction);
        }

        /// <summary>
        /// Add the percentage to the given value. E.g. 1 + 10% = 1.1
        /// </summary>
        /// <param name="value">the number to add the percentage to</param>
        /// <param name="percentage">The percentage to add</param>
        /// <returns>The result</returns>
        public static double operator +(double value, Percentage percentage)
        {
            return value * (1 + (Convert.ToDouble(percentage.Fraction)));
        }

        /// <summary>
        /// Subtract the percentage from the given value. E.g. 5 - 10% = 4.5
        /// </summary>
        /// <param name="value">The value to subtract the percentage from</param>
        /// <param name="percentage">the percentage to subtract</param>
        /// <returns>The result, converted to decimal</returns>
        public static decimal operator -(int value, Percentage percentage)
        {
            return value * (1 - percentage.Fraction);
        }

        /// <summary>
        /// Subtract the percentage from the given value. E.g. 5 - 10% = 4.5
        /// </summary>
        /// <param name="value">The value to subtract the percentage from</param>
        /// <param name="percentage">the percentage to subtract</param>
        /// <returns>The result</returns>
        public static decimal operator -(decimal value, Percentage percentage)
        {
            return value * (1 - percentage.Fraction);
        }

        /// <summary>
        /// Subtract the percentage from the given value. E.g. 5 - 10% = 4.5
        /// </summary>
        /// <param name="value">The value to subtract the percentage from</param>
        /// <param name="percentage">the percentage to subtract</param>
        /// <returns>The result</returns>
        public static double operator -(double value, Percentage percentage)
        {
            return value * (1 - Convert.ToDouble(percentage.Fraction));
        }

        /// <summary>
        /// Calculate the result of the percentage of a given value. E.g. 200 * 10% = 20 and 
        /// convert the result to a decimal
        /// </summary>
        public static decimal operator *(int value, Percentage percentage)
        {
            return value * percentage.Fraction;
        }

        /// <summary>
        /// Calculate the result of the percentage of a given value. E.g. 200 * 10% = 20
        /// </summary>
        public static decimal operator *(decimal value, Percentage percentage)
        {
            return value * percentage.Fraction;
        }

        /// <summary>
        /// Calculate the result of the percentage of a given value. E.g. 200 * 10% = 20
        /// </summary>
        public static double operator *(double value, Percentage percentage)
        {
            return value * Convert.ToDouble(percentage.Fraction);
        }

        ///<inheritdoc/>
        public static bool operator >(Percentage lhs, Percentage rhs)
        {
            return lhs._value > rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator <(Percentage lhs, Percentage rhs)
        {
            return lhs._value < rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator >(Percentage lhs, int rhs)
        {
            return lhs._value > rhs;
        }

        ///<inheritdoc/>
        public static bool operator <(Percentage lhs, int rhs)
        {
            return lhs._value < rhs;
        }

        ///<inheritdoc/>
        public static bool operator >(Percentage lhs, decimal rhs)
        {
            return lhs._value > rhs;
        }

        ///<inheritdoc/>
        public static bool operator <(Percentage lhs, decimal rhs)
        {
            return lhs._value < rhs;
        }

        ///<inheritdoc/>
        public static bool operator >(Percentage lhs, double rhs)
        {
            return lhs._value > Convert.ToDecimal(rhs);
        }

        ///<inheritdoc/>
        public static bool operator <(Percentage lhs, double rhs)
        {
            return lhs._value < Convert.ToDecimal(rhs);
        }
        ///<inheritdoc/>
        public static bool operator >(int lhs, Percentage rhs)
        {
            return lhs > rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator <(int lhs, Percentage rhs)
        {
            return lhs < rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator >(decimal lhs, Percentage rhs)
        {
            return lhs > rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator <(decimal lhs, Percentage rhs)
        {
            return lhs < rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator >(double lhs, Percentage rhs)
        {
            return Convert.ToDecimal(lhs) > rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator <(double lhs, Percentage rhs)
        {
            return Convert.ToDecimal(lhs) < rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator >=(Percentage lhs, Percentage rhs)
        {
            return lhs._value >= rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator <=(Percentage lhs, Percentage rhs)
        {
            return lhs._value <= rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator >=(Percentage lhs, int rhs)
        {
            return lhs._value >= rhs;
        }

        ///<inheritdoc/>
        public static bool operator <=(Percentage lhs, int rhs)
        {
            return lhs._value <= rhs;
        }

        ///<inheritdoc/>
        public static bool operator >=(Percentage lhs, decimal rhs)
        {
            return lhs._value >= rhs;
        }

        ///<inheritdoc/>
        public static bool operator <=(Percentage lhs, decimal rhs)
        {
            return lhs._value <= rhs;
        }

        ///<inheritdoc/>
        public static bool operator >=(Percentage lhs, double rhs)
        {
            return lhs._value >= Convert.ToDecimal(rhs);
        }

        ///<inheritdoc/>
        public static bool operator <=(Percentage lhs, double rhs)
        {
            return lhs._value <= Convert.ToDecimal(rhs);
        }
        ///<inheritdoc/>
        public static bool operator >=(int lhs, Percentage rhs)
        {
            return lhs >= rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator <=(int lhs, Percentage rhs)
        {
            return lhs <= rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator >=(decimal lhs, Percentage rhs)
        {
            return lhs >= rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator <=(decimal lhs, Percentage rhs)
        {
            return lhs <= rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator >=(double lhs, Percentage rhs)
        {
            return Convert.ToDecimal(lhs) >= rhs._value;
        }

        ///<inheritdoc/>
        public static bool operator <=(double lhs, Percentage rhs)
        {
            return Convert.ToDecimal(lhs) <= rhs._value;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals((Percentage)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <inheritdoc />
        public bool Equals(Percentage other)
        {
            return _value.Equals(other._value);
        }

        /// <inheritdoc/>
        public int CompareTo(int other)
        {
            return _value.CompareTo(other);
        }

        /// <inheritdoc/>
        public int CompareTo(decimal other)
        {
            return _value.CompareTo(other);
        }

        /// <inheritdoc/>
        public int CompareTo(double other)
        {
            return _value.CompareTo(Convert.ToDecimal(other));
        }

        /// <inheritdoc/>
        public int CompareTo(Percentage other)
        {
            return _value.CompareTo(other._value);
        }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is int) return CompareTo((int)obj);
            if (obj is double) return CompareTo((double)obj);
            if (obj is decimal) return CompareTo((decimal)obj);
            if (obj is Percentage) return CompareTo((Percentage)obj);
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc/>
        public void ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            reader.MoveToContent();
            var value = reader.ReadElementContentAsString();
            Unsafe.AsRef(this) = new Percentage(decimal.Parse(value, CultureInfo.InvariantCulture));
        }

        /// <inheritdoc/>
        public void WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            writer.WriteElementString("value", Value.ToString(null, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Divide the amount by the given percentage.
        /// Note that this is extremely useful to calculate for instance the amount without VAT, given the total amount. 
        /// E.g. to calculate the amount without 25% VAT for 10,- (which is 8,-), 
        /// calculate <code>10 / new Percentage(125)</code>
        ///</summary>
        public static decimal operator /(int value, Percentage percentage)
        {
            return value / percentage.Fraction;
        }

        /// <summary>
        /// Divide the amount by the given percentage.
        /// Note that this is extremely useful to calculate for instance the amount without VAT, given the total amount. 
        /// E.g. to calculate the amount without 25% VAT for 10,- (which is 8,-), 
        /// calculate <code>10 / new Percentage(125)</code>
        ///</summary>
        public static decimal operator /(decimal value, Percentage percentage)
        {
            return value / percentage.Fraction;
        }

        /// <summary>
        /// Divide the amount by the given percentage.
        /// Note that this is extremely useful to calculate for instance the amount without VAT, given the total amount. 
        /// E.g. to calculate the amount without 25% VAT for 10,- (which is 8,-), 
        /// calculate <code>10 / new Percentage(125)</code>
        ///</summary>
        public static double operator /(double value, Percentage percentage)
        {
            return value / Convert.ToDouble(percentage.Fraction);
        }

        /// <summary>
        /// Cast the percentage value to a percentage struct
        /// </summary>
        public static explicit operator Percentage(int percentage)
        {
            return new Percentage(percentage);
        }

        /// <summary>
        /// Cast the percentage value to a percentage struct
        /// </summary>
        public static explicit operator Percentage(double percentage)
        {
            return new Percentage(percentage);
        }

        /// <summary>
        /// Cast the percentage value to a percentage struct
        /// </summary>
        public static explicit operator Percentage(decimal percentage)
        {
            return new Percentage(percentage);
        }

    }
}
