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
    public class CurrencyCollection : IEnumerable<Currency>
    {

        private Dictionary<string, Currency> _currencies;

        /// <summary>
        /// Create a new Currency holder instance
        /// </summary>
        public CurrencyCollection()
        {
            _currencies = new Dictionary<string, Currency>();
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
        /// Try to find the Currency within the registered currencies by the given code.
        /// </summary>
        /// <param name="code">the ISO 4217 code</param>
        /// <returns>Either the currency that is found, or null when the currency could not be found</returns>
        public Currency Find(string code)
        {
            Currency result;
            if (_currencies.TryGetValue(code, out result)) return result;
            return null;
        }

        /// <summary>
        /// Get the currency based on the given code, a <see cref="KeyNotFoundException"/> when not found.
        /// </summary>
        public Currency this[string code] => _currencies[code];
        

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
