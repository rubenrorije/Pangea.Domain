using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Pangea.Domain
{
    /// <summary>
    /// Represents an angle in degrees
    /// </summary>
    [Serializable]
    public struct Angle
        : IEquatable<Angle>
        , IFormattable
        , IXmlSerializable
        , IConvertible
        , IComparable
        , IComparable<Angle>
    {
        /// <summary>
        /// The degrees of the Angle
        /// </summary>
        public decimal Degrees { get; }

        /// <summary>
        /// Create a new Angle based on the number of degrees (0-360)
        /// </summary>
        /// <param name="degrees">The degrees (0-360)</param>
        public Angle(decimal degrees)
        {
            var safeDegrees = degrees % 360;
            if (safeDegrees < 0) safeDegrees += 360;
            Degrees = safeDegrees;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals((Angle)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Degrees.GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals(Angle other)
        {
            return Degrees == other.Degrees;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Represent the Angle in the correct format. Standard numeric format strings are used to format the degrees
        /// </summary>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Represent the Angle in the correct format. Standard numeric format strings are used to format the degrees
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format) || format == "G") return $"{Degrees}°";
            else return Degrees.ToString(format, formatProvider);
        }

        /// <inheritdoc />
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <inheritdoc />
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var value = reader.MoveToAttribute("value") ? reader.Value : null;

            if (string.IsNullOrEmpty(value))
            {
                System.Runtime.CompilerServices.Unsafe.AsRef(this) = default;
            }
            else
            {
                System.Runtime.CompilerServices.Unsafe.AsRef(this) = new Angle(int.Parse(value, CultureInfo.InvariantCulture));
            }

            reader.Skip();
        }

        /// <inheritdoc />
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteAttributeString("value", Degrees.ToString(null, CultureInfo.InvariantCulture));
        }


        /// <inheritdoc/>
        public static bool operator ==(Angle left, Angle right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(Angle left, Angle right) => !(left == right);

        /// <inheritdoc/>
        public static bool operator >(Angle left, Angle right) => left.Degrees > right.Degrees;

        /// <inheritdoc/>
        public static bool operator >=(Angle left, Angle right) => left.Degrees >= right.Degrees;
        /// <inheritdoc/>
        public static bool operator <(Angle left, Angle right) => left.Degrees < right.Degrees;

        /// <inheritdoc/>
        public static bool operator <=(Angle left, Angle right) => left.Degrees <= right.Degrees;

        /// <summary>
        /// Extract the raw degrees value from the angle
        /// </summary>
        /// <param name="angle">The angle</param>
        public static explicit operator decimal(Angle angle) => angle.ToDecimal();

        /// <summary>
        /// Extract the raw degrees value from the angle
        /// </summary>
        public decimal ToDecimal() => Degrees;

        TypeCode IConvertible.GetTypeCode() => TypeCode.Object;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Degrees, provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(Degrees, provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(Degrees, provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Degrees, provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Degrees, provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(Degrees, provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(Degrees, provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(Degrees, provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(Degrees, provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(Degrees, provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(Degrees, provider);
        string IConvertible.ToString(IFormatProvider provider) => Convert.ToString(Degrees, provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Degrees, conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Degrees, provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Degrees, provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Degrees, provider);

        /// <summary>
        /// Compare the degrees to another angle
        /// </summary>
        /// <param name="other">the angle to compare to</param>
        /// <returns>the comparison of the two angles</returns>
        public int CompareTo(Angle other)
        {
            return Degrees.CompareTo(other.Degrees);
        }

        /// <inheritdoc/>
        public int CompareTo(object other)
        {
            return CompareTo((Angle)other);
        }
    }
}
