using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Internal class to parse a GPS location from text
    /// </summary>
    internal class GpsLocationParser
    {
        private bool HasDegreeSymbol { get; set; }
        private bool HasMinuteSymbol { get; set; }
        private bool HasSecondSymbol { get; set; }
        private bool HasDirection { get; set; }

        private string Text { get; set; }

        private static readonly string[] _directions = new[] { "N", "S", "E", "W" };
        private NumberFormatInfo Format { get; }

        private GpsLocationParser(string text, NumberFormatInfo format)
        {
            Text = text;
            HasDegreeSymbol = Text.Contains("°");
            HasMinuteSymbol = text.Contains("'") || text.Contains("′");
            HasSecondSymbol = text.Contains("\"") || text.Contains("″");
            HasDirection = _directions.Any(dir => text.Contains(dir));
            Format = format;
        }

        private Tuple<bool, GpsLocation> Execute()
        {
            var invalid = Tuple.Create(false, new GpsLocation());
            var latitude = 0.0;
            var longitude = 0.0;

            if (HasDegreeSymbol)
            {
                (var lat, var lon) = SplitOnNorthSouth(Text);
                var latPos = IsPositive(lat);
                var lonPos = IsPositive(lon);

                (var latDeg, var latDegRest) = SplitOnDegreeSymbol(lat);
                (var lonDeg, var lonDegRest) = SplitOnDegreeSymbol(lon);

                if (HasMinuteSymbol)
                {
                    if (!TryParsePositiveInteger(latDeg, out var dLatDeg)) return invalid;
                    if (!TryParsePositiveInteger(lonDeg, out var dLonDeg)) return invalid;

                    latitude += dLatDeg;
                    longitude += dLonDeg;

                    (var latMin, var latMinRest) = SplitOnMinuteSymbol(latDegRest);
                    (var lonMin, var lonMinRest) = SplitOnMinuteSymbol(lonDegRest);

                    if (HasSecondSymbol)
                    {
                        if (!TryParsePositiveInteger(latMin, out var dLatMin)) return invalid;
                        if (!TryParsePositiveInteger(lonMin, out var dLonMin)) return invalid;

                        latitude += (dLatMin / 60.0);
                        longitude += (dLonMin / 60.0);

                        // degrees, minutes, seconds
                        // 40° 26′ 46″ N 79° 58′ 56″ W
                        var latSec = SplitOnSecondSymbol(latMinRest);
                        var lonSec = SplitOnSecondSymbol(lonMinRest);

                        if (!TryParsePositiveFloat(latSec, out var dLatSec)) return invalid;
                        if (!TryParsePositiveFloat(lonSec, out var dLonSec)) return invalid;

                        latitude += (dLatSec / 3600.0);
                        longitude += (dLonSec / 3600.0);
                    }
                    else
                    {
                        if (!TryParsePositiveFloat(latMin, out var dLatMin)) return invalid;
                        if (!TryParsePositiveFloat(lonMin, out var dLonMin)) return invalid;

                        latitude += (dLatMin / 60.0);
                        longitude += (dLonMin / 60.0);
                    }
                }
                else
                {
                    if (!TryParsePositiveFloat(latDeg, out var dLatDeg)) return invalid;
                    if (!TryParsePositiveFloat(lonDeg, out var dLonDeg)) return invalid;

                    latitude += dLatDeg;
                    longitude += dLonDeg;
                }

                latitude *= latPos;
                longitude *= lonPos;
            }
            else
            {
                if (HasDirection)
                {
                    // decimal format with direction
                    // 40.55 N 79.95 W
                    (var lat, var lon) = SplitOnNorthSouth(Text);
                    var latPos = IsPositive(lat);
                    var lonPos = IsPositive(lon);

                    if (!TryParsePositiveFloat(lat, out latitude)) return invalid;
                    if (!TryParsePositiveFloat(lon, out longitude)) return invalid;
                    latitude *= latPos;
                    longitude *= lonPos;
                }
                else
                {
                    // decimal format with direction
                    // -40.55 79.95
                    (var lat, var lon) = SplitOnSeparatorSymbol(Text);
                    if (!TryParseFloat(lat, out latitude)) return invalid;
                    if (!TryParseFloat(lon, out longitude)) return invalid;
                }
            }

            if (!GpsLocation.ValidateLatitude(latitude)) return invalid;
            if (!GpsLocation.ValidateLongitude(longitude)) return invalid;

            return Tuple.Create(true, new GpsLocation(latitude, longitude));
        }

        private bool TryParseFloat(string text, out double result)
        {
            var style = NumberStyles.Float & ~NumberStyles.AllowExponent & ~NumberStyles.AllowThousands;
            return TryParse(text, style, out result);
        }

        private bool TryParsePositiveFloat(string text, out double result)
        {
            var style =
                NumberStyles.Float &
                ~NumberStyles.AllowExponent &
                ~NumberStyles.AllowThousands &
                ~NumberStyles.AllowLeadingSign &
                ~NumberStyles.AllowTrailingSign;

            return TryParse(text, style, out result);
        }

        private bool TryParseInteger(string text, out double result)
        {
            var style = NumberStyles.Integer;
            return TryParse(text, style, out result);
        }

        private bool TryParsePositiveInteger(string text, out double result)
        {
            var style = NumberStyles.Integer & ~NumberStyles.AllowLeadingSign;
            return TryParse(text, style, out result);
        }

        private bool TryParse(string text, NumberStyles style, out double result)
        {
            return double.TryParse(text, style, Format, out result);
        }

        /// <summary>
        /// Parse a text to a GPS location.
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <param name="format">the format to use, to determine the decimal separator</param>
        /// <param name="result">the parsed GPS location</param>
        internal static bool TryParse(string text, NumberFormatInfo format, out GpsLocation result)
        {
            var parser = new GpsLocationParser(text, format);
            var parsed = parser.Execute();
            result = parsed.Item2;
            return parsed.Item1;
        }

        private Tuple<string, string> SplitOnSeparatorSymbol(string text)
        {
            var delimiters = new List<char> { ' ', ';' };

            if (Format.NumberDecimalSeparator == CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
            {
                delimiters.Add(',');
            }

            return SplitOn(text, false, delimiters.ToArray());
        }

        private static int IsPositive(string text)
        {
            if (text.Contains("S") || text.Contains("W")) return -1;
            else return 1;
        }

        private static string SplitOnSecondSymbol(string text)
        {
            return SplitOn(text, false, '″', '"').Item1;
        }

        private static Tuple<string, string> SplitOnMinuteSymbol(string text)
        {
            return SplitOn(text, false, '′', '\'');
        }

        private static Tuple<string, string> SplitOnDegreeSymbol(string text)
        {
            return SplitOn(text, false, '°');
        }

        private static Tuple<string, string> SplitOnNorthSouth(string text)
        {
            return SplitOn(text, true, 'N', 'S');
        }

        private static Tuple<string, string> SplitOn(string text, bool includeDelimiter, params char[] delimiters)
        {
            var index = delimiters.Max(del => text.IndexOf(del));
            if (index < 0) return new Tuple<string, string>(text, null);
            return Tuple.Create(text.Substring(0, index + (includeDelimiter ? 1 : 0)).Trim(), text.Substring(index + 1).Trim());
        }

    }
}
