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
    public class CurrencyCollectionTests
    {
        [TestMethod]
        public void Create_Empty_Currencies_Does_Not_Have_Any_Currencies_Registered()
        {
            var sut = new CurrencyCollection();
            sut.Count.Should().Be(0);
        }

        [TestMethod]
        public void Add_A_Currency_Must_Increment_Count()
        {
            var sut = new CurrencyCollection
            {
                EUR
            };

            sut.Count.Should().Be(1);
        }

        [TestMethod]
        public void Add_A_Currency_Multiple_Times_Will_Register_It_Only_Once()
        {
            var sut = new CurrencyCollection
            {
                EUR,
                EUR,
                EUR
            };

            sut.Count.Should().Be(1);
        }

        [TestMethod]
        public void Remove_Currency_From_List()
        {
            var sut = new CurrencyCollection
            {
                EUR
            };

            sut.Count.Should().Be(1);

            sut.Remove(EUR);

            sut.Count.Should().Be(0);
        }

        [TestMethod]
        public void Adding_Null_Throws_ArgumentNullException()
        {
            Action action = () => new CurrencyCollection
            {
                null
            };
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Removing_Null_Throws_ArgumentNullException()
        {
            var sut = new CurrencyCollection
            {
                EUR
            };
            Action action = () => sut.Remove(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Removing_A_Currency_That_Was_Not_Registered_Does_Not_Do_Anything()
        {
            var sut = new CurrencyCollection
            {
                EUR
            };

            sut.Remove(new Currency("AED"));

            sut.Count.Should().Be(1);
        }

        [TestMethod]
        public void Removing_A_Currency_Does_Not_Depend_On_Pointer_Reference()
        {
            var sut = new CurrencyCollection
            {
                new Currency("AED")
            };
            sut.Remove(new Currency("AED"));

            sut.Count.Should().Be(0);
        }

    }
}
