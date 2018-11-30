using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Checksums
{

    /// <summary>
    /// Convenient methods to validate a number directly 
    /// </summary>
    internal static class ChecksumCalculatorExtensions
    {
        /// <summary>
        /// Validate the given string having the last digit to be the check digit
        /// </summary>
        /// <param name="calculator">the algorithm</param>
        /// <param name="includingChecksumAsFinalDigit">the full number to validate including the last digit that is the expected checksum</param>
        /// <returns>Is the given number valid</returns>
        public static bool Validate(this IChecksumCalculator calculator, string includingChecksumAsFinalDigit)
        {
            int indexOfCheckDigit = includingChecksumAsFinalDigit.Length - 2;
            return calculator.Validate(includingChecksumAsFinalDigit.Substring(0, indexOfCheckDigit), includingChecksumAsFinalDigit.Substring(indexOfCheckDigit));
        }


        /// <summary>
        /// Validate the subject for the given expected checksum.
        /// The checksum is calculated and then compared to the expected one.
        /// </summary>
        /// <param name="calculator">The algorithm</param>
        /// <param name="subject">the text to check</param>
        /// <param name="expectedChecksum">the checksum that is expected</param>
        /// <returns>Is the expected checksum found</returns>
        public static bool Validate(this IChecksumCalculator calculator, string subject, string expectedChecksum)
        {
            return calculator.Validate(subject, int.Parse(expectedChecksum));
        }

        /// <summary>
        /// Validate the subject for the given expected checksum.
        /// The checksum is calculated and then compared to the expected one.
        /// </summary>
        /// <param name="calculator">The algorithm</param>
        /// <param name="subject">the text to check</param>
        /// <param name="expectedChecksum">the checksum that is expected</param>
        /// <returns>Is the expected checksum found</returns>
        public static bool Validate(this IChecksumCalculator calculator, string subject, int expectedChecksum)
        {
            var actualChecksum = calculator.Calculate(subject);
            return actualChecksum == expectedChecksum;
        }

    }
}
