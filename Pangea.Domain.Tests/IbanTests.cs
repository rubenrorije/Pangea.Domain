using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class IbanTests
    {
        [TestMethod]
        public void Create_Empty_Iban()
        {
            new Iban();
        }

        [TestMethod]
        public void Create_Empty_Iban_From_Empty_String()
        {
            var sut = new Iban(string.Empty);
        }

        [TestMethod]
        public void Empty_Iban_ToString_Returns_String_Empty()
        {
            var sut = new Iban();
            sut.ToString().Should().Be(string.Empty);
        }

        [TestMethod]
        public void Create_Iban_With_Invalid_Char_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => new Iban("NL45 1234 1234 $");
            action
                .Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("*letters*");
        }

        [TestMethod]
        public void Equals_Of_Default_Iban_Does_Not_Throw_Exception()
        {
            new Iban().Equals(new Iban()).Should().BeTrue();
        }

        [TestMethod]
        [DataRow("BE71 0961 2345 6769")]
        [DataRow("FR76 3000 6000 0112 3456 7890 189")]
        [DataRow("DE91 1000 0000 0123 4567 89")]
        [DataRow("GR96 0810 0010 0000 0123 4567 890")]
        [DataRow("RO09 BCYP 0000 0012 3456 7890")]
        [DataRow("SA44 2000 0001 2345 6789 1234")]
        [DataRow("ES79 2100 0813 6101 2345 6789")]
        [DataRow("CH56 0483 5012 3456 7800 9")]
        [DataRow("GB98 MIDL 0700 9312 3456 78")]
        public void Test_Ibans_From_Wikipedia_Must_Parse(string iban)
        {
            new Iban(iban).ToString().Should().Be(iban);
        }

        [TestMethod]
        public void Country_Code_Should_Be_Letters()
        {
            Action action = () => new Iban("1298 MIDL 0700 9312 3456 78");
            action
                .Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithMessage("*country code*");
        }

        [TestMethod]
        public void Leading_Or_Trailing_Spaces_Do_Not_Matter()
        {
            new Iban("    BE71 0961 2345 6769    ");
        }

        [TestMethod]
        public void Iban_Does_Not_Have_To_Be_Grouped()
        {
            new Iban("BE71096123456769");
        }

        [TestMethod]
        public void Lower_Case_Iban_Is_Allowed_And_Automatically_Uppercased()
        {
            new Iban("be71096123456769");
        }

        [TestMethod]
        public void Country_Code_Is_Extracted()
        {
            var sut = new Iban("BE71096123456769");
            sut.CountryCode.Should().Be("BE");
        }

        [TestMethod]
        public void Country_Code_Is_UpperCase()
        {
            var sut = new Iban("be71096123456769");
            sut.CountryCode.Should().Be("BE");
        }

        [TestMethod]
        public void Check_Digits_Are_Extracted()
        {
            var sut = new Iban("be71096123456769");
            sut.CheckDigits.Should().Be("71");
        }

        [TestMethod]
        public void Validate_The_Check_Digits()
        {
            Action action = () => new Iban("be71096123456869");
            action.Should().Throw<ArgumentOutOfRangeException>("*incorrect*");
        }

        [TestMethod]
        public void Equality_Ignores_Whitespace()
        {
            var lhs = new Iban("BE71 0961 2345 6769");
            var rhs = new Iban("BE 71 096123456769");

            lhs.Equals(rhs).Should().BeTrue();
            (lhs == rhs).Should().BeTrue();
            (lhs != rhs).Should().BeFalse();
        }

        [TestMethod]
        public void Hashcode_Ignores_Whitespace()
        {
            var lhs = new Iban("BE71 0961 2345 6769");
            var rhs = new Iban("BE 71 096123456769");

            lhs.GetHashCode().Equals(rhs.GetHashCode()).Should().BeTrue();
        }

        [TestMethod]
        public void Unsafe_Ignores_Validation()
        {
            // the check digits are incorrect.
            var sut = Iban.Unsafe("BE81 0961 2345 6769");
            sut.CountryCode.Should().Be("BE");
            sut.CheckDigits.Should().Be("81");
            sut.BasicBankAccountNumber.Should().Be("096123456769");
            sut.ToString().Should().Be("BE81 0961 2345 6769");
        }

        [TestMethod]
        public void Serialization_Of_Iban()
        {
            var sut = new Iban("BE71 0961 2345 6769");

            sut.Should().BeXmlSerializable();
            sut.Should().BeBinarySerializable();
            sut.Should().BeDataContractSerializable();
        }

        [TestMethod]
        public void TryParse_Null_Returns_True()
        {
            Iban.TryParse(null, out var _).Should().BeTrue();
        }


        [TestMethod]
        public void TryParse_String_Empty_Returns_True()
        {
            Iban.TryParse(string.Empty, out var _).Should().BeTrue();
        }

        [TestMethod]
        public void TryParse_Iban_That_Is_Too_Long_Returns_False()
        {
            Iban.TryParse(new string('1', 35), out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_Iban_That_Is_Too_Short_Returns_False()
        {
            Iban.TryParse(new string('1', 3), out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_Iban_That_Is_In_Wrong_Format_Returns_False()
        {
            Iban.TryParse(new string('1', 30), out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_Iban_That_Has_LowerCase_CountryCode_Returns_False()
        {
            Iban.TryParse("nl12 3456789", out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_Iban_That_Has_Letters_As_Check_Digits_Returns_False()
        {
            Iban.TryParse("NLA1 3456789", out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_Iban_That_Has_Other_Characters_As_Bank_Account_Number_Returns_False()
        {
            Iban.TryParse("NLA1 34$56789", out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_Iban_That_Has_Incorrect_CheckDigit_Returns_False()
        {
            Iban.TryParse("BE72 0961 2345 6769", out var _).Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_Iban_That_Is_Correct_Returns_True()
        {
            Iban.TryParse("BE71 0961 2345 6769", out var _).Should().BeTrue();
            new Iban("BE71 0961 2345 6769");
        }

        [TestMethod]
        public void DataContract_Serializable()
        {
            var sut = new Iban("BE71 0961 2345 6769");
            sut.Should().BeDataContractSerializable();
        }

        [TestMethod]
        public void Xml_Serializable()
        {
            var sut = new Iban("BE71 0961 2345 6769");
            sut.Should().BeXmlSerializable();
        }

        [TestMethod]
        public void Binary_Serializable()
        {
            var sut = new Iban("BE71 0961 2345 6769");
            sut.Should().BeBinarySerializable();
        }

        [TestMethod]
        public void Invalid_Iban_Must_Be_Serializable_And_Not_Throw_On_Deserialization()
        {
            var sut = Iban.Unsafe("BE71 0962 2345 6769"); // invalid check digit

            sut.Should().BeBinarySerializable();
            sut.Should().BeXmlSerializable();
            sut.Should().BeDataContractSerializable();
        }

    }
}
