using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// The exchange rate at a given point in time
    /// </summary>
    public sealed class ExchangeRateAt
        : IFormattable
    {
        /// <summary>
        /// The date for the exchange rate
        /// </summary>
        public DateTime Date { get; }
        /// <summary>
        /// the innner exchange rate
        /// </summary>
        private readonly ExchangeRate _innerRate;

        /// <summary>
        /// The currency to convert from
        /// </summary>
        public Currency From => _innerRate.From;
        /// <summary>
        /// The currency to convert to
        /// </summary>
        public Currency To => _innerRate.To;
        /// <summary>
        /// The actual exchange rate
        /// </summary>
        public decimal Rate => _innerRate.Rate;

        /// <summary>
        /// Create an exchange rate for the given date
        /// </summary>
        /// <param name="date">The date of the exchange rate</param>
        /// <param name="rate">the rate</param>
        public ExchangeRateAt(DateTime date, ExchangeRate rate)
        {
            if (rate == null) throw new ArgumentNullException(nameof(rate));
            if (date == DateTime.MinValue) throw new ArgumentOutOfRangeException(nameof(date));
            Date = date;
            _innerRate = rate;
        }

        /// <summary>
        /// Create an exchange rate for the given date
        /// </summary>
        /// <param name="date">the date </param>
        /// <param name="from">the currency to convert from</param>
        /// <param name="to">the currency to convert to</param>
        /// <param name="rate">the actual rate</param>
        public ExchangeRateAt(DateTime date, Currency from, Currency to, decimal rate) : this(date, new ExchangeRate(from, to, rate))
        { }

        /// <summary>
        /// Calculate the amount in the other currency. Assumes that the amount given is in the 'From' currency
        /// </summary>
        /// <param name="amount">the amount to convert</param>
        /// <param name="rate">the exchange rate</param>
        /// <returns>the converted amount</returns>
        public static decimal operator *(decimal amount, ExchangeRateAt rate)
        {
            return amount * rate._innerRate;
        }

        /// <summary>
        /// Calculate the amount in the other currency. Assumes that the amount given is in the 'To' currency
        /// </summary>
        /// <param name="amount">The amount in the TO Currency</param>
        /// <param name="rate">the exchange rate FROM->TO</param>
        /// <returns>The converted amount in the FROM currency</returns>
        public static decimal operator /(decimal amount, ExchangeRateAt rate)
        {
            return amount / rate._innerRate;
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
            return _innerRate.Convert(amount, currency);
        }

        /// <summary>
        /// return the textual representation of the exchange rate on the given date, using the default formats
        /// </summary>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// return the textual representation of the exchange rate on the given date
        /// </summary>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// return the textual representation of the exchange rate on the given date.
        /// When a format is given, both parts (for the date and the number) must be specified, separated by a |.
        /// 
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == "G") format = null;

            var formats = ExtractFormats(format);

            return Date.ToString(formats.Item1, formatProvider) + ": " + _innerRate.ToString(formats.Item2, formatProvider);
        }
        private static Tuple<string, string> ExtractFormats(string format)
        {
            if (format == null) return new Tuple<string, string>("d", null);

            var splitted = format.Split("|".ToCharArray());
            return Tuple.Create(splitted[0], splitted[1]);
        }
    }
}
