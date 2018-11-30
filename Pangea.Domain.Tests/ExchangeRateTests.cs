using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pangea.Domain.DefaultCurrencies;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class ExchangeRateTests
    {
        [TestMethod]
        public void From_Null_Currency_Will_Throw_Exception()
        {
            Action action = () => new ExchangeRate(EUR, null, 1);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void To_Null_Currency_Will_Throw_Exception()
        {
            Action action = () => new ExchangeRate(null, EUR, 1);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void It_Is_Not_Possible_To_Create_An_Exchange_Rate_When_The_To_And_From_Currency_Are_The_Same()
        {
            Action action = () => new ExchangeRate(EUR, EUR, 1);

            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Create_An_Exchange_Rate()
        {
            var sut = new ExchangeRate(EUR, USD, 1.1m);
            sut.From.Should().Be(EUR);
            sut.To.Should().Be(USD);
            sut.Rate.Should().Be(1.1m);
        }

        [TestMethod]
        public void The_Rate_Cannot_Be_Zero()
        {
            Action action = () => new ExchangeRate(EUR, USD, 0);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void The_Rate_Cannot_Be_Below_Zero()
        {
            Action action = () => new ExchangeRate(EUR, USD, -1);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ToString_Must_Include_Currencies()
        {
            var sut = new ExchangeRate(EUR, USD, 1.141233m);
            sut.ToString().Should().Contain("EUR/USD");
        }

        [TestMethod]
        public void ToString_Prints_Currencies()
        {
            var sut = new ExchangeRate(EUR, USD, 1.141233m);

            sut.ToString(null, CultureInfo.InvariantCulture).Should().Be("EUR/USD 1.141233");
        }


        [TestMethod]
        public void ToString_Prints_Currencies_Using_The_Format()
        {
            var sut = new ExchangeRate(EUR, USD, 1.141233m);

            sut.ToString("F2", CultureInfo.InvariantCulture).Should().Be("EUR/USD 1.14");
        }

        [TestMethod]
        public void Invert_Switches_The_Currencies()
        {
            var original = new ExchangeRate(EUR, USD, 1.141233m);
            var sut = original.Invert();

            sut.From.Should().Be(USD);
            sut.To.Should().Be(EUR);
            sut.Rate.Should().BeApproximately(0.8762452540366428m, 0.00000001m);
        }

        [TestMethod]
        public void Operator_Multiply_Uses_Exchange_Rate_To_Calculate_The_Correct_Amount_In_The_To_Currency()
        {
            var rate = new ExchangeRate(EUR, USD, 1.141233m);

            var eurAmount = 100;
            var usdAmount = eurAmount * rate;

            usdAmount.Should().BeApproximately(114.12m, 0.01m);
        }

        [TestMethod]
        public void Operator_Divide_Uses_Exchange_Rate_To_Calculate_The_Correct_Amount_In_The_To_Currency()
        {
            var rate = new ExchangeRate(EUR, USD, 1.141233m);

            var usdAmount = 100;
            var eurAmount = usdAmount / rate;

            eurAmount.Should().BeApproximately(87.62m, 0.01m);
        }

        [TestMethod]
        public void Convert_Will_Throw_Exception_When_The_Currency_Is_Null()
        {
            var rate = new ExchangeRate(EUR, USD, 1.141233m);

            Action action = () => rate.Convert(100, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Convert_Will_Throw_Exception_When_A_Wrong_Currency_Is_Used()
        {
            var rate = new ExchangeRate(EUR, USD, 1.141233m);

            Action action = () => rate.Convert(100, AUD);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }


        [TestMethod]
        public void Convert_Allows_The_Rate_To_Be_Specified_The_Other_Way_Around()
        {
            var rate = new ExchangeRate(EUR, USD, 1.141233m);

            rate.Convert(100m, EUR).Should().BeApproximately(114.12m, 0.01m);
            rate.Convert(100m, USD).Should().BeApproximately(87.62m, 0.01m);
        }
    }
}
