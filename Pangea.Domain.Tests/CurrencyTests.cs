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
        public void Euro_Currency_Numeric()
        {
            DefaultCurrencies.EUR.Numeric.Should().Be(978);
        }

        [TestMethod]
        public void Create_Currency_With_Code()
        {
            new Currency("AED", 784).Code.Should().Be("AED");
        }

        [TestMethod]
        public void Create_Currency_With_Null_Code_Throws_ArgumentNullException()
        {
            Action action = () => new Currency(null, 784);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Create_Currency_With_Code_That_Does_Not_Have_Three_Letters_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => new Currency("AEDV", 784);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Create_Currency_With_LowerCase_Code_Throws_ArgumentOutOfRangeException()
        {
            Action action = ()=> new Currency("aed", 784);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Create_Currency_With_Negative_Numeric_Code_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => new Currency("AED", -784);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Create_Currency_With_Numeric_Code()
        {
            new Currency("AED", 784).Numeric.Should().Be(784);
        }

        [TestMethod]
        public void ToString_Returns_The_Code()
        {
            new Currency("AED", 784).ToString().Should().Be("AED");
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
        public void ToString_With_Format_N_Returns_The_Numeric_Code()
        {
            DefaultCurrencies.EUR.ToString("N").Should().Be("978");
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
            var lhs = new Currency("ABC", 123);
            var rhs = new Currency("ABC", 456);
            lhs.Equals(rhs).Should().BeTrue("the currencies should be the same when the code is the same");
        }

        [TestMethod]
        public void GetHashCode_Returns_The_Same_Value_For_The_Same_Code()
        {
            var lhs = new Currency("ABC", 123);
            var rhs = new Currency("ABC", 456);
            lhs.GetHashCode().Equals(rhs.GetHashCode()).Should().BeTrue("the currencies should have the same hashcode when the code is the same");
        }
    }
}
