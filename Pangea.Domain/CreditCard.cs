﻿using Pangea.Domain.Checksums;
using Pangea.Domain.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Pangea.Domain
{
    /// <summary>
    /// A validated object representing a credit card.
    /// </summary>
    public struct CreditCard : IEquatable<CreditCard>
    {
        private readonly string _value;

        /// <summary>
        /// Create a new credit card that is validated using the given card number
        /// </summary>
        /// <param name="cardNumber">the card number in text format with or without spaces that must be validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the number of characters (excluding spaces) is less than 8, more than 19, are no digits, or when the checksum is not correct.</exception>
        public CreditCard(string cardNumber)
        {
            var trimmed = cardNumber?.Replace(" ", "");
            if (!string.IsNullOrEmpty(cardNumber))
            {
                if (trimmed.Length < 8) throw new ArgumentOutOfRangeException("A credit card must be at least 8 characters");
                if (trimmed.Length > 19) throw new ArgumentOutOfRangeException("A credit card must be less than 20 characters");
                if (trimmed.Any(chr => !Char.IsDigit(chr))) throw new ArgumentOutOfRangeException("A credit card can only contain digits or spaces");

                var algorithm = new LuhnChecksumCalculator();
                if (!algorithm.Validate(trimmed)) throw new ArgumentOutOfRangeException("The creditcard is invalid because the checksum is incorrect");
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
            return _value.Equals(other._value);
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
    }
}
