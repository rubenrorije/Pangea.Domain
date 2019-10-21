using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pangea.Domain.DefaultCurrencies;
namespace Pangea.Domain.Tests
{
    [TestClass]
    public class ExchangeRateAtTests
    {
        [TestMethod]
        public void Empty_Rate_Throws_ArgumentNullException()
        {
            Action action = () => new ExchangeRateAt(DateTime.Today, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void MinDate_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => new ExchangeRateAt(DateTime.MinValue, new ExchangeRate(EUR, USD, 1.1m));
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Forward_Multiply_Operator()
        {
            var rate = new ExchangeRate(EUR, USD, 1.1m);
            var rateAt = new ExchangeRateAt(DateTime.Today, rate);

            (100 * rateAt).Should().Be(110m);
        }

        [TestMethod]
        public void Forward_Divide_Operator()
        {
            var rate = new ExchangeRate(EUR, USD, 1.25m);
            var rateAt = new ExchangeRateAt(DateTime.Today, rate);

            (100 / rateAt).Should().Be(80m);
        }

        [TestMethod]
        public void ToString_Returns_The_Text_Format()
        {
            var sut = new ExchangeRateAt(DateTime.Today, EUR, USD, 1.25m);

            var text = sut.ToString();
            text.Should().Contain(DateTime.Today.ToString("d"));
            text.Should().Contain(EUR.Code);
            text.Should().Contain(USD.Code);
            text.Should().Contain(1.25m.ToString());
        }

        [TestMethod]
        public void ToString_With_A_Different_Format()
        {
            var sut = new ExchangeRateAt(DateTime.Today, EUR, USD, 1.25m);
            var text = sut.ToString("s|N2");
            
            text.Should().Contain(DateTime.Today.ToString("s"));
            text.Should().Contain(1.25m.ToString("N2"));
        }

        [TestMethod]
        public void Equals_Null_Does_Not_Throw()
        {
            var one = new ExchangeRateAt(DateTime.Today, EUR, USD, 1.25m);

            one.Equals(null).Should().BeFalse();
        }

        [TestMethod]
        public void Equals_Structurally()
        {
            var one = new ExchangeRateAt(DateTime.Today, EUR, USD, 1.25m);
            var other = new ExchangeRateAt(DateTime.Today, EUR, USD, 1.25m);

            one.Equals(other).Should().BeTrue();
        }
    }
}
