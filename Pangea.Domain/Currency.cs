using Pangea.Domain.Properties;
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
    /// Representation of a currency
    /// </summary>
    [Serializable]
    public sealed class Currency
        : IEquatable<Currency>
        , IXmlSerializable
    {
        /// <summary>
        /// the ISO 4217 code of the given currency
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Optional symbol for the given currency
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Create a currency based on the given ISO 4217 code and the numeric value
        /// </summary>
        /// <param name="code">The ISO 4217 code (uppercase)</param>
        /// <param name="symbol">the (optional) symbol for the currency</param>
        /// <exception cref="ArgumentNullException"><paramref name="code"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="code"/> is not 3 characters, not uppercase</exception>
        public Currency(string code, string symbol)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            if (code.Length != 3) throw new ArgumentOutOfRangeException(nameof(code));
            if (code.Any(chr => char.IsLower(chr))) throw new ArgumentOutOfRangeException(nameof(code));

            Code = code;
            Symbol = symbol;
        }

        /// <summary>
        /// Create a currency based on the given ISO 4217 code and the numeric value
        /// </summary>
        /// <param name="code">The ISO 4217 code (uppercase)</param>
        public Currency(string code) : this(code, null)
        {
        }

        /// <summary>
        /// Return the string representation of the currency, by default the Code
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// Get the textual representation of the currency. Allowed formats are:
        /// <list type="bullet">
        /// <item>null: see G</item>
        /// <item>G: General format, returns the Code of the currency</item>
        /// <item>S: returns the Symbol of the currency, when the symbol is null, the code will be returned</item>
        /// </list>
        /// </summary>
        /// <param name="format">Specifies the format of the string representation to return. Allowed formats are:
        /// <list type="bullet">
        /// <item>null: see G</item>
        /// <item>G: General format, returns the Code of the currency</item>
        /// <item>S: returns the Symbol of the currency, when the symbol is null, the code will be returned</item>
        /// </list>
        ///</param>
        /// <returns>The string representation of the Currency for the given format, a FormatException otherwise</returns>
        public string ToString(string format)
        {
            switch (format)
            {
                case null:
                case "G":
                    return Code;
                case "S":
                    return Symbol;
                default:
                    throw new FormatException(Resources.Currency_InvalidFormat);
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Currency) return Equals((Currency)obj);
            return base.Equals(obj);
        }

        /// <summary>
        /// Are the currencies equal?
        /// </summary>
        public bool Equals(Currency other)
        {
            if (other == null) return false;
            return Code.Equals(other.Code, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc/>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <inheritdoc/>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var codeValue = reader.MoveToAttribute("code") ? reader.Value : null;
            var symbolValue = reader.MoveToAttribute("symbol") ? reader.Value : null;


            if (string.IsNullOrEmpty(codeValue))
            {
                Unsafe.AsRef(this) = default;
            }
            else
            {
                Unsafe.AsRef(this) = new Currency(codeValue, symbolValue);
            }
            reader.Skip();
        }

        /// <inheritdoc/>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteAttributeString("code", Code);
            writer.WriteAttributeString("symbol", Symbol);
        }
    }
}
