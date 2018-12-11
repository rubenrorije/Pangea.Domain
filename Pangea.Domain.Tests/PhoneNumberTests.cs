using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class PhoneNumberTests
    {
        [TestMethod]
        public void Null_Text_Does_Not_Throw_Exception()
        {
            var sut = new PhoneNumber(null);
        }

        [TestMethod]
        public void Empty_Text_Does_Not_Throw_Exception()
        {
            var sut = new PhoneNumber(string.Empty);
        }

        [TestMethod]
        public void Invalid_PhoneNumber_Throws_Exception()
        {
            Action action = () => new PhoneNumber("abc");
            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void Parse_International_PhoneNumber()
        {
            var sut = new PhoneNumber("+31123456789");
            sut.ToString().Should().Be("+31123456789");
        }

        [TestMethod]
        public void Parse_International_PhoneNumber_With_Spaces()
        {
            var sut = new PhoneNumber("+31 12 34 56 789");
            sut.ToString().Should().Be("+31 12 34 56 789");
        }

        [TestMethod]
        public void Parse_International_PhoneNumber_With_Zeros_And_Spaces()
        {
            var sut = new PhoneNumber("0031 12 34 56 789");
            sut.ToString().Should().Be("+31 12 34 56 789");
        }

        [TestMethod]
        public void Country_Code_Can_Be_Resolved()
        {
            CountryCodes.SetProvider(() => new DefaultCountryProvider());
            var sut = new PhoneNumber("+31123456789");
            sut.CountryCode.Should().Be(31);
            CountryCodes.ClearProvider();
        }

        [TestMethod]
        public void ToString_Returns_The_Representation_Of_The_PhoneNumber()
        {
            var sut = new PhoneNumber("+31123456789");
            sut.ToString().Should().Be("+31123456789");
        }

        [TestMethod]
        public void ToString_Returns_The_Representation_Of_The_PhoneNumber_Including_The_Original_Spaces()
        {
            var sut = new PhoneNumber("+31 12 345 6789");
            sut.ToString().Should().Be("+31 12 345 6789");
        }

        [TestMethod]
        public void ToString_G_Returns_The_Representation_Of_The_PhoneNumber_Including_The_Original_Spaces()
        {
            var sut = new PhoneNumber("+31 12 345 6789");
            sut.ToString("G").Should().Be("+31 12 345 6789");
        }
        [TestMethod]
        public void ToString_g_Returns_The_Representation_Of_The_PhoneNumber_Excluding_The_Original_Spaces()
        {
            var sut = new PhoneNumber("+31 12 345 6789");
            sut.ToString("g").Should().Be("+31123456789");
        }

        [TestMethod]
        public void ToString_O_Returns_The_Representation_Of_The_PhoneNumber_Including_The_Original_Spaces()
        {
            var sut = new PhoneNumber("+31 12 345 6789");
            sut.ToString("O").Should().Be("0031 12 345 6789");
        }

        [TestMethod]
        public void ToString_o_Returns_The_Representation_Of_The_PhoneNumber_Excluding_The_Original_Spaces()
        {
            var sut = new PhoneNumber("+31 12 345 6789");
            sut.ToString("o").Should().Be("0031123456789");
        }

        [TestMethod]
        public void ToString_l_Returns_The_Representation_Of_The_PhoneNumber_Excluding_The_Original_Spaces()
        {
            CountryCodes.SetProvider(() => new DefaultCountryProvider());
            var sut = new PhoneNumber("+31 12 345 6789");
            sut.ToString("l").Should().Be("0123456789");
            CountryCodes.ClearProvider();
        }

        [TestMethod]
        public void ToString_L_Returns_The_Representation_Of_The_PhoneNumber_Excluding_The_Original_Spaces()
        {
            CountryCodes.SetProvider(() => new DefaultCountryProvider());
            var sut = new PhoneNumber("+31 12 345 6789");
            sut.ToString("L").Should().Be("0 12 345 6789");
            CountryCodes.ClearProvider();
        }

        [TestMethod]
        public void ToString_L_When_Country_Code_Not_Found_Still_Is_The_International_Format()
        {
            var sut = new PhoneNumber("+31 12 345 6789");
            sut.ToString("L").Should().Be("+31 12 345 6789");
        }


        [TestMethod]
        public void ToString_With_A_Format_That_Is_Unuseful_Returns_The_Text()
        {
            var sut = new PhoneNumber("+31 12 345 6789");
            sut.ToString("X").Should().Be("X");
        }

        [TestMethod]
        public void ToString_For_Empty_PhoneNumber_Does_Not_Throw_An_Exception_And_Returns_Null()
        {
            var sut = new PhoneNumber();

            sut.ToString("g").Should().Be(null);
            sut.ToString("G").Should().Be(null);
            sut.ToString("o").Should().Be(null);
            sut.ToString("O").Should().Be(null);
            sut.ToString("l").Should().Be(null);
            sut.ToString("L").Should().Be(null);
        }

        [TestMethod]
        public void Create_A_Local_Phone_Number_With_A_Country_Code()
        {
            var sut = new PhoneNumber(31, "0 12 34 56 789");
            sut.ToString().Should().Be("+31 12 34 56 789");
        }

        [TestMethod]
        public void Create_An_Empty_PhoneNumber_With_A_Country_Code()
        {
            var sut = new PhoneNumber(31, null);
            sut.ToString().Should().Be(null);
            sut.CountryCode.Should().Be(null);
            sut.Should().Be(new PhoneNumber());
        }

        [TestMethod]
        public void CountryCode_Can_Not_Be_Zero()
        {
            Action action = () => new PhoneNumber(0, null);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Serialize()
        {
            var sut = new PhoneNumber("+31123456789");
            var text = new StringBuilder();
            using (var writer = XmlWriter.Create(text))
            {
                sut.WriteXml(writer);
            }
            text.ToString().Should().Contain("<value>+31123456789</value>");
        }

        [TestMethod]
        public void Deserialize()
        {
            using (var tr = new StringReader("<value>+31 1234 56789</value>"))
            using (var reader = XmlReader.Create(tr))
            {
                var sut = new PhoneNumber();
                sut.ReadXml(reader);

                sut.ToString().Should().Be("+31 1234 56789");
            }
        }

        [TestMethod]
        public void CountryCode_Can_Not_Be_Less_Than_Zero()
        {
            Action action = () => new PhoneNumber(-1, null);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Create_A_Local_Phone_Number_With_A_Wrong_Format_Throws_A_FormatException()
        {
            Action action = () => new PhoneNumber(31, "12 34 56 789");

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void Equals_Returns_True_For_Similar_PhoneNumbers()
        {
            var one = new PhoneNumber("+31 12 345 6789");
            var other = new PhoneNumber("+31 12 3456789");

            one.Should().Be(other);
        }

        [TestMethod]
        public void Equals_With_Empty_PhoneNumber_Returns_False()
        {
            var one = new PhoneNumber("+31 12 345 6789");
            var other = new PhoneNumber();

            (one.Equals(other)).Should().BeFalse();
        }

        [TestMethod]
        public void HashCode_Returns_0_For_Empty_PhoneNumber()
        {
            var sut = new PhoneNumber();
            sut.GetHashCode().Should().Be(0);
        }

        [TestMethod]
        public void Operator_Equals_Returns_True_For_Similar_PhoneNumbers()
        {
            var one = new PhoneNumber("+31 12 345 6789");
            var other = new PhoneNumber("+31 12 3456789");

            (one == other).Should().BeTrue();
        }

        [TestMethod]
        public void Operator_Not_Equals_Returns_False_For_Similar_PhoneNumbers()
        {
            var one = new PhoneNumber("+31 12 345 6789");
            var other = new PhoneNumber("+31 12 3456789");

            (one != other).Should().BeFalse();
        }

        [TestMethod]
        public void Hashcode_Returns_The_Same_For_Similar_PhoneNumbers()
        {
            var one = new PhoneNumber("+31 12 345 6789");
            var other = new PhoneNumber("+31 12 3456789");

            one.GetHashCode().Should().Be(other.GetHashCode());
        }

        [TestMethod]
        public void TryParse_PhoneNumber_Null_Returns_True()
        {
            PhoneNumber.TryParse(null, out var _).Should().BeTrue();
        }

        [TestMethod]
        public void TryParse_PhoneNumber_String_Empty_Returns_True()
        {
            PhoneNumber.TryParse(string.Empty, out var _).Should().BeTrue();
        }

        [TestMethod]
        public void TryParse_PhoneNumber_That_Is_Invalid_Returns_False()
        {
            PhoneNumber.TryParse("abc", out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_PhoneNumber_That_Is_Valid_With_Plus_Returns_The_Parsed_PhoneNumber()
        {
            PhoneNumber sut;
            PhoneNumber.TryParse("+31 123456789", out sut).Should().BeTrue();
            sut.ToString().Should().Be("+31 123456789");
        }

        [TestMethod]
        public void TryParse_PhoneNumber_That_Is_Valid_With_Zeros_Returns_The_Parsed_PhoneNumber()
        {
            PhoneNumber sut;
            PhoneNumber.TryParse("0031 123456789", out sut).Should().BeTrue();
            sut.ToString().Should().Be("+31 123456789");
        }

        [TestMethod]
        public void TryParse_Local_PhoneNumber()
        {
            PhoneNumber sut;
            PhoneNumber.TryParse(31, "0123456789", out sut).Should().BeTrue();
            sut.ToString().Should().Be("+31123456789");
        }

        [TestMethod]
        public void TryParse_Local_PhoneNumber_With_Invalid_Country_Code_Returns_False()
        {
            PhoneNumber sut;
            PhoneNumber.TryParse(0, "0123456789", out sut).Should().BeFalse();
        }

        [TestMethod]
        public void Format_With_Custom_Format()
        {
            var sut = new PhoneNumber(31, "0123456789");

            sut.ToString("+C NNN NN NNN N").Should().Be("+31 123 45 678 9");
            sut.ToString("+C (0) NNN NN NN NN").Should().Be("+31 (0) 123 45 67 89");
            sut.ToString("0NNN-NNNNNN").Should().Be("0123-456789");
        }
    }
}
