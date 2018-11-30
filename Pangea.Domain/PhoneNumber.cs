using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Pangea.Domain
{
    /// <summary>
    /// Represents an international phone number, including the country code and multiple ways to format the phone number.
    /// It also allows local (national) phone numbers when the country code is specified. Useful for applications that are 
    /// used within one country and therefore have an implicit country code for all phone numbers.
    /// </summary>
    public struct PhoneNumber
        : IEquatable<PhoneNumber>
        , IFormattable
        , IXmlSerializable
    {
        private static readonly Regex _internationalExpression = new Regex(@"^(00|\+)(?<numbers>(\d|\s)+)$", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex _localExpression = new Regex(@"^0(?<numbers>(\d|\s)+)$", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// The string representation of the phone number, including the original spaces
        /// </summary>
        private string Text { get; }
        /// <summary>
        /// The string representation of the phone number, excluding spaces
        /// </summary>
        private string Trimmed { get; }
        /// <summary>
        /// The country code of the phone number, if known, otherwise null
        /// </summary>
        public int? CountryCode { get; }

#pragma warning disable AV1500 // Member or local function contains more than 7 statements
        /// <summary>
        /// Create a phone number based on the text representation of the phone number. 
        /// Allowed formats are +31 12 34 56 789, 00 31 12 34 56 789, with or without spaces 
        /// </summary>
        /// <param name="text"></param>
        public PhoneNumber(string text)
#pragma warning restore AV1500 // Member or local function contains more than 7 statements
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
                if (!result.Success) throw new FormatException($"Invalid phone number '{text}'");
                var numbers = result.Groups["numbers"].Value;
                Text = numbers;
                Trimmed = Text.Replace(" ", "");
                CountryCode = CountryCodes.Instance?.GetCountryCallingCodeFrom(numbers);
            }
        }

#pragma warning disable AV1500 // Member or local function contains more than 7 statements
        /// <summary>
        /// Create a phone number from a text representation that is in a local format. That is
        /// a number starting with a 0 without a country calling code. The country calling code is given.
        /// </summary>
        /// <param name="countryCode">The country calling code</param>
        /// <param name="text">The local phone number, starting with a 0, which can include spaces</param>
        public PhoneNumber(int countryCode, string text)
#pragma warning restore AV1500 // Member or local function contains more than 7 statements
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
                if (!result.Success) throw new FormatException($"Invalid phone number '{text}'");
                var numbers = result.Groups["numbers"].Value;
                Text = countryCode + numbers;
                Trimmed = Text?.Replace(" ", "");
                CountryCode = countryCode;
            }
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
        public static bool operator ==(PhoneNumber lhs, PhoneNumber rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Check whether the given objects do not represent the same phone number
        /// </summary>
        /// <returns>False when representing the same phone numbers, True otherwise</returns>
        public static bool operator !=(PhoneNumber lhs, PhoneNumber rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// The hashcode of the phone number
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Trimmed?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Check whether the given phone number is similar to the current one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PhoneNumber other)
        {
            return Trimmed == other.Trimmed;
        }


        /// <summary>
        /// The string representation of the phone number, by default the international format, with + sign of the phone number.
        /// The spaces in the original phone number are preserved.
        /// </summary>
        /// <returns>The default (international) string representation of the phone number. Equal to the "G"-format or null format</returns>
        public override string ToString()
        {
            return ToString("G");
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
            return ToString(format, null);
        }

#pragma warning disable AV1500 // Member or local function contains more than 7 statements
        /// <summary>
        /// The string representation of the phone number, given in the format specified. 
        /// </summary>
        /// <param name="format">
        /// <list type="bullet">
        /// <item>G: The default format. International format using a + sign, including the original spaces (+31 123 456 789)</item>
        /// <item>g: International format using a + sign, excluding the original spaces (+31123456789)</item>
        /// <item>O: International format using 0's, including the original spaces (0031 123 456 789)</item>
        /// <item>o: International format using 0's, excluding the original spaces (0031123456789)</item>
        /// <item>L: Local format, including the original spaces (0 123 456 789)</item>
        /// <item>l: Local format, excluding the original spaces (0123456789)</item>
        /// </list>
        /// </param>
        /// <param name="formatProvider">The format provider, does not change the output at all</param>
        /// <returns>The phone number in the given format</returns>
        public string ToString(string format, IFormatProvider formatProvider)
#pragma warning restore AV1500 // Member or local function contains more than 7 statements
        {
            if (Text == null) return null;

            switch (format)
            {
                case "o":
                    return "00" + Trimmed;
                case "O":
                    return "00" + Text;
                case "g":
                    return "+" + Trimmed;
                case "L":
                    if (CountryCode == null) return ToString("G");
                    return "0" + Text.Substring(CountryCode?.ToString().Length ?? 0);
                case "l":
                    if (CountryCode == null) return ToString("g");
                    return "0" + Trimmed.Substring(CountryCode?.ToString().Length ?? 0);
                case "G":
                case null:
                    return "+" + Text;
                default:
                    throw new FormatException($"Cannot create a representation of the phone number with the {format} format");
            }
        }

        /// <inheritdoc />
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc />
        public void ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            reader.MoveToContent();
            var value = reader.ReadElementContentAsString();
            Unsafe.AsRef(this) = new PhoneNumber(value);
        }

        /// <inheritdoc />
        public void WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (string.IsNullOrEmpty(Text)) writer.WriteElementString("value", string.Empty);
            else writer.WriteElementString("value", "+" + Text);
        }
    }
}
