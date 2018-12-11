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

        /// <summary>
        /// Return the dutch format of a phone number
        /// </summary>
        /// <param name="phoneNumber">the phone number to format</param>
        /// <returns>the new format</returns>
        public string GetLocalFormat(PhoneNumber phoneNumber)
        {
            if (phoneNumber.Trimmed.StartsWith("6"))
            {
                return "0N-NN NN NN NN";
            }
            if (IsThreeDigitAreaCode(phoneNumber))
            {
                return "0NN-NNNNNNN";
            }
            else
            {
                return "0NNN-NNNNNN";
            }
        }

        /// <summary>
        /// Return the dutch format of a phone number
        /// </summary>
        /// <param name="phoneNumber">the phone number to format</param>
        /// <returns>the new format</returns>
        public string GetGlobalFormat(PhoneNumber phoneNumber)
        {
            if (phoneNumber.Trimmed.StartsWith("6"))
            {
                return "+C N NN NN NN NN";
            }
            if (IsThreeDigitAreaCode(phoneNumber))
            {
                return "+C NN-NN NN NN NN";
            }
            else
            {
                return "+C NNN-NN NN NNN";
            }
        }

        private bool IsThreeDigitAreaCode(PhoneNumber phoneNumber)
        {
            if (phoneNumber.Trimmed.Length < 2) return false;
            var possibleAreaCode = phoneNumber.Trimmed.Substring(0, 2);

            switch (possibleAreaCode)
            {
                case "10":
                case "13":
                case "14":
                case "15":
                case "20":
                case "23":
                case "24":
                case "26":
                case "30":
                case "33":
                case "35":
                case "36":
                case "38":
                case "40":
                case "43":
                case "44":
                case "45":
                case "46":
                case "50":
                case "53":
                case "55":
                case "58":
                case "70":
                case "71":
                case "72":
                case "73":
                case "74":
                case "75":
                case "76":
                case "77":
                case "78":
                case "79":
                    return true;
                default:
                    return false;

            }
        }
    }
}
