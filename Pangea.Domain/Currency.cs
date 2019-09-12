using Pangea.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Representation of a currency
    /// </summary>
    public sealed class Currency 
        : IEquatable<Currency> 
    {
        /// <summary>
        /// the ISO 4217 code of the given currency
        /// </summary>
        public string Code { get; }
     
        /// <summary>
        /// The numeric code for the given currency
        /// </summary>
        public int Numeric { get; }

        /// <summary>
        /// Optional symbol for the given currency
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Create a currency based on the given ISO 4217 code and the numeric value
        /// </summary>
        /// <param name="code">The ISO 4217 code (uppercase)</param>
        /// <param name="numeric">the numeric value</param>
        /// <param name="symbol">the (optional) symbol for the currency</param>
        /// <exception cref="ArgumentNullException"><paramref name="code"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="code"/> is not 3 characters, not uppercase, or <paramref name="numeric"/> is 0 or less</exception>
        public Currency(string code, int numeric, string symbol)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            if (code.Length != 3) throw new ArgumentOutOfRangeException(nameof(code));
            if (code.Any(chr => Char.IsLower(chr))) throw new ArgumentOutOfRangeException(nameof(code));
            if (numeric < 0) throw new ArgumentOutOfRangeException(nameof(numeric));

            Code = code;
            Numeric = numeric;
            Symbol = symbol;
        }

        /// <summary>
        /// Create a currency based on the given ISO 4217 code and the numeric value
        /// </summary>
        /// <param name="code">The ISO 4217 code (uppercase)</param>
        /// <param name="numeric">the numeric value</param>
        public Currency(string code, int numeric) : this(code, numeric, null)
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
        /// <item>N: returns the Numeric of the currency</item>
        /// </list>
        /// </summary>
        /// <param name="format">Specifies the format of the string representation to return. Allowed formats are:
        /// <list type="bullet">
        /// <item>null: see G</item>
        /// <item>G: General format, returns the Code of the currency</item>
        /// <item>S: returns the Symbol of the currency, when the symbol is null, the code will be returned</item>
        /// <item>N: returns the Numeric of the currency</item>
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
                case "N":
                    return Numeric.ToString(CultureInfo.InvariantCulture);
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
    }
}
