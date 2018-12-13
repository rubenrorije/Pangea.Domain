using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pangea.Domain.Formatters
{
    /// <summary>
    /// Aggregate multiple formatters in one instance
    /// </summary>
    public class PhoneNumberAggregateFormatter : IPhoneNumberFormatter, IEnumerable<IPhoneNumberFormatter>
    {
        private readonly List<IPhoneNumberFormatter> _formatters;

        /// <summary>
        /// Aggregate multiple formatters in one instance
        /// </summary>
        public PhoneNumberAggregateFormatter(params IPhoneNumberFormatter[] formatters)
        {
            if (formatters.Any(fmt => fmt == null)) throw new ArgumentNullException(nameof(formatters));

            _formatters = formatters.ToList();
        }

        /// <summary>
        /// One of the inner formatters applies for this country code
        /// </summary>
        /// <param name="countryCode">The country code to check</param>
        /// <returns>Whether one of the formatters applies to this country code</returns>
        public bool AppliesTo(int countryCode)
        {
            return _formatters.Any(fmt => fmt.AppliesTo(countryCode));
        }

        /// <summary>
        /// Add a formatter
        /// </summary>
        public void Add(IPhoneNumberFormatter formatter)
        {
            _formatters.Add(formatter);
        }


        ///`<inheritdoc/>
        public IEnumerator<IPhoneNumberFormatter> GetEnumerator()
        {
            return _formatters.GetEnumerator();
        }

        /// <summary>
        /// Returns the global format for this phone number based on the first formatter that applies. <c>null</c> otherwise
        /// </summary>
        public string GetGlobalFormat(PhoneNumber phoneNumber)
        {
            return Find(phoneNumber)?.GetGlobalFormat(phoneNumber);
        }

        /// <summary>
        /// Returns the local format for this phone number based on the first formatter that applies. <c>null</c> otherwise
        /// </summary>
        public string GetLocalFormat(PhoneNumber phoneNumber)
        {
            return Find(phoneNumber)?.GetLocalFormat(phoneNumber);
        }

        private IPhoneNumberFormatter Find(PhoneNumber phoneNumber)
        {
            if (phoneNumber.CountryCode == null) return null;
            return _formatters.FirstOrDefault(fmt => fmt.AppliesTo(phoneNumber.CountryCode.Value));
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
