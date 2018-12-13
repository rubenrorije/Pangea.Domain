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
    public class PhoneNumberAggregateFormatterTests
    {
        [TestMethod]
        public void Cannot_Create_With_A_Null_Argument()
        {
            Action action = ()=> new PhoneNumberAggregateFormatter(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Applies_Redirects_To_Inner()
        {
            var sut = new PhoneNumberAggregateFormatter(new PhoneNumberNLFormatter());

            sut.AppliesTo(31).Should().BeTrue();
            sut.AppliesTo(32).Should().BeFalse();
        }

        [TestMethod]
        public void Constructor_Initialization()
        {
            var sut = 
                new PhoneNumberAggregateFormatter
                {
                    new PhoneNumberNLFormatter(),
                    new PhoneNumberBEFormatter()
                };

            sut.Should().HaveCount(2);
        }

        [TestMethod]
        [DataRow(32, "0459 234567", "0459 234567")]
        [DataRow(31, "0791234567", "079-1234567")]
        public void Format_Using_The_Right_Formatter_LocalFormat(int country, string phoneNumber, string expectedFormat)
        {
            var sut =
                new PhoneNumberAggregateFormatter
                {
                    new PhoneNumberNLFormatter(),
                    new PhoneNumberBEFormatter()
                };

            var phone = new PhoneNumber(country, phoneNumber);

            phone.ToString("L", sut).Should().Be(expectedFormat);
        }
        [TestMethod]
        [DataRow(32, "0459 234567", "+32 459 234567")]
        [DataRow(31, "0791234567", "+31 79-1234567")]
        public void Format_Using_The_Right_Formatter_GlobalFormat(int country, string phoneNumber, string expectedFormat)
        {
            var sut =
                new PhoneNumberAggregateFormatter
                {
                    new PhoneNumberNLFormatter(),
                    new PhoneNumberBEFormatter()
                };

            var phone = new PhoneNumber(country, phoneNumber);

            phone.ToString("G", sut).Should().Be(expectedFormat);
        }
    }
}
