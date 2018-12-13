using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Pangea.Domain.Formatters
{
    /// <summary>
    /// Abstract base class to have logic for the different country formatters
    /// </summary>
    public abstract class BasePhoneNumberCountryFormatter
    {
        private readonly int _countryCode;

        /// <summary>
        /// Create a base phone number country formatter
        /// </summary>
        /// <param name="countryCode">The country calling code to which this formatter applies</param>
        public BasePhoneNumberCountryFormatter(int countryCode)
        {
            _countryCode = countryCode;
        }

        /// <summary>
        /// Does this formatter apply to the given country calling code.
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public virtual bool AppliesTo(string countryCode)
        {
            return _countryCode.ToString(CultureInfo.InvariantCulture).Equals(countryCode, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Return the global format of a phone number. By default this will be the partial format prepended with '+C '
        /// </summary>
        /// <param name="phoneNumber">the phone number to format</param>
        /// <returns>the new format</returns>
        public virtual string GetGlobalFormat(PhoneNumber phoneNumber)
        {
            var partial = GetPartialFormat(phoneNumber);
            if (partial == null) return null;
            else return "+C " + partial;
        }

        /// <summary>
        /// Return the local format of a phone number. By default this will be the partial format prepended with '0'
        /// </summary>
        /// <param name="phoneNumber">the phone number to format</param>
        /// <returns>the new format</returns>
        public virtual string GetLocalFormat(PhoneNumber phoneNumber)
        {
            var partial = GetPartialFormat(phoneNumber);
            if (partial == null) return null;
            else return "0" + partial;
        }

        /// <summary>
        /// Get the partial format for this phone number, to be implemented by inherited classes when the 
        /// global/local format of the phone number only differs for the start, '+C ' or '0' respectively
        /// </summary>
        /// <param name="phoneNumber">The phone number to format</param>
        /// <returns>The partial format that will be prepended with a local/global prefix</returns>
        protected virtual string GetPartialFormat(PhoneNumber phoneNumber)
        {
            return null;
        }

        /// <summary>
        /// Returns whether the phone number starts with the given number
        /// </summary>
        /// <param name="phoneNumber">The phone number to check</param>
        /// <param name="number">The number that should be the first character of the phone number</param>
        public static bool StartsWith(PhoneNumber phoneNumber, int number)
        {
            return phoneNumber.Trimmed.StartsWith(number.ToString(CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
        }

    }

}
