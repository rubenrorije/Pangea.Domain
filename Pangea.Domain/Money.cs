﻿using Pangea.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Pangea.Domain
{
    /// <summary>
    /// Representation of an amount in a given currency
    /// </summary>
    [Serializable]
    public struct Money
        : IEquatable<Money>
        , IXmlSerializable
        , IFormattable
        , IComparable
        , IComparable<Money>
    {
        /// <summary>
        /// The Currency   
        /// </summary>
        public Currency Currency { get; }

        /// <summary>
        /// The actual amount
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// A given amount specified for a given currency
        /// </summary>
        public Money(Currency currency, decimal amount)
        {
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
            Amount = amount;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals((Money)obj);
        }

        /// <summary>
        /// Is the amount and currency equal 
        /// </summary>
        /// <param name="other">The money to compare to</param>
        /// <returns>Whether the currency and amount are equal</returns>
        public bool Equals(Money other)
        {
            return
                Currency == other.Currency &&
                Amount == other.Amount;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Currency?.GetHashCode() ?? 0;
                hash = hash * 23 + Amount.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <inheritdoc/>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <inheritdoc/>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var amountValue = reader.MoveToAttribute("amount") ? reader.Value : null;
            var currencyValue = reader.MoveToAttribute("currency") ? reader.Value : null;


            if (string.IsNullOrEmpty(currencyValue))
            {
                Unsafe.AsRef(this) = default;
            }
            else
            {
                var amount = decimal.Parse(amountValue, CultureInfo.InvariantCulture);
                var currency = Currencies.Find(currencyValue);
                Unsafe.AsRef(this) = new Money(currency, amount);
            }
            reader.Skip();
        }

        /// <inheritdoc/>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteAttributeString("amount", Amount.ToString("G", CultureInfo.InvariantCulture));
            writer.WriteAttributeString("currency", Currency?.Code);
        }

        private NumberFormatInfo GetNumberFormat(IFormatProvider provider, bool useSymbol)
        {
            NumberFormatInfo result;
            if (provider == null)
            {
                result = CultureInfo.CurrentCulture.NumberFormat;
            }
            else if (provider is CultureInfo)
            {
                result = ((CultureInfo)provider).NumberFormat;
            }
            else if (provider is NumberFormatInfo)
            {
                result = ((NumberFormatInfo)provider);
            }
            else
            {
                throw new ArgumentException(Resources.Money_InvalidFormatProvider, nameof(provider));
            }

            result = (NumberFormatInfo)result.Clone();
            result.CurrencySymbol = useSymbol ? Currency.Symbol : Currency.Code;
            return result;
        }

        private string GetFormat(string format)
        {
            if (string.IsNullOrEmpty(format)) return "C2";
            if (format.Contains("N")) return format.Replace("N", "C");
            if (format.Contains("n")) return format.Replace("n", "c");
            if (format.Contains("S")) return format.Replace("S", "C");
            if (format.Contains("s")) return format.Replace("s", "c");
            else return format;
        }

        /// <inheritdoc/>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <inheritdoc/>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (Currency == null) return string.Empty;

            var safeFormat = GetFormat(format);
            var nfi = GetNumberFormat(formatProvider, !format?.Contains("C") ?? true);
            return Amount.ToString(safeFormat, nfi);
        }

        /// <inheritdoc/>
        public static bool operator ==(Money left, Money right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(Money left, Money right) => !(left == right);

        /// <summary>
        /// Negate the amount of money
        /// </summary>
        /// <returns>The negated money amount.</returns>
        public static Money operator -(Money money) => Negate(money);

        /// <summary>
        /// Adds the money objects. 
        /// </summary>
        public static Money operator +(Money lhs, Money rhs) => lhs.Add(rhs);

        /// <summary>
        /// Adds a percentage to the money
        /// </summary>
        public static Money operator +(Money lhs, Percentage percentage) => lhs.Add(percentage);

        /// <summary>
        /// Divide the money into equal parts
        /// </summary>
        public static Money operator /(Money lhs, int times) => lhs.Divide(times);

        /// <summary>
        /// Multiply the money
        /// </summary>
        public static Money operator *(Money lhs, int times) => lhs.Multiply(times);

        /// <summary>
        /// Divide the money into equal parts
        /// </summary>
        public static Money operator /(Money lhs, decimal times) => lhs.Divide(times);

        /// <summary>
        /// Multiply the money
        /// </summary>
        public static Money operator *(Money lhs, decimal times) => lhs.Multiply(times);

        /// <summary>
        /// Multiply the money
        /// </summary>
        public static Money operator *(Money lhs, ExchangeRate rate) => lhs.Multiply(rate);

        /// <summary>
        /// Multiply the money
        /// </summary>
        public static Money operator *(Money lhs, Percentage percentage) => lhs.Multiply(percentage);

        /// /// <summary>
        /// Divide the money into equal parts
        /// </summary>
        public Money Divide(int times) => new Money(Currency, Amount / times);
        
        ///<summary>
        /// Divide the money into equal parts
        /// </summary>
        public Money Divide(decimal times) => new Money(Currency, Amount / times);

        /// <summary>
        /// Multiply the money
        /// </summary>
        public Money Multiply(int times) => new Money(Currency, Amount * times);

        /// <summary>
        /// Multiply the money
        /// </summary>
        public Money Multiply(decimal times) => new Money(Currency, Amount * times);
        
        /// <summary>
        /// Multiply the money with the percentage
        /// </summary>
        public Money Multiply(Percentage percentage) => new Money(Currency, Amount * percentage);

        /// <summary>
        /// Multiply the money with the given exchange rate. 
        /// The result will be the converted money in the other currency
        /// </summary>
        public Money Multiply(ExchangeRate rate)
        {
            if (rate == null)
            {
                throw new ArgumentNullException(nameof(rate));
            }
            else if (this == default)
            {
                return default;
            }
            else if (rate.From == Currency)
            {
                return new Money(rate.To, Amount * rate.Rate);
            }
            else if (rate.To == Currency)
            {
                return new Money(rate.From, Amount / rate.Rate);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(rate), Resources.Money_CannotConvertWithExchangeRate);
            }
        }

        /// <summary>
        /// Negate the amount of money
        /// </summary>
        /// <returns>The negated money amount.</returns>
        public static Money Negate(Money money)
        {
            if (money == default) return money;
            return new Money(money.Currency, -money.Amount);
        }

        /// <summary>
        /// Adds the money objects. 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Money Add(Money other)
        {
            if (this == default) return other;
            else if (other == default) return this;
            else if (Currency != other.Currency) throw new ArgumentOutOfRangeException(nameof(other), Resources.Money_CannotAddDifferentCurrencies);
            else return new Money(Currency, Amount + other.Amount);
        }

        /// <summary>
        /// Adds the money objects. 
        /// </summary>
        public Money Add(Percentage percentage)
        {
            if (this == default) return default;
            else return new Money(Currency, Amount + percentage);
        }

        /// <summary>
        /// Returns the absolute representation of this amount. 
        /// </summary>
        public Money Absolute()
        {
            if (Amount >= 0) return this;
            else return -this;
        }

        /// <summary>
        /// Compares the given objects. Throws exception when the given <paramref name="obj"/> is not of the Money Type
        /// </summary>
        public int CompareTo(object obj)
        {
            return CompareTo((Money)obj);
        }

        /// <summary>
        /// Compare two money objects. When the currencies do not match an exception is thrown.
        /// </summary>
        /// <param name="other">The money to compare to</param>
        /// <exception cref="ArgumentException">The money instances are not of the same currency</exception>
        public int CompareTo(Money other)
        {
            if (Currency != other.Currency) throw new ArgumentOutOfRangeException(nameof(other), Resources.Money_CannotCompareDifferentCurrencies);
            return Amount.CompareTo(other.Amount);
        }

        /// <inheritdoc/>
        public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;

        /// <inheritdoc/>
        public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;

        /// <inheritdoc/>
        public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;

        /// <inheritdoc/>
        public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;

        /// <summary>
        /// Returns a new money object that is rounded to the given number of decimals.
        /// </summary>
        /// <param name="decimals">The number of decimals to round, 0 for integer values, 2 for rounding on cents, etc.</param>
        /// <param name="rounding">The way the amount must be rounded when the amount is midway between numbers. Default = <see cref="MidpointRounding.ToEven"/></param>
        /// <returns>A new instance of money rounded to the given number of decimals</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="decimals"/> is negative</exception>
        public Money Round(int decimals, MidpointRounding rounding = MidpointRounding.ToEven)
        {
            if (decimals < 0) throw new ArgumentOutOfRangeException(nameof(decimals));
            return new Money(Currency, Math.Round(Amount, decimals, rounding));
        }


    }
}
