using Pangea.Domain.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Holds a number of exchange rates to do easy conversions between currencies
    /// </summary>
    public class ExchangeRateCollection : ICollection<ExchangeRate>
    {
        private readonly List<ExchangeRate> _exchangeRates;

        /// <summary>
        /// The type of conversion that is used in this instance
        /// </summary>
        public ExchangeRateConversionType ConversionType { get; }

        /// <summary>
        /// The number of elements in this collection
        /// </summary>
        public int Count => _exchangeRates.Count;

        /// <summary>
        /// Whether the collection is readonly (false)
        /// </summary>
        public bool IsReadOnly => false;


        /// <summary>
        /// Create an Exchange rate collection and uses the same rate both ways
        /// </summary>
        public ExchangeRateCollection() : this(ExchangeRateConversionType.SameRateBothWays)
        {
        }

        /// <summary>
        /// Create an Exchange rate collection
        /// </summary>
        /// <param name="conversionType">Specifies the type of conversion that is used</param>
        public ExchangeRateCollection(ExchangeRateConversionType conversionType)
        {
            ConversionType = conversionType;
            _exchangeRates = new List<ExchangeRate>();
        }
        /// <summary>
        /// Adds an exchange rate to the end of this collection. 
        /// 
        /// </summary>
        /// <param name="rate">the rate to add</param>
        /// <exception cref="ArgumentNullException">When the <paramref name="rate"/> is null</exception>
        /// <exception cref="ArgumentException">When a <paramref name="rate"/> for the same currencies is already added</exception>
        public void Add(ExchangeRate rate)
        {
            if (rate == null) throw new ArgumentNullException(nameof(rate));
            if (Contains(rate.From, rate.To)) throw new ArgumentException(Resources.ExchangeRateCollection_RateIsAlreadyAdded, nameof(rate));
            _exchangeRates.Add(rate);
        }

        /// <summary>
        /// Returns the exchange rate for the given currencies when it exists
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public ExchangeRate this[Currency from, Currency to]
        {
            get
            {
                if (from == null) throw new ArgumentNullException(nameof(from));
                if (to == null) throw new ArgumentNullException(nameof(to));

                var result = _exchangeRates.FirstOrDefault(rate => rate.From == from && rate.To == to);
                if (result != null)
                {
                    return result;
                }
                else if (ConversionType == ExchangeRateConversionType.SameRateBothWays)
                {
                    return _exchangeRates.FirstOrDefault(rate => rate.To == from && rate.From == to);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(Resources.ExchangeRateCollection_CannotFindExchangeRateForCurrencies);
                }
            }
        }

        /// <summary>
        /// Returns the exchange rate for the given currencies.
        /// </summary>
        /// <param name="from">The currency to convert from</param>
        /// <param name="to">The currency to convert to</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">When either currency is null</exception>
        public bool Contains(Currency from, Currency to)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));

            if (_exchangeRates.Any(rate => rate.From == from && rate.To == to))
            {
                return true;
            }
            else if (ConversionType == ExchangeRateConversionType.SameRateBothWays)
            {
                return _exchangeRates.Any(rate => rate.To == from && rate.From == to);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Returns an enumerator that iterates through the exchange rates
        /// </summary>
        public IEnumerator<ExchangeRate> GetEnumerator() => _exchangeRates.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        /// <inheritdoc/>
        public void Clear() => _exchangeRates.Clear();

        /// <inheritdoc/>
        public bool Contains(ExchangeRate item) => _exchangeRates.Contains(item);

        /// <inheritdoc/>
        public void CopyTo(ExchangeRate[] array, int arrayIndex) => _exchangeRates.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public bool Remove(ExchangeRate item) => _exchangeRates.Remove(item);
    }
}
