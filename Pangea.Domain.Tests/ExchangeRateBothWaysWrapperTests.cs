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
    public class ExchangeRateBothWaysWrapperTests
    {
        [TestMethod]
        public void Inner_Cannot_Be_Null()
        {
            Action action = () => new ExchangeRateBothWaysWrapper(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Use_The_Rate_Forward()
        {
            var wrapper = new ExchangeRateBothWaysWrapper(new ExchangeRates(new ExchangeRate(EUR, USD, 2)));

            var result = wrapper.TryConvert(new Money(EUR, 5), USD);
            result.Should().NotBeNull();
            result.Value.Should().Be(new Money(USD, 10));
        }

        [TestMethod]
        public void Use_The_Rate_Backward()
        {
            var wrapper = new ExchangeRateBothWaysWrapper(new ExchangeRates(new ExchangeRate(EUR, USD, 2)));

            var result = wrapper.TryConvert(new Money(USD, 5), EUR);

            result.Should().NotBeNull();
            result.Value.Should().Be(new Money(EUR, 2.5m));
        }

        private class ExchangeRates : IExchangeRateProvider
        {
            private readonly ExchangeRate _rate;
            public ExchangeRates(ExchangeRate rate)
            {
                _rate = rate;
            }

            public Money? TryConvert(Money amount, Currency targetCurrency)
            {
                if (amount.Currency == _rate.From && targetCurrency == _rate.To)
                {
                    return amount * _rate;
                }
                else
                {
                    return null;
                }
            }

            public ExchangeRate TryGet(Currency sourceCurrency, Currency targetCurrency)
            {
                if (sourceCurrency == _rate.From && targetCurrency == _rate.To)
                    return _rate;
                return null;
            }
        }
    }
}
