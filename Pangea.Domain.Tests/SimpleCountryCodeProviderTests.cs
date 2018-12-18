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
    public class SimpleCountryCodeProviderTests
    {
        [TestMethod]
        public void Null_Dictionary_Throws_Exception()
        {
            Action action = () => new SimpleCountryCodeProvider(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Resolve_Netherlands()
        {
            var sut = new SimpleCountryCodeProvider
            {
                { "NL", 31 }
            };

            sut.GetCountryCallingCodeFrom("31").Should().Be(31);
        }

        [TestMethod]
        public void Resolve_The_Country_Calling_Code_For_The_Country_Alpha_2_Name()
        {
            var sut = new SimpleCountryCodeProvider
            {
                { "NL", 31 }
            };
            sut.GetCountryCallingCodeFor("NL").Should().Be(31);
        }

        [TestMethod]
        public void Resolving_Null_Should_Return_Null()
        {
            var sut = new SimpleCountryCodeProvider
            {
            };

            sut.GetCountryCallingCodeFrom(null).Should().Be(null);
        }

        [TestMethod]
        public void Resolve_With_Multiple_Different_Size_CountryCodes()
        {
            var sut = new SimpleCountryCodeProvider
            {
                { "XX", 123 },
                { "NL", 31 }
            };
            sut.GetCountryCallingCodeFrom("31").Should().Be(31);
        }

        [TestMethod]
        public void Add_A_Country()
        {
            var sut = new SimpleCountryCodeProvider
            {
                { "NL", 31 }
            };
        }

        [TestMethod]
        public void Add_A_Country_Twice_Is_Not_Allowed()
        {
            var sut = new SimpleCountryCodeProvider
            {
                { "NL", 31 }
            };
            Action action = () => sut.Add("NL", 31);
            action.Should().Throw<Exception>();
        }
    }
}
