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
    public class ExchangeRateCollectionTests
    {
        [TestMethod]
        public void No_Conversion_Specified_Assumes_Same_Rate()
        {
            var sut = new ExchangeRateCollection();
            sut.ConversionType.Should().Be(ExchangeRateConversionType.SameRateBothWays);
        }

        [TestMethod]
        public void The_ConversionType_Specified_Must_Be_Stored()
        {
            var sut = new ExchangeRateCollection(ExchangeRateConversionType.InverseRateIsDifferent);
            sut.ConversionType.Should().Be(ExchangeRateConversionType.InverseRateIsDifferent);
        }

        [TestMethod]
        public void Convenient_Construction_With_Initializer()
        {
            var sut = new ExchangeRateCollection
            {
                new ExchangeRate(EUR, USD, 5m),
                new ExchangeRate(EUR,AUD,10m)
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
        public void Contains_For_Currencies_When_Using_Same_Rates_Both_Ways()
        {
            var sut = new ExchangeRateCollection(ExchangeRateConversionType.SameRateBothWays)
            {
                new ExchangeRate(EUR, USD, 5m),
            };
            sut.Contains(EUR, USD).Should().BeTrue();
            sut.Contains(EUR, AUD).Should().BeFalse();
            sut.Contains(USD, EUR).Should().BeTrue();
        }
        [TestMethod]
        public void Contains_For_Currencies_When_Using_Different_Rates_Both_Ways()
        {
            var sut = new ExchangeRateCollection(ExchangeRateConversionType.InverseRateIsDifferent)
            {
                new ExchangeRate(EUR, USD, 5m),
            };
            sut.Contains(EUR, USD).Should().BeTrue();
            sut.Contains(USD, EUR).Should().BeFalse();
        }

        [TestMethod]
        public void Cannot_Add_The_Inverted_Rate_When_Using_The_Same_Rate_Both_Ways()
        {
            Action action = () => new ExchangeRateCollection(ExchangeRateConversionType.SameRateBothWays)
            {
                new ExchangeRate(EUR, USD, 5m),
                new ExchangeRate(USD, EUR, 5m)
            };
            action.Should().Throw<ArgumentException>();
        }
    }
}
