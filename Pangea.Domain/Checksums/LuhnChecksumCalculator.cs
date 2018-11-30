using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.Checksums
{
    /// <summary>
    /// Use the Luhn formula to calculate the check digit. See https://en.wikipedia.org/wiki/Luhn_algorithm
    /// for more information
    /// </summary>
    internal class LuhnChecksumCalculator : IChecksumCalculator
    {

        /// <inheritdoc/>
        public int Calculate(string subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));

            var runningSum = 0;

            // calculate based on the length of the string whether 
            // the even or the odd characters must be doubled.
            // Because the algorithm starts from the right side, this depends on the length 
            // of the given string. 
            // when the number of characters are odd (e.g. 3) the even indices (0 and 2) must be doubled
            // when even (e.g. 4) the odd indices (1 and 3) must be doubled
            var doublingEveryOddOrEven = (subject.Length + 1) % 2;

            for (var index = subject.Length - 1; index >= 0; index--)
            {
                var current = subject[index] - '0';

                if (index % 2 == doublingEveryOddOrEven)
                {
                    current *= 2;
                    if (current >= 10)
                    {
                        runningSum -= 9;
                    }
                }
                runningSum += current;
            }
            return (10 - (runningSum % 10));
        }

    }
}
