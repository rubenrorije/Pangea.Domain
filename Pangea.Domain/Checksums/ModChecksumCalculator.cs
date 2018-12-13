using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Pangea.Domain.Checksums
{
    /// <summary>
    /// Verify a number for a given modulo. That is, given a number (in string format) the
    /// modulo is performed and the remainder is returned. That remainder can be validated
    /// against the check digit.
    /// </summary>
    internal class ModChecksumCalculator : IChecksumCalculator
    {
        private readonly int _modulo;
        /// <summary>
        /// Create a new modulo verifier for the given modulo
        /// </summary>
        /// <param name="modulo"></param>
        public ModChecksumCalculator(int modulo)
        {
            if (modulo <= 0) throw new ArgumentOutOfRangeException(nameof(modulo));
            _modulo = modulo;
        }
        /// <summary>
        /// Calculate the result of the modulo algorithm for the given subject.
        /// The result can be validated against the check digit
        /// </summary>
        /// <param name="subject">the number/text to validate</param>
        /// <returns>the expected check digit</returns>
        public int Calculate(string subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));

            var index = 2;
            var mod = int.Parse(subject.Substring(0, Math.Min(subject.Length, 2)), CultureInfo.InvariantCulture);

            while (index <= subject.Length)
            {
                var current = int.Parse(mod.ToString(CultureInfo.InvariantCulture) + subject.Substring(index, Math.Min(subject.Length - index, 7)), CultureInfo.InvariantCulture);
                mod = current % _modulo;
                index += 7;
            }
            return mod;
        }
    }
}
