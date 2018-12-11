using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Formatters
{
    /// <summary>
    /// interface for all phone number formatters
    /// </summary>
    public interface IPhoneNumberFormatter
    {
        /// <summary>
        /// Is this formatter specified for the given country
        /// </summary>
        /// <param name="countryCode">the calling code of the country</param>
        /// <returns>whether the formatter applies</returns>
        bool AppliesTo(string countryCode);

        /// <summary>
        /// Returns a format string for a given phone number and an original format.
        /// This allows to create a format string that depends on the given phone number.
        /// E.g. in the Netherlands the format of the phone number depends on the area code, which can 
        /// be 3 or 4 digits.
        /// </summary>
        /// <param name="phoneNumber">the phone number to format</param>
        /// <returns>the new custom format</returns>
        string GetLocalFormat(PhoneNumber phoneNumber);
        /// <summary>
        /// Returns a format string for a given phone number and an original format.
        /// This allows to create a format string that depends on the given phone number.
        /// E.g. in the Netherlands the format of the phone number depends on the area code, which can 
        /// be 3 or 4 digits.
        /// </summary>
        /// <param name="phoneNumber">the phone number to format</param>
        /// <returns>the new custom format</returns>
        string GetGlobalFormat(PhoneNumber phoneNumber);
    }
}
