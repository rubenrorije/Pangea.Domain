using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Singleton for holding all registered currencies
    /// </summary>
    public static class Currencies
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
        /// The registered instance will be returned using the provider function.
        /// </summary>
        private static CurrencyCollection Instance
        {
            get
            {
                if (_instanceProvider == null)
                {
                    SetRegionInfoProvider();
                }
                return _instanceProvider();
            }
        }

        /// <summary>
        /// Initialize a new CurrencyCollection with the given currencies.
        /// These will be registered as a singleton as well as returned directly
        /// </summary>
        /// <param name="currencies">The currencies to register</param>
        /// <returns>The created Currency Collection</returns>
        public static CurrencyCollection Initialize(params Currency[] currencies)
        {
            var instance = new CurrencyCollection();
            instance.AddRange(currencies);
            SetProvider(() => instance);
            return instance;
        }

        /// <summary>
        /// Set the provider to use the region info's from standard .Net
        /// </summary>
        public static CurrencyCollection SetRegionInfoProvider()
        {
            var currencies =
                CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(culture => !culture.IsNeutralCulture)
                .Where(culture => culture.LCID != CultureInfo.InvariantCulture.LCID)
                .Select(culture => new RegionInfo(culture.LCID))
                .Select(region => new Currency(region.ISOCurrencySymbol, region.CurrencySymbol))
                .Distinct()
                .ToArray();

            var instance = new CurrencyCollection();
            instance.AddRange(currencies);
            SetProvider(() => instance);

            return instance;
        }

        /// <summary>
        /// Try to find the Currency within the registered currencies by the given code.
        /// </summary>
        /// <param name="code">the ISO 4217 code</param>
        /// <returns>Either the currency that is found, or null when the currency could not be found</returns>
        public static Currency Find(string code)
        {
            return Instance.Find(code);
        }
    }
}
