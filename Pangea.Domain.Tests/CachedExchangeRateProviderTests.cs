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
    public class CachedExchangeRateProviderTests
    {
        [TestMethod]
        public void Inner_Provider_Cannot_Be_Null()
        {
            Action action = () => new CachedExchangeRateProvider(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Getting_ExchangeRate_Twice_Only_Queries_The_Internal_Provider_Once()
        {
            var inner = new DummyProvider();
            var cached = new CachedExchangeRateProvider(inner);

            inner.Counter.Should().Be(0);

            cached.TryGet(EUR, USD);
            inner.Counter.Should().Be(1);
            cached.TryGet(EUR, USD);
            inner.Counter.Should().Be(1);
            cached.TryGet(EUR, CAD);
            inner.Counter.Should().Be(2);
        }


        private class DummyProvider : IExchangeRateProvider
        {
            public int Counter { get; private set; }

            public ExchangeRate TryGet(Currency source, Currency target)
            {
                Counter++;
                return null;
            }
        }
    }


}
