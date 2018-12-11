using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Formatters
{
    /// <summary>
    /// Class to wrap logic aroung an <see cref="IPhoneNumberFormatter"/> so that this does not have
    /// to be implemented in every <see cref="IPhoneNumberFormatter"/>
    /// </summary>
    internal class PhoneNumberFormatterWrapper
    {
        private readonly IPhoneNumberFormatter _formatter;

        /// <summary>
        /// create a new wrapper
        /// </summary>
        /// <param name="formatter">the formatter to wrap</param>
        public PhoneNumberFormatterWrapper(IPhoneNumberFormatter formatter)
        {
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        /// <summary>
        /// Wrapper around <see cref="IPhoneNumberFormatter"/> to call the right method
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string GetFormat(PhoneNumber phoneNumber, string format)
        {
            format = string.IsNullOrEmpty(format) ? "G" : format;
            switch (format)
            {
                case "L": return _formatter.GetLocalFormat(phoneNumber);
                case "G":return _formatter.GetGlobalFormat(phoneNumber);
                default: throw new FormatException(nameof(format));
            }
        }


    }
}
