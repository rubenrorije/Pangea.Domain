using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static Pangea.Domain.Util.Math;
using static System.Math;
namespace Pangea.Domain
{
    /// <summary>
    /// Location represented by Latitude and Longitude to store a position on the earth.
    /// </summary>
    public struct GpsLocation
        : IEquatable<GpsLocation>
        , IFormattable
    {
        /// <summary>
        /// An angle which ranges from -90° to 90° (south to north)
        /// The value represents the vertical position on a map. ≈ Y
        /// Denoted by Φ (the greek letter phi)
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// An angle which ranges from -180° to 180° (west to east)
        /// The value represents the horizontal position on a map. ≈ X
        /// Denoted by λ (the greek letter lambda)
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Returns whether the current location is on the northern hemisphere. 
        /// The equator (0°) is included in the northern hemisphere
        /// </summary>
        public bool IsNorthernHemisphere => Latitude >= 0;

        /// <summary>
        /// Returns whether the current location is on the southern hemisphere. 
        /// The equator (0°) is excluded from the southern hemisphere
        /// </summary>
        public bool IsSouthernHemisphere => !IsNorthernHemisphere;

        /// <summary>
        /// Returns whether the current locaiton is on the eastern hemisphere.
        /// The prime meridian (0°) is included in the eastern hemisphere
        /// </summary>
        public bool IsEasternHemisphere => Longitude >= 0;

        /// <summary>
        /// Returns whether the current locaiton is on the eastern hemisphere.
        /// The prime meridian (0°) is included in the eastern hemisphere
        /// </summary>
        public bool IsWesternHemisphere => !IsEasternHemisphere;

        /// <summary>
        /// Create a new GPS location for the given latitude and longitude
        /// </summary>
        /// <param name="latitude">between -90° and 90° representing the south-north position on the earth. (≈Y)</param>
        /// <param name="longitude">between -180° and 180° representing the west-east position on the earth. (≈X)</param>
        public GpsLocation(double latitude, double longitude)
        {
            if (!ValidateLatitude(latitude)) throw new ArgumentOutOfRangeException(nameof(latitude));
            if (!ValidateLongitude(longitude)) throw new ArgumentOutOfRangeException(nameof(longitude));

            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Validate whether the latitude is within the expected range
        /// </summary>
        /// <param name="latitude">the latitude to check</param>
        /// <returns>In range?</returns>
        internal static bool ValidateLatitude(double latitude)
        {
            return latitude >= -90 && latitude <= 90;
        }

        /// <summary>
        /// Validate whether the longitude is within the expected range
        /// </summary>
        /// <param name="longitude">the longitude to check</param>
        /// <returns>In range?</returns>
        internal static bool ValidateLongitude(double longitude)
        {
            return longitude >= -180 && longitude <= 180;
        }

        /// <inheritdoc/>
        public bool Equals(GpsLocation other)
        {
            return
                Latitude == other.Latitude &
                Longitude == other.Longitude;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is GpsLocation) return Equals((GpsLocation)obj);
            return base.Equals(obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Latitude.GetHashCode();
                hash = hash * 23 + Longitude.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public static bool operator ==(GpsLocation lhs, GpsLocation rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <inheritdoc/>
        public static bool operator !=(GpsLocation lhs, GpsLocation rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Return the approximate distance (as the crow flies) between two points in kilometers. Note that this is not an accurate distance 
        /// because it assumes a spherical earth, which is not the case. Yet, because the error is less than 0.3% this is 
        /// accurate enough for most calculations.
        /// </summary>
        /// <param name="other">the other location</param>
        /// <returns>The great circle distance between both points in kilometers.</returns>
        public double ApproximateDistanceTo(GpsLocation other)
        {
            /*
             * This uses the ‘haversine’ formula to calculate the great-circle distance between 
             * two points – that is, the shortest distance over the earth’s surface – 
             * giving an ‘as-the-crow-flies’ distance between the points 
             * (ignoring any hills they fly over, of course!).
             * 
             * Haversine formula:	
             * a = sin²(Δφ/2) + cos φ1 ⋅ cos φ2 ⋅ sin²(Δλ/2)
             * c = 2 ⋅ atan2( √a, √(1−a) )
             * d = R ⋅ c
             * where φ is latitude, λ is longitude, R is earth’s radius (mean radius = 6,371km);
             * note that angles need to be in radians to pass to trig functions!
             */

            var radius = 6371;

            var phi_1 = Latitude.ToRadians();
            var phi_2 = other.Latitude.ToRadians();

            var delta_phi = (Latitude - other.Latitude).ToRadians();
            var delta_lambda = (Longitude - other.Longitude).ToRadians();

            var temp_a =
                Sin2(delta_phi / 2) +
                (Cos(phi_1) * Cos(phi_2) * Sin2(delta_lambda / 2));
            var temp_c = 2 * Atan2(Sqrt(temp_a), Sqrt(1 - temp_a));

            return radius * temp_c;
        }

        /// <summary>
        /// returns the textual representation of a GPS location in the default format.
        /// For both Latitude and Longitude the default decimal format will be used for the current culture. 
        /// The Latitude and Longitude are separated with a space and will be represented with a 
        /// negative value for South / West locations
        /// </summary>
        /// <returns>The default textual representation of a GPS location</returns>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Convert a textual representation of a GPS location to an actual GPS location.
        /// This will throw an exception when the text could not be parsed.
        /// When no format provider is specified, it is assumed that the decimal separator is a dot (.),
        /// i.e. the invariant culture will be used.
        /// <para>
        /// Allowed formats are DMS, DM, D. When using only degrees the direction can be specified using a sign or using N/S E/W
        /// </para>
        /// <para>
        /// Allowed separators between Latitude and Longitude are &lt;space&gt;, &lt;,&gt; and &lt;;&gt;. A comma is only allowed when the decimal separator used is not a comma as well. 
        /// </para>
        /// </summary>
        /// <param name="text">the text to be parsed</param>
        /// <returns>a valid GPS location. When the text could not be parsed an exception is thrown</returns>
        public static GpsLocation Parse(string text)
        {
            return Parse(text, CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Convert a textual representation of a GPS location to an actual GPS location.
        /// This will throw an exception when the text could not be parsed.
        /// </summary>
        /// <param name="text">the text to be parsed</param>
        /// <param name="culture">the culture to use, to determine the decimal separator</param>
        /// <returns>a valid GPS location. When the text could not be parsed an exception is thrown</returns>
        public static GpsLocation Parse(string text, CultureInfo culture)
        {
            return Parse(text, culture.NumberFormat);
        }

        /// <summary>
        /// Convert a textual representation of a GPS location to an actual GPS location.
        /// This will throw an exception when the text could not be parsed.
        /// </summary>
        /// <param name="text">the text to be parsed</param>
        /// <param name="format">the format to use, to determine the decimal separator</param>
        /// <returns>a valid GPS location. When the text could not be parsed an exception is thrown</returns>
        public static GpsLocation Parse(string text, NumberFormatInfo format)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrEmpty(text)) throw new ArgumentOutOfRangeException(nameof(text));

            if (!GpsLocationParser.TryParse(text, format, out GpsLocation result))
            {
                throw new ArgumentOutOfRangeException(nameof(text));
            }
            return result;
        }

        /// <summary>
        /// returns the textual representation of a GPS location in the given format for the current culture.
        /// The Latitude and Longitude are separated with a space and will be represented with a 
        /// negative value for South / West locations
        /// </summary>
        /// <returns>The textual representation of a GPS location</returns>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// returns the textual representation of a GPS location in the given format and the given format provider.
        /// The Latitude and Longitude are separated with a space and will be represented with a 
        /// negative value for South / West locations
        /// </summary>
        /// <returns>The textual representation of a GPS location</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return
                Latitude.ToString(format, formatProvider) +
                " " +
                Longitude.ToString(format, formatProvider);
        }

        /// <summary>
        /// Try to create a GPS location from the given text
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <param name="result">the parsed location</param>
        /// <returns>whether the parsing was successful</returns>
        public static bool TryParse(string text, out GpsLocation result)
        {
            return TryParse(text, CultureInfo.InvariantCulture.NumberFormat, out result);
        }

        /// <summary>
        /// Try to create a GPS location from the given text
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <param name="result">the parsed location</param>
        /// <param name="culture">the culture to use to determine the decimal separator</param>
        /// <returns>whether the parsing was successful</returns>
        public static bool TryParse(string text, CultureInfo culture, out GpsLocation result)
        {
            return TryParse(text, culture.NumberFormat, out result);
        }

        /// <summary>
        /// Try to create a GPS location from the given text
        /// </summary>
        /// <param name="text">the text to parse</param>
        /// <param name="result">the parsed location</param>
        /// <param name="format">the format to use</param>
        /// <returns>whether the parsing was successful</returns>
        public static bool TryParse(string text, NumberFormatInfo format, out GpsLocation result)
        {
            return GpsLocationParser.TryParse(text, format, out result);
        }
    }
}
