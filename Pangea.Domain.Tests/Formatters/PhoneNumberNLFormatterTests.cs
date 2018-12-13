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
            _mobilePhone.ToString("G", sut).Should().Be("+31 6-23 45 67 89");
        }

        [TestMethod]
        [DataRow("0101234567")]
        [DataRow("0131234567")]
        [DataRow("0141234567")]
        [DataRow("0151234567")]
        [DataRow("0201234567")]
        [DataRow("0231234567")]
        [DataRow("0241234567")]
        [DataRow("0261234567")]
        [DataRow("0301234567")]
        [DataRow("0331234567")]
        [DataRow("0361234567")]
        [DataRow("0381234567")]
        [DataRow("0401234567")]
        [DataRow("0431234567")]
        [DataRow("0441234567")]
        [DataRow("0451234567")]
        [DataRow("0461234567")]
        [DataRow("0501234567")]
        [DataRow("0531234567")]
        [DataRow("0581234567")]
        [DataRow("0701234567")]
        [DataRow("0711234567")]
        [DataRow("0721234567")]
        [DataRow("0731234567")]
        [DataRow("0741234567")]
        [DataRow("0751234567")]
        [DataRow("0761234567")]
        [DataRow("0771234567")]
        [DataRow("0781234567")]
        [DataRow("0791234567")]
        public void Format_Area_Codes(string phoneNumber)
        {
            var sut = new PhoneNumberNLFormatter();
            sut.GetLocalFormat(new PhoneNumber(31, phoneNumber)).Should().Be("0NN-NNNNNNN");
        }
        
        [TestMethod]
        public void Format_0800()
        {
            var sut = new PhoneNumberNLFormatter();
            sut.GetLocalFormat(new PhoneNumber(31, "08004444")).Should().Be("0NNN-NNNNNN");
            new PhoneNumber(31, "0800 4444").ToString("L", sut).Should().Be("0800-4444");
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
            sut.AppliesTo(31).Should().BeTrue();
        }
    }
}
