using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// An exchange rate to convert a money amount in the 'From' currency to an equivalent amount 
    /// in the 'To' currency.
    /// </summary>
    public sealed class ExchangeRate
        : IFormattable
    {
        /// <summary>
        /// The currency that will be used to convert a money amount from
        /// </summary>
        public Currency From { get; }
        /// <summary>
        /// The currency that will be used to convert a money amount to
        /// </summary>
        public Currency To { get; }

        /// <summary>
        /// The actual exchange rate to convert 'From' to 'To'
        /// </summary>
        public decimal Rate { get; }

        /// <summary>
        /// Create an exchange rate for the given currencies.
        /// </summary>
        /// <param name="from">the currency that will be used to convert a money amount from</param>
        /// <param name="to">the currency that will be used to convert a money amount to</param>
        /// <param name="rate">The actual rate to convert the currencies</param>
        /// <exception cref="ArgumentNullException">The <paramref name="from"/> or <paramref name="to"/> currency is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="rate"/>is non-positive</exception>
        /// <exception cref="ArgumentException">The <paramref name="from"/> is equal to <paramref name="to"/></exception>
        public ExchangeRate(Currency from, Currency to, decimal rate)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (from.Equals(to)) throw new ArgumentException("It is not possible to create an exchange rate within the same currency");
            if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate));

            From = from;
            To = to;
            Rate = rate;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return the text representation for the exchange rate, based on the given format.
        /// The format of the result will be the from Currency followed by a /, the To Currency, a space, and then
        /// the actual rate formatted based on the given format and -provider. An example is EUR/USD 1.14133
        /// </summary>
        /// <param name="format">The format to print the actual rate</param>
        /// <returns>FROM/TO rate</returns>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Invert the given exchange rate. That is, From will be To and To will be From. The exchange rate will 
        /// be inverted as well
        /// </summary>
        /// <returns>The inverted exchange rate</returns>
        public ExchangeRate Invert()
        {
            return
                new ExchangeRate
                (
                    To,
                    From,
                    1m / Rate
                );
        }

        /// <summary>
        /// return the text representation for the exchange rate, based on the given format and -provider.
        /// The format of the result will be the from Currency followed by a /, the To Currency, a space, and then
        /// the actual rate formatted based on the given format and -provider. An example is EUR/USD 1.14133
        /// </summary>
        /// <param name="format">The format to print the actual rate</param>
        /// <param name="formatProvider">the formatprovider to print the actual rate</param>
        /// <returns>FROM/TO rate</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{From.Code}/{To.Code} " + Rate.ToString(format, formatProvider);
        }


        /// <summary>
        /// Calculate the amount in the other currency. Assumes that the amount given is in the 'From' currency
        /// </summary>
        /// <param name="amount">The amount in the from Currency</param>
        /// <param name="rate">the exchange rate</param>
        /// <returns>The converted amount in the To currency</returns>
        public static decimal operator *(decimal amount, ExchangeRate rate)
        {
            return rate.Multiply(amount);
        }

        /// <summary>
        /// Calculate the amount in the other currency. Assumes that the amount given is in the 'From' currency
        /// </summary>
        /// <param name="amount">The amount in the from Currency</param>
        /// <returns>The converted amount in the To currency</returns>
        public decimal Multiply(decimal amount)
        {
            return amount * Rate;
        }

        /// <summary>
        /// Calculate the amount in the other currency. Assumes that the amount given is in the 'To' currency
        /// </summary>
        /// <param name="amount">The amount in the TO Currency</param>
        /// <param name="rate">the exchange rate FROM->TO</param>
        /// <returns>The converted amount in the FROM currency</returns>
        public static decimal operator /(decimal amount, ExchangeRate rate)
        {
            return rate.Divide(amount);
        }

        /// <summary>
        /// Calculate the amount in the other currency. Assumes that the amount given is in the 'To' currency
        /// </summary>
        /// <param name="amount">The amount in the TO Currency</param>
        /// <returns>The converted amount in the FROM currency</returns>
        public decimal Divide(decimal amount)
        {
            return amount / Rate;
        }

        /// <summary>
        /// Convert the amount in the currency to the other currency. This allows to convert the amount even when
        /// the rate is reversed.
        /// </summary>
        /// <param name="amount">The amount to be converted</param>
        /// <param name="currency">The currency of the amount. Note that when the currency given is not one of the 
        /// two currencies in the exchane rate an exception will be thrown.</param>
        /// <returns>The converted amount</returns>
        public decimal Convert(decimal amount, Currency currency)
        {
            if (currency == null) throw new ArgumentNullException(nameof(currency));

            if (currency.Equals(From))
            {
                return amount * this;
            }
            else if (currency.Equals(To))
            {
                return amount / this;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(currency));
            }
        }
    }
}
