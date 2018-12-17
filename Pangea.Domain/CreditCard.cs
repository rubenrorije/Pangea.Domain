using Pangea.Domain.Checksums;
using Pangea.Domain.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Pangea.Domain
{
    /// <summary>
    /// A validated object representing a credit card.
    /// </summary>
    [Serializable]
    public struct CreditCard
        : IEquatable<CreditCard>
        , IConvertible
        , IXmlSerializable
    {
        private readonly string _value;

        /// <summary>
        /// Create a new credit card that is validated using the given card number
        /// </summary>
        /// <param name="cardNumber">the card number in text format with or without spaces that must be validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the number of characters (excluding spaces) is less than 8, more than 19, are no digits, or when the checksum is not correct.</exception>
        public CreditCard(string cardNumber) : this(cardNumber, false)
        {
        }

        private CreditCard(string cardNumber, bool bypassChecks)
        {
            var trimmed = cardNumber?.Replace(" ", "");
            if (!bypassChecks && !string.IsNullOrEmpty(cardNumber))
            {
                if (trimmed.Length < 8) throw new ArgumentOutOfRangeException(nameof(cardNumber), "A credit card must be at least 8 characters");
                if (trimmed.Length > 19) throw new ArgumentOutOfRangeException(nameof(cardNumber), "A credit card must be less than 20 characters");
                if (trimmed.Any(chr => !char.IsDigit(chr))) throw new ArgumentOutOfRangeException(nameof(cardNumber), "A credit card can only contain digits or spaces");

                var algorithm = new LuhnChecksumCalculator();
                if (!algorithm.Validate(trimmed)) throw new ArgumentOutOfRangeException(nameof(cardNumber), "The creditcard is invalid because the checksum is incorrect");
            }
            _value = trimmed;
        }

        /// <summary>
        /// Compare this instance to another credit card.
        /// </summary>
        /// <param name="other">The credit card to compare to</param>
        /// <returns>Are the cards equivalent?</returns>
        public bool Equals(CreditCard other)
        {
            return string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Is this instance equal to the given <paramref name="obj"/>. 
        /// <c>true</c> when <paramref name="obj"/> is a credit card and they are equivalent.
        /// </summary>
        /// <param name="obj">the object to compare to</param>
        /// <returns>The <paramref name="obj"/> is a credit card and they are equivalent</returns>
        public override bool Equals(object obj)
        {
            if (obj is CreditCard) return Equals((CreditCard)obj);
            return base.Equals(obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _value?.GetHashCode() ?? 0;
        }

        /// <inheritdoc/>
        public static bool operator ==(CreditCard lhs, CreditCard rhs)
        {
            return lhs._value == rhs._value;
        }

        /// <inheritdoc/>
        public static bool operator !=(CreditCard lhs, CreditCard rhs)
        {
            return !(lhs == rhs);
        }


        /// <summary>
        /// Represent how the issuer is formatted. Since 2017 the issuer can be 8 (Long) characters,
        /// before only 6 (Short) characters were used to identify the issuer
        /// </summary>
        public enum IssuerIdentifierFormat
        {
            /// <summary>
            /// The issuer identifier is 8 characters long
            /// </summary>
            LongIdentifier = 8,
            /// <summary>
            /// The issuer identifier is 6 characters long
            /// </summary>
            ShortIdentifier = 6
        }

        /// <summary>
        /// Get the issuer based on the given format.
        /// </summary>
        /// <param name="format">Either Long (8) or Short (6)</param>
        /// <returns>The part of the credit card denoting the issuer</returns>
        public string GetIssuerIdentificationNumber(IssuerIdentifierFormat format)
        {
            if (string.IsNullOrEmpty(_value)) return _value;
            return _value.Substring(0, (int)format);
        }

        /// <summary>
        /// Get the individual's acount number. 
        /// </summary>
        /// <param name="format">How many characters are used for the Issuer, either 8 (Long) or 6 (Short)</param>
        /// <returns>The part of the credit card denoting the individual account number</returns>
        public string GetIndividualAccountNumber(IssuerIdentifierFormat format)
        {
            if (string.IsNullOrEmpty(_value)) return _value;
            return _value.Substring((int)format, _value.Length - (int)format - 1);
        }

        /// <summary>
        /// Returns a text format of the credit card grouped in digits of 4.
        /// </summary>
        /// <returns>the text format of the credit card grouped in digits of 4</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_value)) return _value;

            var grouped = _value.Page(4).Select(group => new string(group.ToArray()));

            return string.Join(" ", grouped);
        }


        /// <summary>
        /// Try to parse the text to a credit card. Returns true when the parsing succeeds, false otherwise
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <param name="result">the resulting credit card</param>
        public static bool TryParse(string text, out CreditCard result)
        {
            var trimmed = text?.Replace(" ", "");
            if (string.IsNullOrEmpty(text))
            {
                result = new CreditCard();
                return true;
            }
            if (!trimmed.All(Char.IsDigit))
            {
                result = new CreditCard();
                return false;
            }
            if (trimmed.Length < 8)
            {
                result = new CreditCard();
                return false;
            }
            if (trimmed.Length > 19)
            {
                result = new CreditCard();
                return false;
            }
            var algorithm = new LuhnChecksumCalculator();
            if (!algorithm.Validate(text))
            {
                result = new CreditCard();
                return false;
            }

            result = new CreditCard(text);
            return true;
        }

        /// <summary>
        /// Create a new credit card from the given card number, but bypass all checks. 
        /// This assumes that these checks are already done on the card number.
        /// </summary>
        /// <param name="cardNumber">The card number to create a credit card from. No validation on this parameter will be done.</param>
        /// <returns>The credit card</returns>
        public static CreditCard Unsafe(string cardNumber)
        {
            return new CreditCard(cardNumber, true);
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
                System.Runtime.CompilerServices.Unsafe.AsRef(this) = CreditCard.Unsafe(value);
            }

            reader.Skip();
        }

        /// <inheritdoc />
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            if (!string.IsNullOrEmpty(_value)) writer.WriteAttributeString("value", _value);
        }

        /// <summary>
        /// Cast a credit card to a string
        /// </summary>
        public static explicit operator string(CreditCard creditCard)
        {
            return creditCard.ToString();
        }

        TypeCode IConvertible.GetTypeCode() => TypeCode.Object;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(_value, provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(_value, provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(_value, provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(_value, provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(_value, provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(_value, provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(_value, provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(_value, provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(_value, provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(_value, provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(_value, provider);
        string IConvertible.ToString(IFormatProvider provider) => Convert.ToString(_value, provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(_value, conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(_value, provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(_value, provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(_value, provider);

    }
}
