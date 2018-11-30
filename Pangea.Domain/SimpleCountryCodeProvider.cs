using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// A simple country code provider that only determines which country codes exist.
    /// </summary>
    public class SimpleCountryCodeProvider : BaseCountryCodeProvider
    {
        private readonly IDictionary<string, int> _countryCodes;
        /// <summary>
        /// the number of digits in the country code that is the largest
        /// </summary>
        protected override int MaxCountryCodeLength { get; }

        /// <summary>
        /// Create a provider with an empty dictionary. Items can be added later
        /// </summary>
        public SimpleCountryCodeProvider() : this(new Dictionary<string, int>())
        {

        }


        /// <summary>
        /// Create a country calling code provider based on the given dictionary
        /// </summary>
        /// <param name="countryCodes">a dictionary containing all the country codes that are registered</param>
        public SimpleCountryCodeProvider(IDictionary<string, int> countryCodes)
        {
            _countryCodes = countryCodes ?? throw new ArgumentNullException(nameof(countryCodes));

            var maxValue = countryCodes.Values.DefaultIfEmpty().Max();
            if (maxValue == 0) MaxCountryCodeLength = 0;
            else MaxCountryCodeLength = (int)Math.Pow(maxValue, 1 / 10.0) + 1;
        }
        /// <summary>
        /// Check a part of the phone number to see whether it is a country code
        /// </summary>
        /// <param name="possibleCountryCode">a part of a phone number to check</param>
        /// <returns>either the country code, or null when no country code could be found</returns>
        protected override int? Check(int possibleCountryCode)
        {
            if (_countryCodes.Values.Contains(possibleCountryCode)) return possibleCountryCode;
            return null;
        }

        /// <inheritdoc />
        public override int? GetCountryCallingCodeFor(string isoTwoLetterCountryName)
        {
            if (_countryCodes.TryGetValue(isoTwoLetterCountryName, out int result)) return result;
            return null;
        }

        /// <summary>
        /// Add a single country. 
        /// </summary>
        /// <param name="isoTwoLetterCountryName">the country ISO two leter name</param>
        /// <param name="code">the country calling code</param>
        public void Add(string isoTwoLetterCountryName, int code)
        {
            _countryCodes.Add(isoTwoLetterCountryName, code);
        }
    }
}
