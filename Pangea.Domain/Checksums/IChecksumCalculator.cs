using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Checksums
{
    /// <summary>
    /// Allows multiple algorithms to calculate a checksum to have the same interface
    /// </summary>
    internal interface IChecksumCalculator
    {
        /// <summary>
        /// Calculate the result of the modulo algorithm for the given subject.
        /// The result can be validated against the check digit
        /// </summary>
        /// <param name="subject">the number/text to validate</param>
        /// <returns>the expected check digit</returns>
        int Calculate(string subject);
    }
}
