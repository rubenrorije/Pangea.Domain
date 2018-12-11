using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests.Formatters
{
    [TestClass]
    public class PhoneNumberNLFormatterTests
    {
        private static readonly PhoneNumber _mobilePhone = new PhoneNumber(31, "0623456789");

        [TestMethod]
        public void Format_Mobile_Phone_Number()
        {
            var sut = new PhoneNumberNLFormatter();

            _mobilePhone.ToString("L", sut).Should().Be("06-23 45 67 89");
            _mobilePhone.ToString("G", sut).Should().Be("+31 6 23 45 67 89");
        }

        [TestMethod]
        public void Format_Area_Codes()
        {
            var sut = new PhoneNumberNLFormatter();
            sut.GetLocalFormat(new PhoneNumber(31, "0101234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0131234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0141234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0151234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0201234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0231234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0241234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0261234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0301234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0331234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0361234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0381234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0401234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0431234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0441234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0451234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0461234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0501234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0531234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0581234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0701234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0711234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0721234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0731234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0741234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0751234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0761234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0771234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0781234567")).Should().Be("0NN-NNNNNNN");
            sut.GetLocalFormat(new PhoneNumber(31, "0791234567")).Should().Be("0NN-NNNNNNN");
        }

        [TestMethod]
        public void FormatStrings_That_Are_Not_Allowed_Throw_FormatException()
        {
            var sut = new PhoneNumberNLFormatter();

            Action action = () => _mobilePhone.ToString("X", sut);
            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void AppliesTo_For_31()
        {
            var sut = new PhoneNumberNLFormatter();
            sut.AppliesTo("31").Should().BeTrue();
        }
    }
}
