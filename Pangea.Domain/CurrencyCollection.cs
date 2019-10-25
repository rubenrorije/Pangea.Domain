using Pangea.Domain.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// A holder containing all registered currencies in the implementation
    /// </summary>
    internal class CurrencyCollection
        : IEnumerable<Currency>
        , IReadOnlyCollection<Currency>
    {

        private Dictionary<string, Currency> _currencies;

        /// <summary>
        /// Create a new Currency holder instance
        /// </summary>
        public CurrencyCollection() : this(Enumerable.Empty<Currency>())
        {
        }

        /// <summary>
        /// Create a new Currency holder instance initialized with the given currencies
        /// </summary>
        /// <param name="currencies">A list of currencties to use</param>
        public CurrencyCollection(IEnumerable<Currency> currencies)
        {
            _currencies = currencies.ToDictionary(cur => cur.Code, cur => cur);
        }

        /// <summary>
        /// The total number of uniquely registered currencies.
        /// </summary>
        public int Count => _currencies.Count;

        /// <summary>
        /// Add a new currency to the registered currencies. 
        /// When the currency is already registered, nothing happens
        /// </summary>
        /// <param name="currency">The Currency to add</param>
        public void Add(Currency currency)
        {
            if (currency == null) throw new ArgumentNullException(nameof(currency));

            if (!_currencies.ContainsKey(currency.Code))
            {
                _currencies.Add(currency.Code, currency);
            }
        }

        /// <summary>
        /// Add a number of currencies at once.
        /// </summary>
        /// <param name="currencies">the currencies to add</param>
        public void AddRange(IEnumerable<Currency> currencies)
        {
            if (currencies == null) throw new ArgumentNullException(nameof(currencies));
            foreach (var currency in currencies)
            {
                Add(currency);
            }
        }

        /// <summary>
        /// Add a number of currencies at once.
        /// </summary>
        /// <param name="currencies">The currencies to add</param>
        public void AddRange(params Currency[] currencies)
        {
            AddRange(currencies.AsEnumerable());
        }

        /// <summary>
        /// Remove a registered currency
        /// </summary>
        /// <param name="currency">The currency to remove</param>
        public void Remove(Currency currency)
        {
            if (currency == null) throw new ArgumentNullException(nameof(currency));
            _currencies.Remove(currency.Code);
        }

        /// <summary>
        /// Return a list of currencies.
        /// </summary>
        public IEnumerator<Currency> GetEnumerator()
        {
            return _currencies.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
