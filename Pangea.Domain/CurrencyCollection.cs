using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// A holder containing all registered currencies in the implementation
    /// </summary>
    public class CurrencyCollection : IEnumerable<Currency>
    {
        /// <summary>
        /// A private field to store a way to retrieve the Currencies instance.
        /// This allows the end user to store the Currencies instance for instance in an IoC-container
        /// </summary>
        private static Func<CurrencyCollection> _instanceProvider;


        /// <summary>
        /// Set the way to retrieve the global instance of the registered Currencies.
        /// A func is used to allow the end user (developer) to store the actual instance within the 
        /// IoC-container if applicable. 
        /// </summary>
        /// <param name="functionToRetrieveTheInstance">The way to retrieve the instance</param>
        public static void SetProvider(Func<CurrencyCollection> functionToRetrieveTheInstance)
        {
            _instanceProvider = functionToRetrieveTheInstance;
        }

        /// <summary>
        /// Set a default provider
        /// </summary>
        public static void SetEmptyProvider()
        {
            var instance = new CurrencyCollection();
            SetProvider(() => instance);
        }

        /// <summary>
        /// Remove the registered provider
        /// </summary>
        public static void ClearProvider()
        {
            SetProvider(null);
        }

        /// <summary>
        /// Is a provider function registered?
        /// </summary>
        public static bool ProviderIsRegistered => _instanceProvider != null;


        /// <summary>
        /// The registered instance will be returned using the provider function.
        /// </summary>
        public static CurrencyCollection Instance
        {
            get
            {
                if (_instanceProvider == null)
                {
                    throw new InvalidOperationException(
                      "The provider function is not set. Use the SetProvider-function to register an instance. " +
                      "Probably you want to create a Currencies instance once for your application in the bootstrapping code " +
                      "and register it in your IoC-Container. After that you want to " +
                      "call the SetProvider with a function to retrieve the instance from your IoC-container. " +
                      "When you do not use an IoC-container in your application, you can create a new Currencies instance and use the " +
                      "following call: Currencies.SetProvider(() => instance);");
                }
                return _instanceProvider();
            }
        }

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
            foreach (var currency in currencies)
            {
                Add(currency);
            }
        }

        /// <summary>
        /// Add a number of currencies at once.
        /// </summary>
        /// <param name="currencies">Teh currencies to add</param>
        public void AddRange(params Currency[] currencies)
        {
            AddRange(currencies.AsEnumerable());
        }

        /// <summary>
        /// Try to find the Currency within the registered currencies by the given code.
        /// </summary>
        /// <param name="code">the ISO 4217 code</param>
        /// <returns>Either the currency that is found, or null when the currency could not be found</returns>
        public static Currency Find(string code)
        {
            return Instance._currencies[code];
        }

        /// <summary>
        /// Try to find the Currency within the registered currencies by the given code.
        /// </summary>
        /// <param name="numeric">the numeric code of the currency</param>
        /// <returns>Either the currency that is found, or null when the currency could not be found</returns>
        public static Currency Find(int numeric)
        {
            return Instance._currencies.Values.FirstOrDefault(currency => currency.Numeric == numeric);
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
