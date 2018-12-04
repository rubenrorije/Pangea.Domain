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
    public struct Iban :
        IXmlSerializable,
        IEquatable<Iban>
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
            return _text.Equals(other._text);
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
        public XmlSchema GetSchema()
        {
            return null;
        }

        ///<inheritdoc/>
        public void ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            reader.MoveToContent();
            var value = reader.ReadElementContentAsString();
            System.Runtime.CompilerServices.Unsafe.AsRef(this) = Iban.Unsafe(value);
        }

        /// <inheritdoc/>
        public void WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            writer.WriteElementString("value", _text);
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
    }
}
