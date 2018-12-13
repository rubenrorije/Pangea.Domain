using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class CreditCardTests
    {
        [TestMethod]
        public void CreditCard_Can_Be_Created_With_Null_Text()
        {
            new CreditCard(null);
        }

        [TestMethod]
        public void CreditCard_Can_Be_Created_With_Empty_Text()
        {
            new CreditCard(string.Empty);
        }

        [TestMethod]
        public void CreditCard_Must_Be_At_Least_8_Characters()
        {
            Action action = () => new CreditCard("1234567");
            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*at least 8 characters*");
        }

        [TestMethod]
        public void CreditCard_Must_Be_Less_Than_20_Characters()
        {
            Action action = () => new CreditCard("12345678901234567890");
            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*less than 20 characters*");
        }


        [TestMethod]
        public void Get_The_Issuer_Identification_Of_An_Empty_CreditCard_Must_Be_Null()
        {
            new CreditCard().GetIssuerIdentificationNumber(CreditCard.IssuerIdentifierFormat.LongIdentifier).Should().Be(null);
        }

        [TestMethod]
        public void Get_The_Individual_Account_Number_Of_An_Empty_CreditCard_Must_Be_Null()
        {
            new CreditCard().GetIssuerIdentificationNumber(CreditCard.IssuerIdentifierFormat.LongIdentifier).Should().Be(null);
        }

        [TestMethod]
        public void Create_Example_Credit_Cards()
        {
            new CreditCard("4111 1111 1111 1111");
            new CreditCard("5500 0000 0000 0004");
            new CreditCard("3400 0000 0000 009");
            new CreditCard("3000 0000 0000 04");
        }

        [TestMethod]
        public void Create_Example_Credit_Cards_Without_Spaces()
        {
            new CreditCard("4111111111111111");
            new CreditCard("5500000000000004");
            new CreditCard("340000000000009");
            new CreditCard("30000000000004");
        }

        [TestMethod]
        public void Spaces_Are_Ignored()
        {
            var lhs = new CreditCard("4111 1111 1111 1111");
            var rhs = new CreditCard("4111111111111111");
            lhs.Should().Be(rhs);
        }

        [TestMethod]
        public void Default_CreditCard_ToString_Returns_Null()
        {
            new CreditCard().ToString().Should().Be(null);
        }

        [TestMethod]
        public void Empty_CreditCard_ToString_Returns_Empty_String()
        {
            new CreditCard(string.Empty).ToString().Should().Be(string.Empty);
        }

        [TestMethod]
        public void ToString_Returns_Grouped_In_4()
        {
            var sut = new CreditCard("4111111111111111");
            sut.ToString().Should().Be("4111 1111 1111 1111");
        }

        [TestMethod]
        public void A_CreditCard_Number_Cannot_Contain_Letters()
        {
            Action action = () => new CreditCard("4111111111111111Z");
            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*can only contain digits or spaces*");
        }

        [TestMethod]
        public void Get_The_Issuer_Identification_Of_A_CreditCard()
        {
            var sut = new CreditCard("4111 1111 1111 1111");

            sut.GetIssuerIdentificationNumber(CreditCard.IssuerIdentifierFormat.LongIdentifier).Should().Be("41111111");
            sut.GetIssuerIdentificationNumber(CreditCard.IssuerIdentifierFormat.ShortIdentifier).Should().Be("411111");
        }

        [TestMethod]
        public void Get_The_Individual_Account_Number_Of_A_CreditCard()
        {
            var sut = new CreditCard("4111 1111 1111 1111");

            sut.GetIndividualAccountNumber(CreditCard.IssuerIdentifierFormat.LongIdentifier).Should().Be("1111111");
            sut.GetIndividualAccountNumber(CreditCard.IssuerIdentifierFormat.ShortIdentifier).Should().Be("111111111");
        }

        [TestMethod]
        public void Validate_The_CheckSum_Digit()
        {
            Action action = () => new CreditCard("4111 1111 1111 1112");
            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*checksum*");
        }

        [TestMethod]
        public void TryParse_CreditCard_That_Is_Null_Is_Valid()
        {
            CreditCard.TryParse(null, out var _).Should().BeTrue();
        }
        [TestMethod]
        public void TryParse_CreditCard_That_Is_Empty_Is_Valid()
        {
            CreditCard.TryParse(string.Empty, out var _).Should().BeTrue();
        }

        [TestMethod]
        public void TryParse_CreditCard_That_Is_Invalid_Does_Not_Throw_An_Exception()
        {
            CreditCard.TryParse("abc", out var _).Should().BeFalse();
        }

        [TestMethod]
        public void Credit_Card_Must_Have_At_Least_8_Digits()
        {
            // 8 chars, but not 8 digits
            Action action = () => new CreditCard(" 1234567");
            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*8*");
        }

        [TestMethod]
        public void Credit_Card_Must_Have_Maximum_19_Digits()
        {
            // 20 chars, but 19 digits
            Action action = () => new CreditCard("1234567890123456785 ");
            action.Should().NotThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void TryParse_CreditCard_That_Is_Invalid_Because_Of_Invalid_Checksum_Does_Not_Throw_An_Exception()
        {
            CreditCard.TryParse("4111 1111 1111 1112", out var _).Should().BeFalse();
        }

        [TestMethod]
        public void Unsafe_Does_Not_Check_Any_Validation_For_The_CreditCard()
        {
            Action action = () => CreditCard.Unsafe("1");
            action.Should().NotThrow();
        }
    }
}
