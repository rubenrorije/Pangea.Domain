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
    public class CountryCodesTests
    {
        [TestMethod]
        public void When_Not_Registered_Return_Null()
        {
            CountryCodes.Instance.Should().BeNull();
        }

        [TestMethod]
        public void Provider_Can_Not_Be_Null()
        {
            Action action = () => CountryCodes.SetProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Registering_An_Instance_Will_Return_That_Instance()
        {
            var sut = new DefaultCountryProvider();

            CountryCodes.SetProvider(() => sut);

            CountryCodes.Instance.Should().Be(sut);

            CountryCodes.ClearProvider();
        }

        [TestMethod]
        public void ClearProvider_When_No_Provider_Is_Registered_Is_Not_An_Issue()
        {
            CountryCodes.ClearProvider();
            CountryCodes.ClearProvider();
        }
    }
}
