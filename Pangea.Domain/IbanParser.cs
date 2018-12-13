using Pangea.Domain.Checksums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Iban parser class
    /// </summary>
    internal static class IbanParser
    {
        /// <summary>
        /// Holder to store the result of the parsing
        /// </summary>
        internal class ParsingResult
        {
            /// <summary>
            /// Country code (2 letters, uppercase)
            /// </summary>
            public string CountryCode { get; set; }
            /// <summary>
            /// Check digits, 2 digits
            /// </summary>
            public string CheckDigits { get; set; }
            /// <summary>
            /// Country specific account number
            /// </summary>
            public string BasicAccountNumber { get; set; }

            /// <summary>
            /// Is the parsed value a valid Iban
            /// </summary>
            public bool Valid { get; set; }

            /// <summary>
            /// The error message
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Empty result
            /// </summary>
            public ParsingResult()
            {
                CountryCode = string.Empty;
                CheckDigits = string.Empty;
                BasicAccountNumber = string.Empty;
                Valid = true;
            }

            /// <summary>
            /// An invalid Iban was parsed
            /// </summary>
            /// <param name="message">The error message</param>
            public static ParsingResult Invalid(string message)
            {
                return
                    new ParsingResult
                    {
                        Valid = false,
                        Message = message
                    };
            }
            /// <summary>
            /// return the pretty printed format of the IBAN
            /// </summary>
            public override string ToString()
            {
                var text = new StringBuilder();
                text.Append(CountryCode);
                text.Append(CheckDigits);

                for (int index = 0; index < BasicAccountNumber.Length; index += 4)
                {
                    var size = Math.Min(4, BasicAccountNumber.Length - index);
                    text.Append(" ");
                    text.Append(BasicAccountNumber.Substring(index, size));
                }
                return text.ToString();
            }
        }

        /// <summary>
        /// check the iban to make sure that the check digits are correct for the basic bank account number
        /// </summary>
        /// <param name="iban">the iban to check</param>
        internal static bool Validate(ParsingResult iban)
        {
            if (string.IsNullOrEmpty(iban.BasicAccountNumber)) return true;
            var rearranged = iban.BasicAccountNumber + iban.CountryCode + iban.CheckDigits;
            var converted = string.Join("", rearranged.Select(ToInteger));

            var algorithm = new ModChecksumCalculator(97);
            return algorithm.Validate(converted, 1);
        }

        private static string ToInteger(char character)
        {
            if (char.IsLetter(character)) return ((character - 'A') + 10).ToString(CultureInfo.InvariantCulture);
            else return character.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parse the text to an Iban
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <returns></returns>
        public static ParsingResult TryParse(string text)
        {
            // remove whitespace chars
            var toParse = (text ?? string.Empty).Replace(" ", "").Replace("\t", "").ToUpper(CultureInfo.InvariantCulture);
            if (toParse.Length == 0) return new ParsingResult();
            if (toParse.Length > 34)
                return ParsingResult.Invalid("the Iban can be at most 34 characters. 2 for the country code, 2 check digits and 30 for the country specific account number");
            if (toParse.Length <= 4)
                return ParsingResult.Invalid("the Iban must be at least 5 characters. 2 for the country code, 2 check digits and at least 1 for the country specific account number");

            var result = new ParsingResult
            {
                CountryCode = toParse.Substring(0, 2),
                CheckDigits = toParse.Substring(2, 2),
                BasicAccountNumber = toParse.Substring(4, toParse.Length - 4)
            };

            if (!result.CountryCode.All(chr => char.IsUpper(chr))) return ParsingResult.Invalid("The Iban should start with the ISO country code (ALPHA-2)");
            if (!result.CheckDigits.All(chr => char.IsDigit(chr))) return ParsingResult.Invalid("The country code should be followed by 2 check digits (0-9)");
            if (!result.BasicAccountNumber.All(chr => char.IsLetterOrDigit(chr))) return ParsingResult.Invalid("The iban can only contain letters (A-Z) and digits (0-9)");

            return result;
        }


    }
}
