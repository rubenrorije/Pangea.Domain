using Pangea.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Easy way to construct an Emoji Flag for a given country. Note that this does not work in Windows 10 (for now)
    /// because Windows 10 does not support flag emojis
    /// </summary>
    public static class EmojiFlag
    {

        /// <summary>
        /// Create a flag by the locale id
        /// </summary>
        /// <param name="lcid">the locale id</param>
        public static string Create(int lcid)
        {
            return Create(new RegionInfo(lcid));
        }

        /// <summary>
        /// Create a flag by the given culture
        /// </summary>
        /// <param name="culture">the culture</param>
        public static string Create(CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));
            return Create(culture.LCID);
        }

        /// <summary>
        /// Create a flag by the given region
        /// </summary>
        /// <param name="region">the region (country)</param>
        public static string Create(RegionInfo region)
        {
            if (region == null) throw new ArgumentNullException(nameof(region));
            return Create(region.TwoLetterISORegionName);
        }

        /// <summary>
        /// Create a flag by the given country code (alpha-2)
        /// </summary>
        /// <param name="isoTwoLetterName">the name of the country in two letters</param>
        public static string Create(string isoTwoLetterName)
        {
            if (isoTwoLetterName == null) return null;
            if (isoTwoLetterName.Length < 2) throw new ArgumentOutOfRangeException(nameof(isoTwoLetterName), Resources.EmojiFlag_LessThan2Characters);
            if (isoTwoLetterName.Length > 2) throw new ArgumentOutOfRangeException(nameof(isoTwoLetterName), Resources.EmojiFlag_MoreThan2Characters);
            return
                string.Concat(
                    isoTwoLetterName
                    .ToUpper(CultureInfo.InvariantCulture)
                    .Select(character => char.ConvertFromUtf32(character + 0x1F1A5)));
        }
    }
}
