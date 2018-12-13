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
    public class CurrenciesTests
    {
        [TestMethod]
        public void Create_Empty_Currencies_Does_Not_Have_Any_Currencies_Registered()
        {
            var sut = new CurrencyCollection();
            sut.Count.Should().Be(0);
        }

        [TestMethod]
        public void Provider_Is_Registered_Must_Be_False_Initially()
        {
            CurrencyCollection.ProviderIsRegistered.Should().BeFalse();
        }
        
        [TestMethod]
        public void Add_A_Currency_Must_Increment_Count()
        {
            var sut = new CurrencyCollection
            {
                DefaultCurrencies.EUR
            };

            sut.Count.Should().Be(1);
        }

        [TestMethod]
        public void Add_A_Currency_Multiple_Times_Will_Register_It_Only_Once()
        {
            var sut = new CurrencyCollection
            {
                DefaultCurrencies.EUR,
                DefaultCurrencies.EUR,
                DefaultCurrencies.EUR
            };

            sut.Count.Should().Be(1);
        }

        [TestMethod]
        public void Create_Currencies_Using_Initialization_Accolades()
        {
            var sut = new CurrencyCollection
            {
                DefaultCurrencies.EUR
            };

            sut.Count.Should().Be(1);
        }

        [TestMethod]
        public void Remove_Currency_From_List()
        {
            var sut = new CurrencyCollection
            {
                DefaultCurrencies.EUR
            };

            sut.Count.Should().Be(1);

            sut.Remove(DefaultCurrencies.EUR);

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
                DefaultCurrencies.EUR
            };
            Action action = () => sut.Remove(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Removing_A_Currency_That_Was_Not_Registered_Does_Not_Do_Anything()
        {
            var sut = new CurrencyCollection
            {
                DefaultCurrencies.EUR
            };

            sut.Remove(new Currency("AED", 123));

            sut.Count.Should().Be(1);
        }

        [TestMethod]
        public void Removing_A_Currency_Does_Not_Depend_On_Pointer_Reference()
        {
            var sut = new CurrencyCollection
            {
                new Currency("AED", 123)
            };
            sut.Remove(new Currency("AED", 456));

            sut.Count.Should().Be(0);
        }

        [TestMethod]
        public void When_Provider_Function_Is_Not_Called_Yet_Throw_An_Explicit_Exception_That_Shows_What_To_Do()
        {
            Action action = () => { var c = CurrencyCollection.Instance; };

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Use_The_Registered_Instance_For_Currency_Resolution_By_Code()
        {
            var sut = new CurrencyCollection
            {
                DefaultCurrencies.EUR
            };

            using (new RegisterCurrencies(sut))
            {
                CurrencyCollection.Find("EUR").Should().NotBeNull();
            }
        }

        [TestMethod]
        public void Use_The_Registered_Instance_For_Currency_Resolution_By_Numeric_Code()
        {
            var sut = new CurrencyCollection
            {
                DefaultCurrencies.EUR
            };

            using (new RegisterCurrencies(sut))
            {
                CurrencyCollection.Find(978).Should().NotBeNull();
            }
        }

    }
}
