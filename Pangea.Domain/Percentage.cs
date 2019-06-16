using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    [Serializable]
    public struct Percentage
        : IFormattable
        , IEquatable<Percentage>
        , IComparable<int>
        , IComparable<decimal>
        , IComparable<double>
        , IComparable<Percentage>
        , IComparable
        , IConvertible
        , IXmlSerializable
    {
        /// <summary>
        /// The actual percentage value
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// The percentage value, represented as a fraction, which is by definition Value / 100
        /// </summary>
        public decimal Fraction => Value / 100m;

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
            Value = percentage;
        }

        /// <summary>
        /// Create a percentage from the given double percentage.
        /// Assumes the percentage given is the actual percentage, not the fraction
        /// </summary>
        /// <param name="percentage">The percentage, not the fraction</param>
        public Percentage(double percentage)
        {
            Value = Convert.ToDecimal(percentage);
        }

        /// <summary>
        /// Create a percentage from the given decimal percentage.
        /// Assumes the percentage given is the actual percentage, not the fraction
        /// </summary>
        /// <param name="percentage">The percentage, not the fraction</param>
        public Percentage(int percentage)
        {
            Value = percentage;
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
        /// Shorthand for calling <c>ToString(null)</c> or <c>ToString("G")</c>
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
            var safeFormat = format;
            if (string.IsNullOrEmpty(format))
            {
                safeFormat = "0.#########";
            }
            return Value.ToString(safeFormat, formatProvider) + "%";
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
        /// <returns>The percentage</returns>
        public static Percentage From(int nominator, int denominator)
        {
            if (denominator <= 0) throw new ArgumentOutOfRangeException(nameof(denominator));
            return FromFraction(nominator / (denominator * 1m));
        }

        /// <inheritdoc />
        public static bool operator ==(Percentage lhs, Percentage rhs) => lhs.Value == rhs.Value;

        /// <inheritdoc />
        public static bool operator !=(Percentage lhs, Percentage rhs) => !(lhs == rhs);

        /// <inheritdoc />
        public static bool operator ==(Percentage lhs, int rhs) => lhs.Value == rhs;

        /// <inheritdoc />
        public static bool operator !=(Percentage lhs, int rhs) => lhs.Value != rhs;

        /// <inheritdoc />
        public static bool operator ==(int lhs, Percentage rhs) => lhs == rhs.Value;

        /// <inheritdoc />
        public static bool operator !=(int lhs, Percentage rhs) => lhs != rhs.Value;
        /// <inheritdoc />
        public static bool operator ==(Percentage lhs, decimal rhs) => lhs.Value == rhs;

        /// <inheritdoc />
        public static bool operator !=(Percentage lhs, decimal rhs) => lhs.Value != rhs;

        /// <inheritdoc />
        public static bool operator ==(decimal lhs, Percentage rhs) => lhs == rhs.Value;

        /// <inheritdoc />
        public static bool operator !=(decimal lhs, Percentage rhs) => lhs != rhs.Value;
        /// <inheritdoc />
        public static bool operator ==(Percentage lhs, double rhs) => lhs.Value == Convert.ToDecimal(rhs);

        /// <inheritdoc />
        public static bool operator !=(Percentage lhs, double rhs) => lhs.Value != Convert.ToDecimal(rhs);

        /// <inheritdoc />
        public static bool operator ==(double lhs, Percentage rhs) => Convert.ToDecimal(lhs) == rhs.Value;

        /// <inheritdoc />
        public static bool operator !=(double lhs, Percentage rhs) => Convert.ToDecimal(lhs) != rhs.Value;

        /// <summary>
        /// Add the percentage to the given value. E.g. 1 + 10% = 1.1
        /// </summary>
        /// <param name="value">the number to add the percentage to</param>
        /// <param name="percentage">The percentage to add</param>
        /// <returns>The result</returns>
        public static decimal operator +(int value, Percentage percentage) => value * (1 + percentage.Fraction);

        /// <summary>
        /// Add the percentage to the given value. E.g. 1 + 10% = 1.1
        /// </summary>
        /// <param name="value">the number to add the percentage to</param>
        /// <param name="percentage">The percentage to add</param>
        /// <returns>The result</returns>
        public static decimal operator +(decimal value, Percentage percentage) => percentage.Add(value);

        /// <summary>
        /// Add the percentage to the given value. E.g. 1 + 10% = 1.1
        /// </summary>
        /// <param name="value">the number to add the percentage to</param>
        /// <returns>The result</returns>
        public decimal Add(decimal value) => value * (1 + Fraction);

        /// <summary>
        /// Add the percentage to the given value. E.g. 1 + 10% = 1.1
        /// </summary>
        /// <param name="value">the number to add the percentage to</param>
        /// <param name="percentage">The percentage to add</param>
        /// <returns>The result</returns>
        public static double operator +(double value, Percentage percentage) => value * (1 + Convert.ToDouble(percentage.Fraction));

        /// <summary>
        /// Subtract the percentage from the given value. E.g. 5 - 10% = 4.5
        /// </summary>
        /// <param name="value">The value to subtract the percentage from</param>
        /// <param name="percentage">the percentage to subtract</param>
        /// <returns>The result, converted to decimal</returns>
        public static decimal operator -(int value, Percentage percentage) => percentage.Subtract(value);

        /// <summary>
        /// Subtract the percentage from the given value. E.g. 5 - 10% = 4.5
        /// </summary>
        /// <param name="value">The value to subtract the percentage from</param>
        /// <returns>The result, converted to decimal</returns>
        public decimal Subtract(int value) => value * (1 - Fraction);

        /// <summary>
        /// Subtract the percentage from the given value. E.g. 5 - 10% = 4.5
        /// </summary>
        /// <param name="value">The value to subtract the percentage from</param>
        /// <param name="percentage">the percentage to subtract</param>
        /// <returns>The result</returns>
        public static decimal operator -(decimal value, Percentage percentage) => value * (1 - percentage.Fraction);

        /// <summary>
        /// Subtract the percentage from the given value. E.g. 5 - 10% = 4.5
        /// </summary>
        /// <param name="value">The value to subtract the percentage from</param>
        /// <param name="percentage">the percentage to subtract</param>
        /// <returns>The result</returns>
        public static double operator -(double value, Percentage percentage) => value * (1 - Convert.ToDouble(percentage.Fraction));


        /// <summary>
        /// Negates the given percentage
        /// </summary>
        /// <param name="percentage">The percentage to negate</param>
        /// <returns>The negated percentage</returns>
        public static Percentage operator -(Percentage percentage) => FromFraction(-percentage.Fraction);

        /// <summary>
        /// Create the negative of the current percentage
        /// </summary>
        /// <returns></returns>
        public Percentage Negate()
        {
            return FromFraction(-Fraction);
        }

        /// <summary>
        /// Returns the absolute (positive) percentage. 
        /// </summary>
        /// <param name="percentage">The percentage</param>
        /// <returns>The positive percentage</returns>
        public static Percentage Abs(Percentage percentage)
        {
            if (percentage.Fraction >= 0) return percentage;
            return -percentage;
        }

        /// <summary>
        /// Calculate the result of the percentage of a given value. E.g. 200 * 10% = 20 and 
        /// convert the result to a decimal
        /// </summary>
        public static decimal operator *(int value, Percentage percentage) => value * percentage.Fraction;

        /// <summary>
        /// Calculate the result of the percentage of a given value. E.g. 200 * 10% = 20
        /// </summary>
        public static decimal operator *(decimal value, Percentage percentage) => percentage.Multiply(value);

        /// <summary>
        /// Calculate the result of the percentage of a given value. E.g. 200 * 10% = 20
        /// </summary>
        public decimal Multiply(decimal value)
        {
            return value * Fraction;
        }

        /// <summary>
        /// Combine the given percentages into a single percentage that will be the same as applying the percentages sequentially. 
        /// </summary>
        /// <param name="percentages">The percentages to combine</param>
        /// <returns>One percentage reflecting the combination of all the given percentages</returns>
        public static Percentage Compounded(params Percentage[] percentages)
        {
            if (percentages == null) throw new ArgumentNullException(nameof(percentages));
            if (percentages.Length == 0) throw new ArgumentOutOfRangeException(nameof(percentages));

            var result = percentages[0];

            foreach (var current in percentages.Skip(1))
            {
                result = FromFraction((1 + result.Fraction) * (1 + current.Fraction) - 1);
            }
            return result;
        }

        /// <summary>
        /// Rounds a percentage value to the nearest integral value.
        /// </summary>
        /// <returns>The integer nearest to the percentage value. If the fractional component of d is halfway
        /// between two integers, one of which is even and the other odd, the even number is returned.
        ///</returns>
        public Percentage Round()
        {
            return new Percentage(Math.Round(Value));
        }

        /// <summary>
        /// Rounds a decimal value to a specified number of fractional digits.
        /// </summary>
        /// <param name="mode">Specification for how to round d if it is midway between two other numbers.</param>
        /// <returns>The number nearest to d that contains a number of fractional digits equal to decimals.</returns>
        public Percentage Round(MidpointRounding mode)
        {
            return new Percentage(Math.Round(Value, mode));
        }

        /// <summary>
        /// Rounds a decimal value to a specified number of fractional digits.
        /// </summary>
        /// <param name="decimals">The number of decimal places in the return value.</param>
        /// <returns>The number nearest to d that contains a number of fractional digits equal to decimals.</returns>
        public Percentage Round(int decimals)
        {
            return new Percentage(Math.Round(Value, decimals));
        }
        /// <summary>
        /// Rounds a decimal value to a specified number of fractional digits.
        /// </summary>
        /// <param name="decimals">The number of decimal places in the return value.</param>
        /// <param name="mode">Specification for how to round d if it is midway between two other numbers.</param>
        /// <returns>The number nearest to d that contains a number of fractional digits equal to decimals.</returns>
        public Percentage Round(int decimals, MidpointRounding mode)
        {
            return new Percentage(Math.Round(Value, decimals, mode));
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified decimal number.
        /// </summary>
        public Percentage Floor()
        {
            return new Percentage(Math.Floor(Value));
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified decimal number.
        /// </summary>
        public Percentage Ceiling()
        {
            return new Percentage(Math.Ceiling(Value));
        }

        /// <summary>
        /// Calculate the result of the percentage of a given value. E.g. 200 * 10% = 20
        /// </summary>
        public static double operator *(double value, Percentage percentage) => value * Convert.ToDouble(percentage.Fraction);

        ///<inheritdoc/>
        public static bool operator >(Percentage lhs, Percentage rhs) => lhs.Value > rhs.Value;

        ///<inheritdoc/>
        public static bool operator <(Percentage lhs, Percentage rhs) => lhs.Value < rhs.Value;

        ///<inheritdoc/>
        public static bool operator >(Percentage lhs, int rhs) => lhs.Value > rhs;

        ///<inheritdoc/>
        public static bool operator <(Percentage lhs, int rhs) => lhs.Value < rhs;

        ///<inheritdoc/>
        public static bool operator >(Percentage lhs, decimal rhs) => lhs.Value > rhs;

        ///<inheritdoc/>
        public static bool operator <(Percentage lhs, decimal rhs) => lhs.Value < rhs;

        ///<inheritdoc/>
        public static bool operator >(Percentage lhs, double rhs) => lhs.Value > Convert.ToDecimal(rhs);

        ///<inheritdoc/>
        public static bool operator <(Percentage lhs, double rhs) => lhs.Value < Convert.ToDecimal(rhs);
        ///<inheritdoc/>
        public static bool operator >(int lhs, Percentage rhs) => lhs > rhs.Value;

        ///<inheritdoc/>
        public static bool operator <(int lhs, Percentage rhs) => lhs < rhs.Value;

        ///<inheritdoc/>
        public static bool operator >(decimal lhs, Percentage rhs) => lhs > rhs.Value;

        ///<inheritdoc/>
        public static bool operator <(decimal lhs, Percentage rhs) => lhs < rhs.Value;

        ///<inheritdoc/>
        public static bool operator >(double lhs, Percentage rhs) => Convert.ToDecimal(lhs) > rhs.Value;

        ///<inheritdoc/>
        public static bool operator <(double lhs, Percentage rhs) => Convert.ToDecimal(lhs) < rhs.Value;

        ///<inheritdoc/>
        public static bool operator >=(Percentage lhs, Percentage rhs) => lhs.Value >= rhs.Value;

        ///<inheritdoc/>
        public static bool operator <=(Percentage lhs, Percentage rhs) => lhs.Value <= rhs.Value;

        ///<inheritdoc/>
        public static bool operator >=(Percentage lhs, int rhs) => lhs.Value >= rhs;

        ///<inheritdoc/>
        public static bool operator <=(Percentage lhs, int rhs) => lhs.Value <= rhs;

        ///<inheritdoc/>
        public static bool operator >=(Percentage lhs, decimal rhs) => lhs.Value >= rhs;

        ///<inheritdoc/>
        public static bool operator <=(Percentage lhs, decimal rhs) => lhs.Value <= rhs;

        ///<inheritdoc/>
        public static bool operator >=(Percentage lhs, double rhs) => lhs.Value >= Convert.ToDecimal(rhs);

        ///<inheritdoc/>
        public static bool operator <=(Percentage lhs, double rhs) => lhs.Value <= Convert.ToDecimal(rhs);
        ///<inheritdoc/>
        public static bool operator >=(int lhs, Percentage rhs) => lhs >= rhs.Value;

        ///<inheritdoc/>
        public static bool operator <=(int lhs, Percentage rhs) => lhs <= rhs.Value;

        ///<inheritdoc/>
        public static bool operator >=(decimal lhs, Percentage rhs) => lhs >= rhs.Value;

        ///<inheritdoc/>
        public static bool operator <=(decimal lhs, Percentage rhs) => lhs <= rhs.Value;

        ///<inheritdoc/>
        public static bool operator >=(double lhs, Percentage rhs) => Convert.ToDecimal(lhs) >= rhs.Value;

        ///<inheritdoc/>
        public static bool operator <=(double lhs, Percentage rhs) => Convert.ToDecimal(lhs) <= rhs.Value;

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals((Percentage)obj);

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        /// <inheritdoc />
        public bool Equals(Percentage other) => Value.Equals(other.Value);

        /// <inheritdoc/>
        public int CompareTo(int other) => Value.CompareTo(other);

        /// <inheritdoc/>
        public int CompareTo(decimal other) => Value.CompareTo(other);

        /// <inheritdoc/>
        public int CompareTo(double other) => Value.CompareTo(Convert.ToDecimal(other));

        /// <inheritdoc/>
        public int CompareTo(Percentage other) => Value.CompareTo(other.Value);

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
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <inheritdoc/>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var value = reader.MoveToAttribute("value") ? reader.Value : null;

            if (string.IsNullOrEmpty(value))
            {
                Unsafe.AsRef(this) = default;
            }
            else
            {
                Unsafe.AsRef(this) = new Percentage(decimal.Parse(value, CultureInfo.InvariantCulture));
            }

            reader.Skip();
        }

        /// <inheritdoc/>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            writer.WriteAttributeString("value", Value.ToString(null, CultureInfo.InvariantCulture));
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
            return percentage.Divide(value);
        }

        /// <summary>
        /// Divide the amount by the given percentage.
        /// Note that this is extremely useful to calculate for instance the amount without VAT, given the total amount. 
        /// E.g. to calculate the amount without 25% VAT for 10,- (which is 8,-), 
        /// calculate <code>10 / new Percentage(125)</code>
        ///</summary>
        public decimal Divide(decimal value)
        {
            return value / Fraction;
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
            return FromInt32(percentage);
        }

        /// <summary>
        /// Convert the percentage value to a percentage struct
        /// </summary>
        public static Percentage FromInt32(int percentage)
        {
            return new Percentage(percentage);
        }

        /// <summary>
        /// Cast the percentage value to a percentage struct
        /// </summary>
        public static explicit operator Percentage(double percentage)
        {
            return FromDouble(percentage);
        }

        /// <summary>
        /// Convert the percentage value to a percentage struct
        /// </summary>
        public static Percentage FromDouble(double percentage)
        {
            return new Percentage(percentage);
        }

        /// <summary>
        /// Cast the percentage value to a percentage struct
        /// </summary>
        public static explicit operator Percentage(decimal percentage)
        {
            return FromDecimal(percentage);
        }

        /// <summary>
        /// Convert the percentage value to a percentage struct
        /// </summary>
        public static Percentage FromDecimal(decimal percentage)
        {
            return new Percentage(percentage);
        }

        /// <summary>
        /// Cast the percentage to a decimal
        /// </summary>
        public static explicit operator decimal(Percentage percentage)
        {
            return percentage.ToDecimal();
        }

        /// <summary>
        /// Convert the percentage to a decimal
        /// </summary>
        public decimal ToDecimal()
        {
            return Value;
        }

        /// <summary>
        /// Cast the percentage to a double
        /// </summary>
        public static explicit operator double(Percentage percentage)
        {
            return percentage.ToDouble();
        }

        /// <summary>
        /// Convert the percentage to a double
        /// </summary>
        public double ToDouble()
        {
            return Convert.ToDouble(Value);
        }

        /// <summary>
        /// Cast the percentage to an int
        /// </summary>
        public static explicit operator int(Percentage percentage)
        {
            return percentage.ToInt32();
        }

        /// <summary>
        /// Convert the percentage to an int
        /// </summary>
        public int ToInt32()
        {
            return Convert.ToInt32(Value);
        }

        TypeCode IConvertible.GetTypeCode() => TypeCode.Object;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Value, provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(Value, provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(Value, provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Value, provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Value, provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(Value, provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(Value, provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(Value, provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(Value, provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(Value, provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(Value, provider);
        string IConvertible.ToString(IFormatProvider provider) => Convert.ToString(Value, provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Value, provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Value, provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Value, provider);

    }
}
