using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Pangea.Domain
{
    /// <summary>
    /// The International Standard Book Number (ISBN) is a numeric commercial book identifier which is intended to be unique.
    /// Publishers purchase ISBNs from an affiliate of the International ISBN Agency. See https://en.wikipedia.org/wiki/International_Standard_Book_Number
    /// </summary>
    [Serializable]
    public struct Isbn
        : IEquatable<Isbn>
        , IConvertible
        , IFormattable
        , IXmlSerializable
    {
        private readonly string _text;
        private readonly string _isbn;

        private readonly string _prefix;
        private readonly string _identifierGroup;

        private static readonly Regex _isbnExpression = new Regex(@"^(?<numbers>(\d-?)+)$");


        /// <summary>
        /// GS-1 prefix, only specified when this is a 13 character Isbn
        /// </summary>
        public string Prefix => _prefix;

        /// <summary>
        /// Registration Group Identifier
        /// </summary>
        public string IdentifierGroup => _identifierGroup;


        private Isbn(string text, string isbn)
        {
            _text = text;
            _isbn = isbn;
            _prefix = isbn.Length == 10 ? null : isbn.Substring(0, 3);
            _identifierGroup = isbn.Substring(isbn.Length - 10);
        }

        /// <summary>
        /// Create a new Isbn based on the given text.
        /// </summary>
        /// <param name="isbn">the text that represents the Isbn</param>
        public Isbn(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                _text = isbn;
                _isbn = null;
                _prefix = null;
                _identifierGroup = null;
            }
            else
            {
                var match = _isbnExpression.Match(isbn);
                if (match.Success)
                {

                    var digits = match.Groups["numbers"].Value?.Replace("-", "");
                    if (digits.Length == 10 || digits.Length == 13)
                    {
                        _isbn = digits;
                        _text = isbn;
                        _prefix = digits.Length == 10 ? null : digits.Substring(0, 3);
                        _identifierGroup = ExtractIdentifierGroup(digits.Substring(digits.Length - 10));
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(isbn));
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(isbn));
                }
            }
        }

        private static string ExtractIdentifierGroup(string isbnWithoutPrefix)
        {
            if (isbnWithoutPrefix == null) return null;

            var c1 = isbnWithoutPrefix.Substring(0, 1);
            var c2 = isbnWithoutPrefix.Substring(0, 2);
            var c3 = isbnWithoutPrefix.Substring(0, 3);
            var c4 = isbnWithoutPrefix.Substring(0, 4);

            var i1 = int.Parse(c1, CultureInfo.InvariantCulture);
            var i2 = int.Parse(c2, CultureInfo.InvariantCulture);
            var i3 = int.Parse(c3, CultureInfo.InvariantCulture);
            var i4 = int.Parse(c4, CultureInfo.InvariantCulture);

            if (i4 >= 9990) return isbnWithoutPrefix.Substring(0, 5);
            else if (i3 >= 990) return c4;
            else if (i2 >= 95) return c3;
            else if (i2 >= 80) return c2;
            else if (i1 == 6) return c3;
            else return c1;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => Equals((Isbn)obj);

        /// <inheritdoc/>
        public bool Equals(Isbn other) => string.Equals(_isbn, other._isbn, StringComparison.InvariantCulture);

        /// <inheritdoc/>
        public override int GetHashCode() => _isbn?.GetHashCode() ?? 0;

        /// <inheritdoc/>
        public override string ToString() => ToString(null, null);

        /// <inheritdoc/>
        public static bool operator ==(Isbn lhs, Isbn rhs) => lhs.Equals(rhs);
        /// <inheritdoc/>
        public static bool operator !=(Isbn lhs, Isbn rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Cast the Isbn to a string
        /// </summary>
        public static explicit operator string(Isbn isbn) => isbn.ToString(null, null);

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
                System.Runtime.CompilerServices.Unsafe.AsRef(this) = new Isbn(value);
            }

            reader.Skip();
        }

        /// <inheritdoc />
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            if (!string.IsNullOrEmpty(_text)) writer.WriteAttributeString("value", _text);
        }

        /// <summary>
        /// Try to create a new Isbn from the text. Returns true when succeeds.
        /// </summary>
        /// <param name="text">The text to be parsed</param>
        /// <param name="result">the resulting Isbn</param>
        public static bool TryParse(string text, out Isbn result)
        {
            if (string.IsNullOrEmpty(text))
            {
                result = new Isbn(text);
                return true;
            }
            var sanitized = text.Replace("-", "");
            if (sanitized.Length == 10 || sanitized.Length == 13)
            {
                if (_isbnExpression.IsMatch(text))
                {
                    result = new Isbn(text);
                    return true;
                }
            }
            result = new Isbn();
            return false;
        }

        TypeCode IConvertible.GetTypeCode() => TypeCode.Object;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(_text, provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(_text, provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(_text, provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(_text, provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(_text, provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(_text, provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(_text, provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(_text, provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(_text, provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(_text, provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(_text, provider);
        string IConvertible.ToString(IFormatProvider provider) => Convert.ToString(_text, provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(_text, conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(_text, provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(_text, provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(_text, provider);

        /// <summary>
        /// Returns the string representation of the ISBN. 
        /// </summary>
        /// <param name="format">Allowed formats are: <c>B</c>, bare and <c>null</c>, <c>G</c> or <c>O</c> for the original representation.</param>
        /// <param name="formatProvider">The format provider</param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "B": return _isbn.ToString(formatProvider);
                case null:
                case "G":
                case "O":
                case "":
                    return _text;
                default:
                    throw new FormatException();
            }
        }

        /// <summary>
        /// Returns the string representation of the ISBN. Overload to prevent from passing the format when not used.
        /// </summary>
        /// <param name="formatProvider">The format provider</param>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

        /// <summary>
        /// Returns the string representation of the ISBN. 
        /// </summary>
        /// <param name="format">Allowed formats are: <c>B</c>, bare and <c>null</c>, <c>G</c> or <c>O</c> for the original representation.</param>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Create a new ISBN based on the given text bypassing all validity checks. Use this method when you know the text
        /// is a valid ISBN. 
        /// </summary>
        /// <param name="text">the already validated correct ISBN</param>
        /// <returns>The ISBN representation of the text</returns>
        public static Isbn Unsafe(string text)
        {
            if (string.IsNullOrEmpty(text)) return new Isbn();
            var digits = text.Replace("-", "");
            return new Isbn(text, digits);
        }
    }
}
