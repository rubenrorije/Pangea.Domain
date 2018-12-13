using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Formatters
{
    /// <summary>
    /// Format Belgium (32) phone numbers
    /// </summary>
    public class PhoneNumberBEFormatter : BasePhoneNumberCountryFormatter, IPhoneNumberFormatter
    {
        /// <summary>
        /// Format Belgium (32) phone numbers
        /// </summary>
        public PhoneNumberBEFormatter() : base(32)
        {
        }

        /// <inheritdoc/>
        protected override string GetPartialFormat(PhoneNumber phoneNumber)
        {
            if (OneDigitAreaCode(phoneNumber)) return "N NNNNNNNN";
            else if (TwoDigitAreaCode(phoneNumber)) return "NN NNNNNNN";
            else return "NNN NNNNNN";
        }

        private static bool TwoDigitAreaCode(PhoneNumber phoneNumber)
        {
            switch (phoneNumber.Trimmed[0])
            {
                case '1':
                case '5':
                case '6':
                case '7':
                case '8':
                    return true;
                default:
                    return false;
            }
        }

        private static bool OneDigitAreaCode(PhoneNumber phoneNumber)
        {
            switch (phoneNumber.Trimmed[0])
            {
                case '2':
                case '3':
                case '4':
                    switch (phoneNumber.Trimmed[1])
                    {
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            return false;
                        default:
                            return true;
                    }
                case '9':
                    return true;
                default:
                    return false;
            }
        }
    }
}
