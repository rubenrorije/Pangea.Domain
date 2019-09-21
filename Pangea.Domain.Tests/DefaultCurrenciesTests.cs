using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class DefaultCurrenciesTests
    {
        [TestMethod]
        public void Register_All_Top_10_Currencies_Directly()
        {
            var sut = DefaultCurrencies.Register();

            sut.Count.Should().Be(10);
        }
    }
}
