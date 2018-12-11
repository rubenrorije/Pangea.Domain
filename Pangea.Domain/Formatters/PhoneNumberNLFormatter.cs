using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Formatters
{
    /// <summary>
    /// Format the phone number in the dutch format.
    /// </summary>
    public class PhoneNumberNLFormatter : IPhoneNumberFormatter
    {
        /// <inheritdoc/>
        public bool AppliesTo(string countryCode)
        {
            return countryCode.Equals("31");
        }

        /// <inheritdoc />
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            var phoneNumber = (PhoneNumber)arg;

            if (format?.ToUpper() == "L")
            {
                if (phoneNumber.Trimmed.StartsWith("6"))
                {
                    return "06-" + phoneNumber.Trimmed.Substring(1);
                }
            }
            if (format?.ToUpper() == "G")
            {
                if (phoneNumber.Trimmed.StartsWith("6"))
                {
                    return "+31 6 " + phoneNumber.Trimmed.Substring(1);
                }
            }
            return phoneNumber.ToString();
        }

        /// <inheritdoc />
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            return null;
        }
    }
}
