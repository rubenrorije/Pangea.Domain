using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pangea.Domain.DefaultCurrencies;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class CurrenciesTests
    {
        [TestMethod]
        public void Register_Known_Currencies()
        {
            Currencies.Initialize(EUR, USD);
        }

        [TestMethod]
        public void Use_Default_Provider_When_Set()
        {
            var sut = Currencies.SetRegionInfoProvider();
            sut.Count.Should().BeGreaterThan(10);
        }

        [TestMethod]
        public void Registering_Null_Throws_ArgumentNullException()
        {
            Action action = () => Currencies.SetProvider(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Use_The_Registered_Instance_For_Currency_Resolution_By_Code()
        {
            var sut = new CurrencyCollection
            {
                EUR
            };

            using (new RegisterCurrencies(sut))
            {
                Currencies.Find("EUR").Should().NotBeNull();
            }
        }

        [TestMethod]
        public void Initialize_Returns_An_Instance_With_The_Given_Currencies()
        {
            var sut = Currencies.Initialize(EUR, USD);
            sut.Count.Should().Be(2);
        }

        [TestMethod]
        public void Initialize_Creates_A_New_Instance_Every_Time()
        {
            var one = Currencies.Initialize(EUR, USD);
            var other = Currencies.Initialize(CAD);

            one.Count.Should().Be(2);
            other.Count.Should().Be(1);
        }

        [TestMethod]
        public void Initialize_Replaces_The_Previously_Registered_Currencies()
        {
            Currencies.Initialize(EUR, USD);
            Currencies.Initialize(CAD);

            Currencies.Find("CAD").Should().NotBeNull();
            Currencies.Find("EUR").Should().BeNull();
        }

        [TestMethod]
        public void When_Provider_Function_Is_Not_Called_The_Default_Is_Used()
        {
            Action action = () => { var c = Currencies.Find("EUR"); };

            action.Should().NotThrow<InvalidOperationException>();
        }



    }
}
