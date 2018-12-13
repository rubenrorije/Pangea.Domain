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
    public class PhoneNumberBEFormatterTests
    {
        [TestMethod]
        [DataRow(32, true)]
        [DataRow(31, false)]
        [DataRow(33, false)]
        public void AppliesTo_Correct_Country(int countryCode, bool result)
        {
            var sut = new PhoneNumberBEFormatter();
            sut.AppliesTo(countryCode).Should().Be(result);
        }

        [TestMethod]
        [DataRow("02 12345678")]
        [DataRow("03 12345678")]
        [DataRow("04 12345678")]
        [DataRow("09 12345678")]
        public void Format_Big_Cities_Correctly(string text)
        {
            var sut = new PhoneNumberBEFormatter();
            sut.GetLocalFormat(new PhoneNumber(32, text)).Should().Be("0N NNNNNNNN");
        }

        [TestMethod]
        [DataRow("011 2345678")]
        [DataRow("051 2345678")]
        [DataRow("061 2345678")]
        [DataRow("071 2345678")]
        [DataRow("081 2345678")]
        public void Format_Smaller_Cities_Correctly(string text)
        {
            var sut = new PhoneNumberBEFormatter();
            sut.GetLocalFormat(new PhoneNumber(32, text)).Should().Be("0NN NNNNNNN");
        }

        [TestMethod]
        [DataRow("0451 234567")]
        [DataRow("0452 234567")]
        [DataRow("0453 234567")]
        [DataRow("0454 234567")]
        [DataRow("0455 234567")]
        [DataRow("0456 234567")]
        [DataRow("0457 234567")]
        [DataRow("0458 234567")]
        [DataRow("0459 234567")]
        public void Format_Mobile_Phone_Correctly(string text)
        {
            var sut = new PhoneNumberBEFormatter();
            sut.GetLocalFormat(new PhoneNumber(32, text)).Should().Be("0NNN NNNNNN");
        }


    }
}
