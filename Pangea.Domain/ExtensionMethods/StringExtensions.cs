using System;
using System.Collections.Generic;
using System.Globalization;
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

        /// <summary>
        /// Remove all occurrences of the given characters to remove and return the result
        /// </summary>
        /// <param name="text">The text to remove parts from</param>
        /// <param name="partsToRemove">the parts to remove</param>
        public static string RemoveAll(this string text, string[] partsToRemove)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var result = new StringBuilder(text);
            foreach (var part in partsToRemove)
            {
                result.Replace(part, "");
            }
            return result.ToString();
        }

        /// <summary>
        /// Returns whether the text contains any of the given characters
        /// </summary>
        /// <param name="text">The text to search in</param>
        /// <param name="characters">The characters to search for</param>
        public static bool ContainsAny(this string text, char[] characters)
        {
            return text.IndexOfAny(characters) >= 0;
        }

        /// <summary>
        /// Does the specified string ends with the given character
        /// </summary>
        /// <param name="text">the text</param>
        /// <param name="character">the character</param>
        /// <returns>Whether text ends with character</returns>
        public static bool EndsWith(this string text, char character)
        {
            return text.EndsWith(character.ToString(CultureInfo.CurrentCulture), StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Does the specified string start with the given character
        /// </summary>
        /// <param name="text">the text</param>
        /// <param name="character">the character</param>
        /// <returns>Whether text starts with character</returns>
        public static bool StartsWith(this string text, char character)
        {
            return text.StartsWith(character.ToString(CultureInfo.CurrentCulture), StringComparison.CurrentCulture);
        }
    }
}
