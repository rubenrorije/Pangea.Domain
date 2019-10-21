using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pangea.Domain.DefaultCurrencies;
using Pangea.Domain.Fluent;
namespace Pangea.Domain.Tests
{
    [TestClass]
    public class ExchangeRateCollectionTests
    {
        [TestMethod]
        public void Convenient_Construction_With_Initializer()
        {
            var sut = new ExchangeRateCollection
            {
                new ExchangeRate(EUR, USD, 5m),
                new ExchangeRate(EUR, AUD, 10m)
            };

            sut.Count.Should().Be(2);
        }
        [TestMethod]
        public void Cannot_Add_The_Same_Rate_Twice()
        {
            Action action = () => new ExchangeRateCollection
            {
                new ExchangeRate(EUR, USD, 5m),
                new ExchangeRate(EUR, USD, 5m)
            };
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Contains_From_Currency_Not_Null()
        {
            Action action = () => new ExchangeRateCollection().Contains(null, EUR);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Contains_To_Currency_Not_Null()
        {
            Action action = () => new ExchangeRateCollection().Contains(EUR, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Indexed_Returns_Rate()
        {
            var sut = new ExchangeRateCollection()
            {
                new ExchangeRate(EUR, USD, 5m),
            };

            sut[EUR, USD].Should().NotBeNull();
        }

        [TestMethod]
        public void Indexed_Returns_No_Rate_When_Reversed_And_Different_Rates()
        {
            var sut = new ExchangeRateCollection()
            {
                new ExchangeRate(EUR, USD, 5m),
            };
            Action action = () => { var result = sut[USD, EUR]; };
            action.Should().Throw<KeyNotFoundException>();
        }

        [TestMethod]
        public void Indexed_Throws_Exception_When_No_Rate_Found()
        {
            var sut = new ExchangeRateCollection()
            {
                new ExchangeRate(EUR, USD, 5m),
            };

            Action action = () => { var _ = sut[USD, CAD]; };
            action.Should().Throw<KeyNotFoundException>();
        }

        [TestMethod]
        public void Contains_For_Currencies_Honors_The_Direction_Of_Exchange_Rates()
        {
            var sut = new ExchangeRateCollection()
            {
                new ExchangeRate(EUR, USD, 5m),
            };
            sut.Contains(EUR, USD).Should().BeTrue();
            sut.Contains(EUR, AUD).Should().BeFalse();
            sut.Contains(USD, EUR).Should().BeFalse();
        }
        [TestMethod]
        public void Contains_For_Currencies_When_Using_Different_Rates_Both_Ways()
        {
            var sut = new ExchangeRateCollection()
            {
                new ExchangeRate(EUR, USD, 5m),
            };
            sut.Contains(EUR, USD).Should().BeTrue();
            sut.Contains(USD, EUR).Should().BeFalse();
        }


        [TestMethod]
        public void Convert_Money_Into_Different_Currency_When_Rate_Exists()
        {
            var euros = new Money(EUR, 6);
            var sut = new ExchangeRateCollection()
            {
                new ExchangeRate(EUR, USD, 5m),
            };

            var result = sut.Convert(euros, USD);

            result.Amount.Should().Be(30);
            result.Currency.Should().Be(USD);
        }

        [TestMethod]
        public void Convert_Money_Into_Different_Currency_When_Rate_Exists_Backward_Does_Not_Work()
        {
            var euros = new Money(EUR, 6);
            var sut = new ExchangeRateCollection()
            {
                new ExchangeRate(USD, EUR, 5m),
            };

            var result = sut.TryConvert(euros, USD);
            result.Should().BeNull();
        }


    }
}
