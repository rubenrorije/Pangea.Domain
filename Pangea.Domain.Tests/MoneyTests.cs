using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pangea.Domain.DefaultCurrencies;
using Pangea.Domain.Fluent;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class MoneyTests
    {
        [TestMethod]
        public void Currency_Must_Be_Specified_When_Creating_A_Money_Object()
        {
            Action action = () => new Money(null, 5);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Default_Is_Empty()
        {
            default(Money).ToString().Should().BeEmpty();
        }

        [TestMethod]
        public void Create_A_Money_Object_For_A_Certain_Currency()
        {
            var sut = new Money(EUR, 5);

            sut.Amount.Should().Be(5);
        }

        [TestMethod]
        public void ToString_Default_With_Two_Digits()
        {
            var sut = new Money(EUR, 5);
            sut.ToString(CultureInfo.InvariantCulture).Should().Be("€5.00");
        }

        [TestMethod]
        public void ToString_Default_Negative()
        {
            var sut = new Money(EUR, -5);
            sut.ToString(CultureInfo.InvariantCulture).Should().Be("(€5.00)");
        }

        [TestMethod]
        public void ToString_Default_Negative_In_Dutch_Culture()
        {
            var sut = new Money(EUR, -5);
            sut.ToString(CultureInfo.GetCultureInfo(1043)).Should().Be("€ -5,00");
        }

        [TestMethod]
        public void ToString_S_Without_Specified_Digits()
        {
            var sut = new Money(EUR, 5);
            sut.ToString("S", CultureInfo.InvariantCulture).Should().Be("€5.00");
        }

        [TestMethod]
        public void ToString_S_With_Specified_Digits()
        {
            var sut = new Money(EUR, 5);
            sut.ToString("S3", CultureInfo.InvariantCulture).Should().Be("€5.000");
        }

        [TestMethod]
        public void ToString_C_With_Specified_Digits()
        {
            var sut = new Money(EUR, 5);
            sut.ToString("C3", CultureInfo.InvariantCulture).Should().Be("EUR5.000");
        }

        [TestMethod]
        public void ToString_N_Without_Specified_Digits()
        {
            var sut = new Money(EUR, 5);
            sut.ToString("N", CultureInfo.InvariantCulture).Should().Be("€5.00");
        }

        [TestMethod]
        public void ToString_N_With_Specified_Digits()
        {
            var sut = new Money(EUR, 5);
            sut.ToString("N3", CultureInfo.InvariantCulture).Should().Be("€5.000");
        }

        [TestMethod]
        public void ToString_In_Dutch_Culture()
        {
            var sut = new Money(EUR, 5);
            sut.ToString(CultureInfo.GetCultureInfo(1043)).Should().Be("€ 5,00");
        }

        [TestMethod]
        public void GetHashCode_Of_Default_Money()
        {
            default(Money).GetHashCode().Should().Be(0);
        }

        [TestMethod]
        public void Same_Money_Must_Be_Equal()
        {
            new Money(EUR, 5).Should().Be(new Money(EUR, 5));
            (new Money(EUR, 5) == new Money(EUR, 5)).Should().BeTrue();
            (new Money(EUR, 5) != new Money(EUR, 5)).Should().BeFalse();
        }

        [TestMethod]
        public void Different_Currency_Money_Is_Not_Equal()
        {
            new Money(EUR, 5).Equals(new Money(USD, 5)).Should().BeFalse();
            (new Money(EUR, 5) == new Money(USD, 5)).Should().BeFalse();
            (new Money(EUR, 5) != new Money(USD, 5)).Should().BeTrue();
        }

        [TestMethod]
        public void Unary_Minus_Negates_The_Amount()
        {
            var sut = -new Money(EUR, 5);
            sut.Should().Be(new Money(EUR, -5));
        }

        [TestMethod]
        public void Unary_Minus_On_Empty_Money_Returns_The_Same()
        {
            var sut = -default(Money);
            sut.Should().Be(default(Money));
        }

        [TestMethod]
        public void Abs_Money_Of_Negative_Amount()
        {
            new Money(EUR, -5).Absolute().Should().Be(new Money(EUR, 5));
        }

        [TestMethod]
        public void Abs_Money_Of_Positive_Amount()
        {
            new Money(EUR, 5).Absolute().Should().Be(new Money(EUR, 5));
        }

        [TestMethod]
        public void Add_Default_Money_Returns_The_Same_When_Reversed()
        {
            var sut = new Money(EUR, 5);
            var result = new Money() + sut;
            result.Should().Be(sut);
        }

        [TestMethod]
        public void Add_Default_Money_Returns_The_Same()
        {
            var sut = new Money(EUR, 5);
            var result = sut + new Money();
            result.Should().Be(sut);
        }

        [TestMethod]
        public void Add_Money_With_Different_Currency_Throws_Exceptoin()
        {
            Action action = () => { var _ = new Money(EUR, 5) + new Money(USD, 5); };
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void CompareTo_When_No_Money_Throws_Exception()
        {
            Action action = () => new Money(EUR, 5).CompareTo(5);
            action.Should().Throw<Exception>();
        }
        [TestMethod]
        public void Compare_To_Default_Money_Throws_Exeption()
        {
            Action action = () => new Money(EUR, 5).CompareTo(default(Money));
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Compare_To_Of_Equal_Money_Returns_0()
        {
            new Money(EUR, 5).CompareTo(new Money(EUR, 5)).Should().Be(0);
        }

        [TestMethod]
        public void Compare_To_When_Comparing_The_Same_Currency_Works()
        {
            var six = new Money(EUR, 6);
            var ten = new Money(EUR, 10);

            six.CompareTo(ten).Should().Be(-1);
            ten.CompareTo(six).Should().Be(1);

            six.Should().BeLessThan(ten);
            ten.Should().BeGreaterThan(six);

            (six > ten).Should().BeFalse();
            (six < ten).Should().BeTrue();

            (ten > six).Should().BeTrue();
            (ten < six).Should().BeFalse();
        }

        [TestMethod]
        public void Divide_Money_Into_Parts()
        {
            var sut = new Money(EUR,6);
            (sut / 2).Should().Be(new Money(EUR, 3)); 
        }

        [TestMethod]
        public void Divide_Money_By_Zero()
        {
            var sut = new Money(EUR, 6);
            Action action = () => { var _ = (sut / 0); };
            action.Should().Throw<DivideByZeroException>();
        }

        [TestMethod]
        public void Multiply_Money_For_A_Number_Of_Times()
        {
            var sut = new Money(EUR, 6);
            (sut * 2).Should().Be(new Money(EUR, 12));
        }

        [TestMethod]
        public void Multiply_With_Percentage()
        {
            var sut = new Money(EUR, 6);
            (sut * 150.Percent()).Should().Be(new Money(EUR, 9));
        }

        [TestMethod]
        public void Add_Percentage_To_Default_Returns_Default()
        {
            var sut = default(Money);
            (sut + 10.Percent()).Should().Be(default(Money));
        }

        [TestMethod]
        public void Add_Percentage_To_A_Money_Object()
        {
            var sut = new Money(EUR, 6);
            (sut + 10.Percent()).Should().Be(new Money(EUR, 6.6m));
        }

        [TestMethod]
        public void Fluent_Creation_Of_Money_In_Decimals()
        {
            5m.AustralianDollars().Should().Be(new Money(AUD, 5));
            5m.CanadianDollars().Should().Be(new Money(CAD, 5));
            5m.SwissFrancs().Should().Be(new Money(CHF, 5));
            5m.ChineseYuan().Should().Be(new Money(CNY, 5));
            5m.Euros().Should().Be(new Money(EUR, 5));
            5m.BritishPounds().Should().Be(new Money(GBP, 5));
            5m.JapaneseYen().Should().Be(new Money(JPY, 5));
            5m.NewZealandDollars().Should().Be(new Money(NZD, 5));
            5m.SwedishKrona().Should().Be(new Money(SEK, 5));
            5m.Dollars().Should().Be(new Money(USD, 5));
        }

        [TestMethod]
        public void Fluent_Creation_Of_Money_In_Integers()
        {
            5.AustralianDollars().Should().Be(new Money(AUD, 5));
            5.CanadianDollars().Should().Be(new Money(CAD, 5));
            5.SwissFrancs().Should().Be(new Money(CHF, 5));
            5.ChineseYuan().Should().Be(new Money(CNY, 5));
            5.Euros().Should().Be(new Money(EUR, 5));
            5.BritishPounds().Should().Be(new Money(GBP, 5));
            5.JapaneseYen().Should().Be(new Money(JPY, 5));
            5.NewZealandDollars().Should().Be(new Money(NZD, 5));
            5.SwedishKrona().Should().Be(new Money(SEK, 5));
            5.Dollars().Should().Be(new Money(USD, 5));
        }

        [TestMethod]
        public void Linq_Methods_On_Money_Objects()
        {
            var sut = new[] { 2, 4, 3, 5, 1 }.Select(i => i.Euros()).ToList();

            sut.Max().Should().Be(5.Euros());
            sut.Min().Should().Be(1.Euros());
            sut.Sum().Should().Be(15.Euros());
            sut.Average().Should().Be(3.Euros());
        }

        [TestMethod]
        public void Multiply_By_Exchange_Rate_Forward()
        {
            var sut = 5.Euros();
            var rate = new ExchangeRate(EUR, USD, 2);

            (sut * rate).Should().Be(10.Dollars());
        }

        [TestMethod]
        public void Multiply_By_Exchange_Rate_Backward()
        {
            var sut = 6.Euros();
            var rate = new ExchangeRate(USD, EUR, 2);

            (sut * rate).Should().Be(3.Dollars());
        }

        [TestMethod]
        public void Multiply_By_Exchange_Rate_For_Default_Money_Returns_The_Default()
        {
            var sut = default(Money);
            var rate = new ExchangeRate(USD, EUR, 2);

            (sut * rate).Should().Be(default(Money));
        }

        [TestMethod]
        public void Multiply_By_Exchange_Rate_When_One_Of_The_Currencies_Does_Not_Match_Throw_Exception()
        {
            var sut = 6.Euros();
            var rate = new ExchangeRate(USD, CAD, 2);

            Action action = () => { var _ = sut * rate; };

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Multiply_By_Exchange_Rate_When_The_Rate_Is_Null_Throws_Exception()
        {
            var sut = 6.Euros();
            Action action = () => { var _ = sut * null; };

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
