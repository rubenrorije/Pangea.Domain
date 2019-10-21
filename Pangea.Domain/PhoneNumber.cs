using Pangea.Domain.ExtensionMethods;
using Pangea.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Pangea.Domain
{
    /// <summary>
    /// Represents an international phone number, including the country calling code. It has multiple ways to format a phone number.
    /// It also allows local (national) phone numbers when the country code is specified. Separators allowed within phone numbers are: space, dot, slash and dash (' ', '.', '/', '-')
    /// </summary>
    [Serializable]
    public struct PhoneNumber
        : IEquatable<PhoneNumber>
        , IConvertible
        , IFormattable
        , IXmlSerializable
    {
        /// <summary>
        /// All separators that are allowed in between phone numbers
        /// </summary>
        internal static readonly string[] AllowedSeparators = new string[] { " ", ".", "-", "/", "(0)" };
        private static readonly Regex _internationalExpression = new Regex(@"^(00|\+)(?<numbers>(\(0\)|[\d\s-/\.])+)$", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex _localExpression = new Regex(@"^0(?<numbers>(\d|(?:[\s-/\.]))+)$", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// The string representation of the phone number, including the original spaces.
        /// </summary>
        internal string Text { get; }

        /// <summary>
        /// The string representation of the phone number, excluding spaces
        /// </summary>
        internal string Trimmed { get; }

        /// <summary>
        /// The country code of the phone number, if known, otherwise null
        /// </summary>
        public int? CountryCode { get; }

        /// <summary>
        /// Create a phone number based on the text representation of the phone number. 
        /// Allowed formats are +31 12 34 56 789, 00 31 12 34 56 789, with or without spaces, dashes, dots, slashes.
        /// </summary>
        /// <param name="text">the text to parse to a valid PhoneNumbe</param>
        /// <exception cref="FormatException">When the text cannot be parsed to a phone number</exception>
        public PhoneNumber(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Text = null;
                Trimmed = null;
                CountryCode = null;
            }
            else
            {
                var result = _internationalExpression.Match(text);
                if (!result.Success) throw new ArgumentOutOfRangeException(nameof(text), Resources.PhoneNumber_Invalid);
                var numbers = result.Groups["numbers"].Value;
                CountryCode = CountryCodes.Instance?.GetCountryCallingCodeFrom(numbers);
                Text = numbers.ReplaceFirst(CountryCode?.ToString(CultureInfo.InvariantCulture), string.Empty);
                Trimmed = Text.RemoveAll(AllowedSeparators);
            }
        }

        private PhoneNumber(Match match)
        {
            if (!match.Success) throw new FormatException(Resources.PhoneNumber_Invalid);
            var numbers = match.Groups["numbers"].Value;
            Text = numbers;
            Trimmed = Text.RemoveAll(AllowedSeparators);
            CountryCode = CountryCodes.Instance?.GetCountryCallingCodeFrom(numbers);
        }

        /// <summary>
        /// Create a phone number from a text representation that is in a local format. That is
        /// a number starting with a 0 without a country calling code.
        /// </summary>
        /// <param name="countryCode">The country calling code</param>
        /// <param name="text">The local phone number, starting with a 0, which can include spaces</param>
        ///<exception cref="ArgumentOutOfRangeException">When the country code is non-positive</exception>
        /// <exception cref="FormatException">When the text cannot be parsed to a phone number</exception>
        public PhoneNumber(int countryCode, string text)
        {
            if (countryCode <= 0) throw new ArgumentOutOfRangeException(nameof(countryCode));

            if (string.IsNullOrEmpty(text))
            {
                Text = null;
                Trimmed = null;
                CountryCode = null;
            }
            else
            {
                var result = _localExpression.Match(text);
                if (!result.Success) throw new FormatException(Resources.PhoneNumber_Invalid);
                var numbers = result.Groups["numbers"].Value;
                Text = numbers;
                Trimmed = Text?.RemoveAll(AllowedSeparators);
                CountryCode = countryCode;
            }
        }

        private PhoneNumber(int countryCode, Match match)
        {
            if (!match.Success) throw new FormatException(Resources.PhoneNumber_Invalid);
            var numbers = match.Groups["numbers"].Value;
            Text = numbers;
            Trimmed = Text?.RemoveAll(AllowedSeparators);
            CountryCode = countryCode;
        }

        /// <summary>
        /// Check whether the given object is a phone number and represents the same phone number
        /// </summary>
        /// <param name="obj">the object to check</param>
        /// <returns>True when representing the same phone number, false otherwise</returns>
        public override bool Equals(object obj)
        {
            return obj is PhoneNumber && Equals((PhoneNumber)obj);
        }

        /// <summary>
        /// Check whether the given objects represent the same phone number
        /// </summary>
        /// <returns>True when representing the same phone numbers, False otherwise</returns>
        public static bool operator ==(PhoneNumber lhs, PhoneNumber rhs)=> lhs.Equals(rhs);

        /// <summary>
        /// Check whether the given objects do not represent the same phone number
        /// </summary>
        /// <returns>False when representing the same phone numbers, True otherwise</returns>
        public static bool operator !=(PhoneNumber lhs, PhoneNumber rhs)=> !lhs.Equals(rhs);

        /// <summary>
        /// The hashcode of the phone number
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (CountryCode == null && Trimmed == null) return 0;
            return (CountryCode + Trimmed)?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Check whether the given phone number is similar to the current one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PhoneNumber other)
        {
            return
                Trimmed == other.Trimmed &&
                CountryCode == other.CountryCode;
        }


        /// <summary>
        /// The string representation of the phone number, by default the international format, with + sign of the phone number.
        /// The spaces in the original phone number are preserved.
        /// </summary>
        /// <returns>The default (international) string representation of the phone number. Equal to the "G"-format or null format</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// The string representation of the phone number, given in the format specified. 
        /// </summary>
        /// <param name="format">
        /// <list type="bullet">
        /// <item>G: The default format. International format using a + sign, including the original spaces (+31 123 456 789)</item>
        /// <item>g: International format using a + sign, excluding the original spaces (+31123456789)</item>
        /// <item>O: International format using 0's, including the original spaces (0031 123 456 789)</item>
        /// <item>o: International format using 0's, excluding the original spaces (0031123456789)</item>
        /// </list>
        /// </param>
        /// <returns>The phone number in the given format</returns>
        public string ToString(string format)
        {
            return ToString(format, (IFormatProvider)null);
        }
        
        /// <summary>
        /// The string representation of the phone number, given in the format specified. 
        /// </summary>
        /// <param name="format">
        /// <list type="bullet">
        /// <item>G: The default format. International format using a + sign, including the original spaces (+31 123 456 789)</item>
        /// <item>g: International format using a + sign, excluding the original spaces (+31123456789)</item>
        /// <item>O: International format using 0's, including the original spaces (0031 123 456 789)</item>
        /// <item>o: International format using 0's, excluding the original spaces (0031123456789)</item>
        /// <item>L: Local format, including the original spaces (0 123 456 789) when the country calling code could not be found, the global format will be returned</item>
        /// <item>l: Local format, excluding the original spaces (0123456789) when the country calling code could not be found, the global format will be returned</item>
        /// </list>
        /// </param>
        /// <param name="formatProvider">The format provider, does not change the output at all</param>
        /// <returns>The phone number in the given format</returns>
        /// <exception cref="FormatException">A format exception will be thrown when the format is incorrect</exception>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return PhoneNumberFormatter.Format(this, format);
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
                Unsafe.AsRef(this) = default;
            }
            else
            {
                Unsafe.AsRef(this) = new PhoneNumber(value);
            }

            reader.Skip();
        }

        /// <inheritdoc />
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            if (!string.IsNullOrEmpty(Text)) writer.WriteAttributeString("value", "+" + CountryCode + Text);
        }


        /// <summary>
        /// Try to parse the text to an international phone number. When this succeeds the result will contain the 
        /// parsed phonenumber and the return value will be true. Otherwise the resulting phonenumber will
        /// be empty and the return value will be false.
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <param name="result">the parsed phone number</param>
        /// <returns>was the parsing succesful?</returns>
        public static bool TryParse(string text, out PhoneNumber result)
        {
            if (string.IsNullOrEmpty(text))
            {
                result = default;
                return true;
            }
            else
            {
                var match = _internationalExpression.Match(text);
                if (match.Success)
                {
                    result = new PhoneNumber(match);
                    return true;
                }
                else
                {
                    result = default;
                    return false;
                }
            }
        }

        /// <summary>
        /// Try to parse the text to a local phone number for the given country code. 
        /// </summary>
        /// <param name="countryCallingCode">The calling code for the country</param>
        /// <param name="text">the text of the phone number that should be parsed.</param>
        /// <param name="result">the parsed phone number</param>
        /// <returns>Was the parsing successful?</returns>
        public static bool TryParse(int countryCallingCode, string text, out PhoneNumber result)
        {
            if (string.IsNullOrEmpty(text))
            {
                result = default;
                return true;
            }
            else if (countryCallingCode <= 0)
            {
                result = default;
                return false;
            }
            else
            {
                var match = _localExpression.Match(text);
                if (match.Success)
                {
                    result = new PhoneNumber(countryCallingCode, match);
                    return true;
                }
                else
                {
                    result = default;
                    return false;
                }
            }
        }
        /// <summary>
        /// Cast the phone number to string
        /// </summary>
        public static explicit operator string(PhoneNumber phoneNumber)
        {
            return phoneNumber.ToString("G", CultureInfo.CurrentCulture);
        }

        TypeCode IConvertible.GetTypeCode() => TypeCode.Object;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Text, provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(Text, provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(Text, provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Text, provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Text, provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(Text, provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(Text, provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(Text, provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(Text, provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(Text, provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(Text, provider);
        string IConvertible.ToString(IFormatProvider provider) => Convert.ToString(Text, provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Text, conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Text, provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Text, provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Text, provider);
    }
}
