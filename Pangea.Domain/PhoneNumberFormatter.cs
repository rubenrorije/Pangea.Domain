using Pangea.Domain.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Class to print a phone number
    /// </summary>
    public class PhoneNumberFormatter
    {
        private readonly PhoneNumber _phoneNumber;
        private readonly string _format;
        private readonly bool _includeOriginalSpaces;

        private string TextToPrint => _includeOriginalSpaces ? _phoneNumber.Text : _phoneNumber.Trimmed;

        private PhoneNumberFormatter(PhoneNumber phone, string format)
        {
            _phoneNumber = phone;
            _format = string.IsNullOrEmpty(format) ? "G" : format;
            _includeOriginalSpaces = char.IsUpper(format[0]);
        }

        private string Print()
        {
            if (TextToPrint == null) return null;

            switch (_format[0])
            {
                case 'o':
                case 'O':
                    if (_format.Length > 1) throw Exception;
                    return "00" + TextToPrint;
                case 'L':
                case 'l':
                    return PrintLocal();
                case 'g':
                case 'G':
                    return PrintGlobal();
                default:
                    throw Exception;
            }
        }

        private Exception Exception => new FormatException($"Cannot create a representation of the phone number with the {_format} format");

        /// <summary>
        /// Return the string representation for the phone number for the given format.
        /// </summary>
        /// <param name="phone">the phone number to format</param>
        /// <param name="format">the format to use</param>
        /// <returns>the string representation of the phone number</returns>
        public static string Format(PhoneNumber phone, string format)
        {
            return new PhoneNumberFormatter(phone, format).Print();
        }

        private string PrintLocal()
        {
            var callingCodeLength = _phoneNumber.CountryCode?.ToString().Length ?? 0;

            if (_phoneNumber.CountryCode == null) return Format(_phoneNumber, char.IsUpper(_format[0]) ? "G" : "g");
            if (_format.Length == 2 && char.IsDigit(_format[1]))
            {
                return Group("0" + _phoneNumber.Trimmed.Substring(callingCodeLength), _format[1]);
            }
            else
            {
                return "0" + TextToPrint.Substring(callingCodeLength);
            }
        }

        private string PrintGlobal()
        {
            var prefix = "+";
            if (_format.Length == 2 && char.IsDigit(_format[1]))
            {
                // groups specified
                return prefix + Group(_phoneNumber.Trimmed, _format[1]);
            }
            else
            {
                return "+" + TextToPrint;
            }
        }

        private string Group(string text, char groupSize)
        {
            if (_includeOriginalSpaces) throw Exception;

            if (char.IsDigit(groupSize) && int.TryParse(groupSize.ToString(), out var size))
            {
                if (size == 0) throw Exception;
                return string.Join(" ", text.Page(size).Select(chrs => new string(chrs.ToArray())));
            }
            else
            {
                throw Exception;
            }
        }

    }
}
