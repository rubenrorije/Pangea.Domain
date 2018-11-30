using Pangea.Domain.Checksums;
using Pangea.Domain.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// A number representing a credit card.
    /// </summary>
    public struct CreditCard : IEquatable<CreditCard>
    {
        private readonly string _value;

        /// <summary>
        /// Create a new credit card that is validated using the given card number
        /// </summary>
        /// <param name="cardNumber"></param>
        public CreditCard(string cardNumber)
        {
            if (!string.IsNullOrEmpty(cardNumber))
            {
                if (cardNumber.Length < 8) throw new ArgumentOutOfRangeException("A credit card must be at least 8 characters");
                if (cardNumber.Length > 19) throw new ArgumentOutOfRangeException("A credit card must be less than 20 characters");
                if (cardNumber.Any(chr => !Char.IsDigit(chr) && chr != ' ')) throw new ArgumentOutOfRangeException("A credit card can only contain digits or spaces");

                var algorithm = new LuhnChecksumCalculator();
                if (!algorithm.Validate(cardNumber)) throw new ArgumentOutOfRangeException("The creditcard is invalid because the checksum is incorrect");
            }
            _value = cardNumber?.Replace(" ", "");
        }

        /// <inheritdoc/>
        public bool Equals(CreditCard other)
        {
            return _value.Equals(other._value);
        }

        /// <inheritdoc/>
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
            Long = 8,
            /// <summary>
            /// The issuer identifier is 6 characters long
            /// </summary>
            Short = 6
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

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_value)) return _value;

            var grouped = _value.Page(4).Select(group => new string(group.ToArray()));

            return string.Join(" ", grouped);
        }
    }
}
