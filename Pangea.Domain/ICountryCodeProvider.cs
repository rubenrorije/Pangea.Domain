using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Extract the country code of the phone number
    /// </summary>
    public interface ICountryCodeProvider
    {
        /// <summary>
        /// Return the country calling code when it can be found.
        /// </summary>
        /// <param name="phoneNumber">The part of the phone number that contains the numbers</param>
        /// <returns>Either the country calling code or null</returns>
        int? GetCountryCallingCodeFrom(string phoneNumber);

        /// <summary>
        /// Resolves the country calling code for the given country
        /// </summary>
        /// <param name="isoTwoLetterCountryName">The 'ISO 3166-1 alpha-2' name of the country</param>
        /// <returns>The country calling code. For instance when given NL the result will be 31</returns>
        int? GetCountryCallingCodeFor(string isoTwoLetterCountryName);
    }
}
