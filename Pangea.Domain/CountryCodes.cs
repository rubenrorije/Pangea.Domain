using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// A class holding an instance to a country code provider to retrieve country codes
    /// based on a phone number
    /// </summary>
    public class CountryCodes
    {
        private static Func<ICountryCodeProvider> _instanceProvider;

        /// <summary>
        /// The registered instance will be returned using the provider function.
        /// </summary>
        public static ICountryCodeProvider Instance
        {
            get { return _instanceProvider?.Invoke(); }
        }

        /// <summary>
        /// Set the way to resolve country codes. This is used to split the country code, and thereby the country, from 
        /// the phone number. A function is used so that you can resolve the country code provider for instance from your IoC container.
        /// </summary>
        /// <param name="countryCodeProviderFunc">a func to get the country code provider to be used to get the country code.</param>
        public static void SetProvider(Func<ICountryCodeProvider> countryCodeProviderFunc)
        {
            _instanceProvider =
                countryCodeProviderFunc ??
                throw new ArgumentNullException(nameof(countryCodeProviderFunc));
        }

        /// <summary>
        /// Remove the currently registered provider
        /// </summary>
        public static void ClearProvider()
        {
            SetProvider(() => null);
        }
    }
}
