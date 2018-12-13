using Pangea.Domain.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            switch (_format)
            {
                case "o":
                case "O":
                    return "00" + TextToPrint;
                case "L":
                case "l":
                    return PrintLocal();
                case "g":
                case "G":
                    return PrintGlobal();
                default:
                    break;
            }

            var builder = new StringBuilder(_phoneNumber.Text.Length);

            var numberIndex = 0;
            foreach (var chr in _format)
            {
                switch (chr)
                {
                    case 'C':
                        builder.Append(_phoneNumber.CountryCode);
                        break;
                    case 'N':
                        if (numberIndex >= _phoneNumber.Trimmed.Length) break;
                        builder.Append(_phoneNumber.Trimmed[numberIndex]);
                        numberIndex++;
                        break;
                    default:
                        builder.Append(chr);
                        break;
                }
            }
            if (numberIndex < _phoneNumber.Trimmed.Length)
            {
                builder.Append(_phoneNumber.Trimmed.Substring(numberIndex));
            }
            return builder.ToString();
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
            if (_phoneNumber.CountryCode == null) return Format(_phoneNumber, char.IsUpper(_format[0]) ? "G" : "g");
            if (_format.Length == 2 && char.IsDigit(_format[1]))
            {
                return Group("0" + _phoneNumber.Trimmed, _format[1]);
            }
            else
            {
                return "0" + TextToPrint;
            }
        }

        private string PrintGlobal()
        {
            var prefix = "+";
            if (_format.Length == 2 && char.IsDigit(_format[1]))
            {
                // groups specified
                return prefix + Group(_phoneNumber.CountryCode.ToString() + _phoneNumber.Trimmed, _format[1]);
            }
            else
            {
                return "+" + _phoneNumber.CountryCode?.ToString(CultureInfo.InvariantCulture) + TextToPrint;
            }
        }

        private string Group(string text, char groupSize)
        {
            if (_includeOriginalSpaces) throw Exception;

            if (char.IsDigit(groupSize) && int.TryParse(groupSize.ToString(CultureInfo.InvariantCulture), out var size))
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
