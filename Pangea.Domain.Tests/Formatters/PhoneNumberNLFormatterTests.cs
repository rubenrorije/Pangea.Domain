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

            _mobilePhone.ToString("L", sut).Should().Be("06-23456789");
            _mobilePhone.ToString("G", sut).Should().Be("+31 6 23456789");
        }

        [TestMethod]
        public void AppliesTo_For_31()
        {
            var sut = new PhoneNumberNLFormatter();
            sut.AppliesTo("31").Should().BeTrue();
        }
    }
}
