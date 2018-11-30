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

        private GpsLocation Execute()
        {
            var latitude = 0.0;
            var longitude = 0.0;

            if (HasDegreeSymbol)
            {
                (var lat, var lon) = SplitOnNorthSouth(Text);
                var latPos = IsPositive(lat);
                var lonPos = IsPositive(lon);

                (var latDeg, var latDegRest) = SplitOnDegreeSymbol(lat);
                (var lonDeg, var lonDegRest) = SplitOnDegreeSymbol(lon);

                latitude += double.Parse(latDeg, Format);
                longitude += double.Parse(lonDeg, Format);

                if (HasMinuteSymbol)
                {
                    (var latMin, var latMinRest) = SplitOnMinuteSymbol(latDegRest);
                    (var lonMin, var lonMinRest) = SplitOnMinuteSymbol(lonDegRest);

                    latitude += double.Parse(latMin, Format) / 60.0;
                    longitude += double.Parse(lonMin, Format) / 60.0;

                    if (HasSecondSymbol)
                    {
                        // degrees, minutes, seconds
                        // 40° 26′ 46″ N 79° 58′ 56″ W
                        var latSec = SplitOnSecondSymbol(latMinRest);
                        var lonSec = SplitOnSecondSymbol(lonMinRest);

                        latitude += double.Parse(latSec, Format) / 3600.0;
                        longitude += double.Parse(lonSec, Format) / 3600.0;
                    }
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

                    latitude = double.Parse(lat, Format) * latPos;
                    longitude = double.Parse(lon, Format) * lonPos;
                }
                else
                {
                    // decimal format with direction
                    // -40.55 79.95
                    (var lat, var lon) = SplitOnSeparatorSymbol(Text);
                    latitude = double.Parse(lat, Format);
                    longitude = double.Parse(lon, Format);
                }
            }
            return new GpsLocation(latitude, longitude);
        }

        /// <summary>
        /// Parse a text to a GPS location.
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <param name="format">the format to use, to determine the decimal separator</param>
        internal static GpsLocation Parse(string text, NumberFormatInfo format)
        {
            var parser = new GpsLocationParser(text, format);
            return parser.Execute();
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

            return Tuple.Create(text.Substring(0, index + (includeDelimiter ? 1 : 0)).Trim(), text.Substring(index + 1).Trim());
        }

    }
}
