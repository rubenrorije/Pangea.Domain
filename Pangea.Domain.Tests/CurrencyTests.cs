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
    public class CurrencyTests
    {
        [TestMethod]
        public void Euro_Currency_Symbol()
        {
            DefaultCurrencies.EUR.Symbol.Should().Be("€");
        }

        [TestMethod]
        public void Euro_Currency_ISO()
        {
            DefaultCurrencies.EUR.Code.Should().Be("EUR");
        }

        [TestMethod]
        public void Create_Currency_With_Code()
        {
            new Currency("AED").Code.Should().Be("AED");
        }

        [TestMethod]
        public void Create_Currency_With_Null_Code_Throws_ArgumentNullException()
        {
            Action action = () => new Currency(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Create_Currency_With_Code_That_Does_Not_Have_Three_Letters_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => new Currency("AEDV");
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Create_Currency_With_LowerCase_Code_Throws_ArgumentOutOfRangeException()
        {
            Action action = ()=> new Currency("aed");
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ToString_With_Format_G_Returns_The_Code()
        {
            DefaultCurrencies.EUR.ToString("G").Should().Be("EUR");
        }

        [TestMethod]
        public void ToString_With_Format_null_Returns_The_Code()
        {
            DefaultCurrencies.EUR.ToString(null).Should().Be("EUR");
        }

        [TestMethod]
        public void ToString_With_Format_S_Returns_The_Symbol()
        {
            DefaultCurrencies.EUR.ToString("S").Should().Be("€");
        }

        [TestMethod]
        public void ToString_With_Unknown_Format_Throws_FormatException()
        {
            Action action = () => DefaultCurrencies.EUR.ToString("X");

            action.Should().Throw<FormatException>();
        }


        [TestMethod]
        public void Equals_Must_Only_Use_Code()
        {
            var lhs = new Currency("ABC");
            var rhs = new Currency("ABC", "X");
            lhs.Equals(rhs).Should().BeTrue("the currencies should be the same when the code is the same");
        }

        [TestMethod]
        public void GetHashCode_Returns_The_Same_Value_For_The_Same_Code()
        {
            var lhs = new Currency("ABC", "X");
            var rhs = new Currency("ABC", "Y");
            lhs.GetHashCode().Equals(rhs.GetHashCode()).Should().BeTrue("the currencies should have the same hashcode when the code is the same");
        }
    }
}
