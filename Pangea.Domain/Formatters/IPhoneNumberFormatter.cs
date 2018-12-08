using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Formatters
{
    /// <summary>
    /// interface for all phone number formatters
    /// </summary>
    public interface IPhoneNumberFormatter : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// Is this formatter specified for the given country
        /// </summary>
        /// <param name="countryCode">the calling code of the country</param>
        /// <returns>whether the formatter applies</returns>
        bool AppliesTo(string countryCode);
    }
}
