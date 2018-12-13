using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class GpsLocationTests
    {
        private static readonly Tuple<double, double> _rio_de_janeiro = Tuple.Create(-22.834345, -43.182080);
        private static readonly Tuple<double, double> _washington = Tuple.Create(38.906807, -77.029661);
        private static readonly Tuple<double, double> _moscow = Tuple.Create(55.762903, 37.636756);
        private static readonly Tuple<double, double> _melbourne = Tuple.Create(-37.799152, 144.977813);

        private static readonly Tuple<double, double> _athens = Tuple.Create(37.983795, 23.727007);
        private static readonly Tuple<double, double> _rome = Tuple.Create(41.902790, 12.494096);
        private static readonly Tuple<double, double> _origin = Tuple.Create(0.0, 0.0);


        [TestMethod]
        public void An_Empty_GpsLocation_Is_At_The_Equator_On_The_Prime_Meridian()
        {
            var sut = new GpsLocation();
            sut.Latitude.Should().Be(0);
            sut.Longitude.Should().Be(0);
        }

        [TestMethod]
        public void Latitude_Too_Small_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => new GpsLocation(-91, 0);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Latitude_Too_Large_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => new GpsLocation(91, 0);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Longitude_Too_Small_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => new GpsLocation(0, -181);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Longitude_Too_Large_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => new GpsLocation(0, 181);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Default_ToString()
        {
            var sut = new GpsLocation(5, 10);
            var text = sut.ToString();

            text.Should().Contain(5m.ToString());
            text.Should().Contain(10m.ToString());
        }

        [TestMethod]
        public void Default_ToString_With_Culture()
        {
            var sut = new GpsLocation(5, 10);
            var text = sut.ToString(null, CultureInfo.InvariantCulture);

            text.Should().Contain(5m.ToString(null, CultureInfo.InvariantCulture));
            text.Should().Contain(10m.ToString(null, CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void Use_FormatString_In_ToString()
        {
            var sut = new GpsLocation(5.5555, 10.5555);
            var text = sut.ToString("N2", CultureInfo.InvariantCulture);

            text.Should().Contain(5.5555m.ToString("N2", CultureInfo.InvariantCulture));
            text.Should().Contain(10.5555m.ToString("N2", CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void Approximate_Distance_Of_Two_Locations()
        {
            var athens = Create(_athens);
            var rome = Create(_rome);

            var distance = athens.ApproximateDistanceTo(rome);

            distance.Should().BeInRange(1000.0, 1100.0);
        }

        [TestMethod]
        public void IsNorthernHemisphere()
        {
            var rio_de_janeiro = Create(_rio_de_janeiro);
            var washington = Create(_washington);
            var moscow = Create(_moscow);
            var melbourne = Create(_melbourne);
            var origin = Create(_origin);

            rio_de_janeiro.IsNorthernHemisphere.Should().BeFalse();
            washington.IsNorthernHemisphere.Should().BeTrue();
            moscow.IsNorthernHemisphere.Should().BeTrue();
            melbourne.IsNorthernHemisphere.Should().BeFalse();
            origin.IsNorthernHemisphere.Should().BeTrue();
        }

        [TestMethod]
        public void IsSouthernHemisphere()
        {
            var rio_de_janeiro = Create(_rio_de_janeiro);
            var washington = Create(_washington);
            var moscow = Create(_moscow);
            var melbourne = Create(_melbourne);
            var origin = Create(_origin);

            rio_de_janeiro.IsSouthernHemisphere.Should().BeTrue();
            washington.IsSouthernHemisphere.Should().BeFalse();
            moscow.IsSouthernHemisphere.Should().BeFalse();
            melbourne.IsSouthernHemisphere.Should().BeTrue();
            origin.IsSouthernHemisphere.Should().BeFalse();
        }

        [TestMethod]
        public void IsEasternHemisphere()
        {
            var rio_de_janeiro = Create(_rio_de_janeiro);
            var washington = Create(_washington);
            var moscow = Create(_moscow);
            var melbourne = Create(_melbourne);
            var origin = Create(_origin);

            rio_de_janeiro.IsEasternHemisphere.Should().BeFalse();
            washington.IsEasternHemisphere.Should().BeFalse();
            moscow.IsEasternHemisphere.Should().BeTrue();
            melbourne.IsEasternHemisphere.Should().BeTrue();
            origin.IsEasternHemisphere.Should().BeTrue();
        }

        [TestMethod]
        public void IsWesternHemisphere()
        {
            var rio_de_janeiro = Create(_rio_de_janeiro);
            var washington = Create(_washington);
            var moscow = Create(_moscow);
            var melbourne = Create(_melbourne);
            var origin = Create(_origin);

            rio_de_janeiro.IsWesternHemisphere.Should().BeTrue();
            washington.IsWesternHemisphere.Should().BeTrue();
            moscow.IsWesternHemisphere.Should().BeFalse();
            melbourne.IsWesternHemisphere.Should().BeFalse();
            origin.IsWesternHemisphere.Should().BeFalse();
        }

        [TestMethod]
        public void Parse_Null_Throws_ArgumentNullException()
        {
            Action action = () => GpsLocation.Parse(null);
            action.Should().Throw<ArgumentNullException>();

        }

        [TestMethod]
        public void Parse_Empty_String_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => GpsLocation.Parse(string.Empty);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Parse_Invalid_String_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => GpsLocation.Parse("abc");
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Parse_Degree_Minutes_Seconds_Without_Spaces_To_GpsLocation()
        {
            var location = "41°54'10.0\"N 12°29'38.8\"E";

            var sut = GpsLocation.Parse(location);

            sut.Latitude.Should().BeInRange(41.9, 42.0);
            sut.Longitude.Should().BeInRange(12.4, 12.5);
        }


        [TestMethod]
        public void Parse_Degree_Minutes_Seconds_With_Spaces_To_GpsLocation()
        {
            var location = "41° 54' 10.0\" N 12° 29' 38.8\" E";

            var sut = GpsLocation.Parse(location);

            sut.Latitude.Should().BeInRange(41.9, 42.0);
            sut.Longitude.Should().BeInRange(12.4, 12.5);
        }

        [TestMethod]
        public void Parse_Degree_Minutes_Without_Spaces_To_GpsLocation()
        {
            var location = "41°54.5'N 12°29.5'E";

            var sut = GpsLocation.Parse(location);

            sut.Latitude.Should().BeInRange(41.9, 42.0);
            sut.Longitude.Should().BeInRange(12.4, 12.5);
        }


        [TestMethod]
        public void Parse_Degree_Minutes_With_Spaces_To_GpsLocation()
        {
            var location = "41° 54.5' N 12° 29.5' E";

            var sut = GpsLocation.Parse(location);

            sut.Latitude.Should().BeInRange(41.9, 42.0);
            sut.Longitude.Should().BeInRange(12.4, 12.5);
        }
        [TestMethod]
        public void Parse_Degrees_Without_Spaces_To_GpsLocation()
        {
            var location = "41.95°N 12.45°E";

            var sut = GpsLocation.Parse(location);

            sut.Latitude.Should().BeInRange(41.9, 42.0);
            sut.Longitude.Should().BeInRange(12.4, 12.5);
        }


        [TestMethod]
        public void Parse_Degrees_With_Spaces_To_GpsLocation()
        {
            var location = "41.95° N 12.45° E";

            var sut = GpsLocation.Parse(location);

            sut.Latitude.Should().BeInRange(41.9, 42.0);
            sut.Longitude.Should().BeInRange(12.4, 12.5);
        }

        [TestMethod]
        public void Parse_Degrees_With_Minus_Sign_To_GpsLocation()
        {
            var location = "-41.95 12.45";

            var sut = GpsLocation.Parse(location);

            sut.Latitude.Should().BeInRange(-42.0, -41.9);
            sut.Longitude.Should().BeInRange(12.4, 12.5);
        }

        [TestMethod]
        public void Parse_Degrees_With_Comma_Separator()
        {
            var location = "-41.95,12.45";

            var sut = GpsLocation.Parse(location);

            sut.Latitude.Should().BeInRange(-42.0, -41.9);
            sut.Longitude.Should().BeInRange(12.4, 12.5);
        }

        [TestMethod]
        public void Parse_Degrees_With_Comma_Decimal_Separator()
        {
            var location = "-41,95 12,45";

            var sut = GpsLocation.Parse(location, CultureInfo.GetCultureInfo(1043));

            sut.Latitude.Should().BeInRange(-42.0, -41.9);
            sut.Longitude.Should().BeInRange(12.4, 12.5);
        }

        [TestMethod]
        public void Parse_Degrees_With_Comma_Decimal_Separator_Not_Allowed_To_Use_Comma_As_Separator()
        {
            var location = "-41,95,12,45";

            Action action = () => GpsLocation.Parse(location, CultureInfo.GetCultureInfo(1043));

            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Parse_Degrees_With_Semicolon_Separator()
        {
            var location = "-41.95;12.45";

            var sut = GpsLocation.Parse(location);

            sut.Latitude.Should().BeInRange(-42.0, -41.9);
            sut.Longitude.Should().BeInRange(12.4, 12.5);
        }

        [TestMethod]
        public void TryParse_Does_Not_Throw_Exception()
        {
            GpsLocation.TryParse("abc", out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_With_A_Location_That_Is_Out_Of_The_Range_Of_Expected_Values_Does_Not_Throw_An_Exception()
        {
            GpsLocation.TryParse("400 400", out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_When_The_Format_Does_Not_Match_The_Decimal_Separator_Dot_Returns_False()
        {
            GpsLocation.TryParse("-41.95;12.45", CultureInfo.GetCultureInfo(1043), out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_When_The_Format_Matches_The_Decimal_Separator_Dot_Returns_True()
        {
            GpsLocation.TryParse("-41.95;12.45", CultureInfo.InvariantCulture, out var _).Should().BeTrue();
        }


        [TestMethod]
        public void TryParse_When_The_Format_Does_Not_Match_The_Decimal_Separator_Comma_Returns_False()
        {
            GpsLocation.TryParse("-41,95;12,45", CultureInfo.InvariantCulture, out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_When_The_Format_Matches_The_Decimal_Separator_Comma_Returns_True()
        {
            GpsLocation.TryParse("-41,95;12,45", CultureInfo.GetCultureInfo(1043), out var _).Should().BeTrue();
        }

        private static GpsLocation Create(Tuple<double, double> tuple)
        {
            return new GpsLocation(tuple.Item1, tuple.Item2);
        }

        [TestMethod]
        public void DataContract_Serializable()
        {
            var sut = Create(_athens);
            sut.Should().BeDataContractSerializable();
        }

        [TestMethod]
        public void Xml_Serializable()
        {
            var sut = Create(_athens);
            sut.Should().BeXmlSerializable();
        }

        [TestMethod]
        public void Binary_Serializable()
        {
            var sut = Create(_athens);
            sut.Should().BeBinarySerializable();
        }
    }
}
