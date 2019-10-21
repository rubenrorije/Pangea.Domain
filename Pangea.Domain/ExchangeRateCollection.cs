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
    public class ExchangeRateCollection : ICollection<ExchangeRate>, IExchangeRateProvider
    {
        private readonly List<ExchangeRate> _exchangeRates;

        /// <summary>
        /// The number of elements in this collection
        /// </summary>
        public int Count => _exchangeRates.Count;

        /// <summary>
        /// Whether the collection is readonly (false)
        /// </summary>
        public bool IsReadOnly => false;


        /// <summary>
        /// Create an Exchange rate collection.
        /// </summary>
        public ExchangeRateCollection()
        {
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
        /// Returns the exchange rate for the given currencies when it exists. 
        /// When the inverse exists and the rates are the same both ways the rate is inverted and returned.
        /// When a result is found, the 
        /// </summary>
        public ExchangeRate this[Currency from, Currency to]
        {
            get
            {
                return
                    TryGet(from, to) ??
                    throw new KeyNotFoundException(Resources.ExchangeRates_CannotFindExchangeRateForCurrencies);
            }
        }

        /// <summary>
        /// Try to get the exchange rate SOURCE->TARGET
        /// </summary>
        /// <param name="sourceCurrency">The currency to convert from</param>
        /// <param name="targetCurrency">The currency to convert to</param>
        /// <returns>Either the exchange rate, or an exception</returns>
        public ExchangeRate TryGet(Currency sourceCurrency, Currency targetCurrency)
        {
            if (sourceCurrency == null) throw new ArgumentNullException(nameof(sourceCurrency));
            if (targetCurrency == null) throw new ArgumentNullException(nameof(targetCurrency));

            return _exchangeRates.FirstOrDefault(rate => rate.From == sourceCurrency && rate.To == targetCurrency);
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

            return _exchangeRates.Any(rate => rate.From == from && rate.To == to);
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
