using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// An abstract implementation of an <see cref="ICountryCodeProvider"/>. 
    /// This class will handle the way to check for country codes based on a given phone number.
    /// The actual implementation of the check can be implemented in the overriden <see cref="Check(int)"/> function.
    /// </summary>
    public abstract class BaseCountryCodeProvider : ICountryCodeProvider
    {
        /// <summary>
        /// Try to retrieve the country code from the part of the phone number
        /// </summary>
        /// <param name="phoneNumber">The phone number to find the country code for</param>
        /// <returns>either the country code or null when no country code could be found</returns>
        public int? GetCountryCallingCodeFrom(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return null;
            var trimmed = phoneNumber.Replace(" ", "").Substring(0, Math.Min(phoneNumber.Length, MaxCountryCodeLength));
            var value = int.Parse(trimmed, CultureInfo.InvariantCulture);
            return GetCountryCallingCodeIteratively(value);
        }

        private int? GetCountryCallingCodeIteratively(int part)
        {
            var value = part;
            while (value > 0)
            {
                var result = Check(value);
                if (result != null) return result;
                value = value / 10;
            }
            return null;
        }

        /// <summary>
        /// The max number of characters in the largest country code. This improves the performance by eliminating the 
        /// need of checking for country code (sizes) that do not exist 
        /// </summary>
        protected abstract int MaxCountryCodeLength { get; }

        /// <summary>
        /// The actual implementation of finding the country code. During the <see cref="GetCountryCallingCodeFrom(string)"/>
        /// this function will be called starting with an integer that represents the first characters of the phone number.
        /// When it does not find any country code, it will continue with a character less.
        /// </summary>
        /// <param name="phoneNumberPart">The number representation of the part of the phone number that might be a country code</param>
        /// <returns>The country code when found, null otherwise</returns>
        protected abstract int? Check(int phoneNumberPart);

        /// <summary>
        /// Try to retrieve the country calling code from the two letter iso country name
        /// </summary>
        /// <param name="isoTwoLetterCountryName">the country</param>
        /// <returns>The country calling code when found, <c>null</c> otherwise</returns>
        public abstract int? GetCountryCallingCodeFor(string isoTwoLetterCountryName);
    }
}
