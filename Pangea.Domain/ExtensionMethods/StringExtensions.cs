using System;
using System.Collections.Generic;
using System.Text;

namespace Pangea.Domain.ExtensionMethods
{
    /// <summary>
    /// String extensions to be used within this project.
    /// </summary>
    internal static class StringExtensions
    {

        /// <summary>
        /// Replace the first occurrence of <paramref name="pattern"/> in <paramref name="text"/> with <paramref name="replacement"/>
        /// </summary>
        /// <param name="text">The original text</param>
        /// <param name="pattern">The pattern to replace</param>
        /// <param name="replacement">The text to replace <paramref name="pattern"/> with</param>
        /// <param name="comparison">The comparison to use</param>
        /// <returns>The text with the first occurrence of <paramref name="pattern"/> replaced when it is found, the original text otherwise.</returns>
        public static string ReplaceFirst(this string text, string pattern, string replacement, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (string.IsNullOrEmpty(pattern)) return text;

            var index = text.IndexOf(pattern, comparison);
            if (index < 0) return text;

            return
                text
                .Remove(index, pattern.Length)
                .Insert(index, replacement);
        }

    }
}
