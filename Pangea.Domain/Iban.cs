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
    /// International bank account number (https://en.wikipedia.org/wiki/International_Bank_Account_Number)
    /// </summary>
    [Serializable]
    public struct Iban 
        : IXmlSerializable
        , IConvertible
        , IEquatable<Iban>
    {
        private readonly string _text;

        /// <summary>
        /// Country code using ISO 3166-1 alpha-2 – two letters
        /// </summary>
        public string CountryCode { get; }

        /// <summary>
        /// The check digits of the Iban to check that the Iban is a valid one.
        /// </summary>
        public string CheckDigits { get; }

        /// <summary>
        /// Basic Bank Account Number (BBAN) – up to 30 alphanumeric characters that are country-specific
        /// </summary>
        public string BasicBankAccountNumber { get; }


        private Iban(string countryCode, string checkDigits, string accountNumber, string text)
        {
            CountryCode = countryCode;
            CheckDigits = checkDigits;
            BasicBankAccountNumber = accountNumber;
            _text = text;
        }

        /// <summary>
        /// Create an IBAN based on the text given
        /// </summary>
        /// <param name="iban">The account number</param>
        /// <exception cref="ArgumentOutOfRangeException">When the text could not be parsed to a valid IBAN, or when the checksum is incorrect</exception>
        public Iban(string iban)
        {
            var parsed = IbanParser.TryParse(iban);
            if (!parsed.Valid) throw new ArgumentOutOfRangeException(parsed.Message);
            _text = parsed.ToString();
            CountryCode = parsed.CountryCode;
            CheckDigits = parsed.CheckDigits;
            BasicBankAccountNumber = parsed.BasicAccountNumber;
            if (!IbanParser.Validate(parsed))
            {
                throw new ArgumentOutOfRangeException(nameof(iban), "The entered Iban is incorrect.");
            }
        }

        /// <summary>
        /// Check equality of IBAN's 
        /// </summary>
        public bool Equals(Iban other)
        {
            return string.Equals(_text, other._text, StringComparison.CurrentCulture);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals((Iban)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _text.GetHashCode();
        }

        /// <summary>
        /// Check equality
        /// </summary>
        public static bool operator ==(Iban lhs, Iban rhs)
        {
            return lhs._text == rhs._text;
        }

        /// <summary>
        /// Check inequality
        /// </summary>
        public static bool operator !=(Iban lhs, Iban rhs)
        {
            return !(lhs == rhs);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _text ?? string.Empty;
        }

        /// <summary>
        /// Create an IBAN without doing any checks. Assumes that the checks have been done before.
        /// </summary>
        /// <param name="text">The already validated IBAN</param>
        public static Iban Unsafe(string text)
        {
            if (string.IsNullOrEmpty(text)) return new Iban();
            return new Iban(text.Substring(0, 2), text.Substring(2, 2), text.Substring(4).Replace(" ", ""), text);
        }

        ///<inheritdoc/>
        XmlSchema IXmlSerializable.GetSchema() => null;

        ///<inheritdoc/>
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
                System.Runtime.CompilerServices.Unsafe.AsRef(this) = Iban.Unsafe(value);
            }

            reader.Skip();
        }

        /// <inheritdoc/>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (_text != null) writer.WriteAttributeString("value", _text);
        }

        /// <summary>
        /// Try to parse the text to an Iban.
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <param name="result">the parsed Iban</param>
        /// <returns>whether the parsing was succesful</returns>
        public static bool TryParse(string text, out Iban result)
        {
            if (string.IsNullOrEmpty(text))
            {
                result = new Iban();
                return true;
            }

            var parsed = IbanParser.TryParse(text);
            if (!parsed.Valid)
            {
                result = new Iban();
                return false;
            }
            if (!IbanParser.Validate(parsed))
            {
                result = new Iban();
                return false;
            }

            result = new Iban(text);
            return true;
        }

        /// <summary>
        /// Cast an Iban to a string
        /// </summary>
        public static explicit operator string(Iban iban)
        {
            return iban.ToString();
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

    }
}
