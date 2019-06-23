using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pangea.Domain.Fluent;
namespace Pangea.Domain.Tests
{
    [TestClass]
    public class PercentageTests
    {
        [TestMethod]
        public void New_Percentage_Has_The_Given_Value()
        {
            new Percentage(20).Value.Should().Be(20);
        }

        [TestMethod]
        public void New_Percentage_Has_Zero_As_Default()
        {
            new Percentage().Value.Should().Be(0);
        }

        [TestMethod]
        public void ToString_By_Default_Returns_Percent()
        {
            new Percentage(5).ToString().Should().Be("5%");
        }

        [TestMethod]
        public void ToString_Formats_The_Number_Correctly()
        {
            new Percentage(5).ToString("N2", CultureInfo.InvariantCulture).Should().Be("5.00%");
        }

        [TestMethod]
        public void Create_A_Percentage_From_Different_Types_Is_The_Same()
        {
            var itg = new Percentage(5);
            var dbl = new Percentage(5.0);
            var dcm = new Percentage(5m);

            itg.Equals(dbl).Should().BeTrue();
            itg.Equals(dcm).Should().BeTrue();
            dbl.Equals(dcm).Should().BeTrue();
        }

        [TestMethod]
        public void Create_Percentage_From_Double_Fraction()
        {
            var percentage = Percentage.FromFraction(0.05);
            percentage.Value.Should().Be(5);
        }

        [TestMethod]
        public void Create_Percentage_From_Decimal_Fraction()
        {
            var percentage = Percentage.FromFraction(0.05m);
            percentage.Value.Should().Be(5);
        }

        [TestMethod]
        public void Equals_Works_As_Expected()
        {
            var one = new Percentage(5.0);
            var other = Percentage.FromFraction(0.05);

            one.Equals(other).Should().BeTrue();
            (one == other).Should().BeTrue();
        }

        [TestMethod]
        public void Equals_Works_With_Conversion()
        {
            var sut = new Percentage(5);

            (sut == 5).Should().BeTrue();
            (sut == 5m).Should().BeTrue();
            (sut == 5.0).Should().BeTrue();
            (5 == sut).Should().BeTrue();
            (5m == sut).Should().BeTrue();
            (5.0 == sut).Should().BeTrue();

        }
        [TestMethod]
        public void NotEquals_Works_With_Conversion()
        {
            var sut = new Percentage(5);

            (sut != 6).Should().BeTrue();
            (sut != 6m).Should().BeTrue();
            (sut != 6.0).Should().BeTrue();
            (6 != sut).Should().BeTrue();
            (6m != sut).Should().BeTrue();
            (6.0 != sut).Should().BeTrue();
        }

        [TestMethod]
        public void Operator_Plus_Adds_Percentage_To_Simple_Integer_And_Do_Decimal_Conversion()
        {
            (5 + new Percentage(10)).Should().Be(5.5m);
        }

        [TestMethod]
        public void Operator_Plus_Adds_Percentage_To_Simple_Decimal()
        {
            (5m + new Percentage(10)).Should().Be(5.5m);
        }

        [TestMethod]
        public void Operator_Plus_Adds_Percentage_To_Simple_Double()
        {
            (5.0 + new Percentage(10)).Should().Be(5.5);
        }

        [TestMethod]
        public void Operator_Minus_Subtracts_ThePercentage_From_A_Simple_Integer_And_Does_Decimal_Conversion()
        {
            (5 - new Percentage(10)).Should().Be(4.5m);
        }

        [TestMethod]
        public void Operator_Minus_Subtracts_ThePercentage_From_A_Simple_Decimal()
        {
            (5m - new Percentage(10)).Should().Be(4.5m);
        }

        [TestMethod]
        public void Operator_Minus_Subtracts_ThePercentage_From_A_Simple_Double()
        {
            (5.0 - new Percentage(10)).Should().Be(4.5);
        }

        [TestMethod]
        public void Operator_Multiply_Calculates_The_Percentage_And_Does_Conversion_To_Decimal()
        {
            (10 * new Percentage(10)).Should().Be(1);
        }

        [TestMethod]
        public void Operator_Multiply_Calculates_The_Percentage_Of_A_Double()
        {
            (10.0 * new Percentage(10)).Should().Be(1.0);
        }

        [TestMethod]
        public void Operator_Multiply_Calculates_The_Percentage_Of_A_Decimal()
        {
            (10m * new Percentage(10)).Should().Be(1m);
        }

        [TestMethod]
        public void Operator_Greather_Than()
        {
            (new Percentage(10) > new Percentage(5)).Should().BeTrue();
            (new Percentage(5) > new Percentage(5)).Should().BeFalse();

            (10 > new Percentage(5)).Should().BeTrue();
            (5 > new Percentage(5)).Should().BeFalse();

            (10.0 > new Percentage(5)).Should().BeTrue();
            (5.0 > new Percentage(5)).Should().BeFalse();

            (10m > new Percentage(5)).Should().BeTrue();
            (5m > new Percentage(5)).Should().BeFalse();
        }

        [TestMethod]
        public void Operator_Less_Than()
        {
            (new Percentage(10) < new Percentage(5)).Should().BeFalse();
            (new Percentage(5) < new Percentage(5)).Should().BeFalse();

            (10 < new Percentage(5)).Should().BeFalse();
            (5 < new Percentage(5)).Should().BeFalse();
            (new Percentage(10) < 5).Should().BeFalse();
            (new Percentage(5) < 5).Should().BeFalse();

            (10.0 < new Percentage(5)).Should().BeFalse();
            (5.0 < new Percentage(5)).Should().BeFalse();
            (new Percentage(10) < 5.0).Should().BeFalse();
            (new Percentage(5) < 5.0).Should().BeFalse();

            (10m < new Percentage(5)).Should().BeFalse();
            (5m < new Percentage(5)).Should().BeFalse();
            (new Percentage(10) < 5m).Should().BeFalse();
            (new Percentage(5) < 5m).Should().BeFalse();
        }

        [TestMethod]
        public void Operator_Greather_Than_Or_Equal()
        {
            (new Percentage(10) >= new Percentage(5)).Should().BeTrue();
            (new Percentage(5) >= new Percentage(5)).Should().BeTrue();

            (10 >= new Percentage(5)).Should().BeTrue();
            (5 >= new Percentage(5)).Should().BeTrue();

            (10.0 >= new Percentage(5)).Should().BeTrue();
            (5.0 >= new Percentage(5)).Should().BeTrue();

            (10m >= new Percentage(5)).Should().BeTrue();
            (5m >= new Percentage(5)).Should().BeTrue();
        }

        [TestMethod]
        public void Operator_Less_Than_Or_Equal()
        {
            (new Percentage(10) <= new Percentage(5)).Should().BeFalse();
            (new Percentage(5) <= new Percentage(5)).Should().BeTrue();

            (10 <= new Percentage(5)).Should().BeFalse();
            (5 <= new Percentage(5)).Should().BeTrue();
            (new Percentage(10) <= 5).Should().BeFalse();
            (new Percentage(5) <= 5).Should().BeTrue();

            (10.0 <= new Percentage(5)).Should().BeFalse();
            (5.0 <= new Percentage(5)).Should().BeTrue();
            (new Percentage(10) <= 5.0).Should().BeFalse();
            (new Percentage(5) <= 5.0).Should().BeTrue();

            (10m <= new Percentage(5)).Should().BeFalse();
            (5m <= new Percentage(5)).Should().BeTrue();
            (new Percentage(10) <= 5m).Should().BeFalse();
            (new Percentage(5) <= 5m).Should().BeTrue();
        }

        [TestMethod]
        public void Percentage_Does_Not_Restrict_To_Hundred()
        {
            new Percentage(250).Value.Should().Be(250);
        }

        [TestMethod]
        public void Create_A_Percentage_From_A_Given_Nominator_And_Denominator()
        {
            var sut = Percentage.From(1, 10);
            sut.Value.Should().Be(10);
        }

        [TestMethod]
        public void When_The_Denominator_Is_Zero_Throw_ArgumentOutOfRangeException()
        {
            Action action = () => Percentage.From(1, 0);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void When_The_Denominator_Is_Less_Than_Zero_Throw_ArgumentOutOfRangeException()
        {
            Action action = () => Percentage.From(1, -1);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void When_The_Nominator_And_Denominator_Are_Less_Than_Zero_Throw_ArgumentOutOfRangeException()
        {
            Action action = () => Percentage.From(-1, -1);
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Operator_Divide()
        {
            (10 / new Percentage(125)).Should().Be(8m);
            (10m / new Percentage(125)).Should().Be(8m);
            (10.0 / new Percentage(125)).Should().Be(8.0);
        }

        [TestMethod]
        public void Serialization()
        {
            var sut = new Percentage(20);

            sut.Should().BeXmlSerializable();
            sut.Should().BeBinarySerializable();
            sut.Should().BeDataContractSerializable();
        }

        [TestMethod]
        public void DataContract_Serializable()
        {
            var sut = new Percentage(5);
            sut.Should().BeDataContractSerializable();
        }

        [TestMethod]
        public void Xml_Serializable()
        {
            var sut = new Percentage(5);
            sut.Should().BeXmlSerializable();
        }

        [TestMethod]
        public void Binary_Serializable()
        {
            var sut = new Percentage(5);
            sut.Should().BeBinarySerializable();
        }

        [TestMethod]
        public void Fluent_Interface_Is_Equal_To_Constructor()
        {
            20.Percent().Should().Be(new Percentage(20));
        }

        [TestMethod]
        public void Compounded_Percentages_Should_Have_An_Array_Of_Percentages()
        {
            Action action = () => Percentage.Compounded(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Compounded_Percentages_Should_Have_An_Array_Of_Percentages_Of_At_Least_One()
        {
            Action action = () => Percentage.Compounded(new Percentage[] { });
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Compounded_Percentages_When_Only_One_Percentage_Returns_The_Parameter()
        {
            var sut = Percentage.Compounded(20.Percent());
            sut.Should().Be(20.Percent());
        }

        [TestMethod]
        public void Compounded_Percentages_Is_Not_The_Same_As_Adding_The_Percentages()
        {
            var sut = Percentage.Compounded(20.Percent(), 5.Percent());
            sut.Should().NotBe(25.Percent());
        }

        [TestMethod]
        public void Compound_Percentages_Will_Return_The_Compounded_Result()
        {
            var sut = Percentage.Compounded(20.Percent(), 5.Percent());
            sut.Should().Be(26.Percent());
        }

        [TestMethod]
        public void Compound_Percentages_Will_Return_The_Compounded_Result_For_Multiple_Percentages()
        {
            var sut = Percentage.Compounded(10.Percent(), 10.Percent(), 10.Percent());
            sut.Should().Be(33.1.Percent());
        }

        [TestMethod]
        public void Compound_Percentages_Can_Be_Negative()
        {
            var sut = Percentage.Compounded(10.Percent(), (-10).Percent());
            sut.Should().Be((-1).Percent());
        }

        [TestMethod]
        public void Default_ToString_No_Trailing_Zeros()
        {
            var one = new Percentage(33.10000m);
            var other = new Percentage(33.1m);
            one.ToString().Should().Be(other.ToString());
        }

        [TestMethod]
        public void Create_Negative_Percentage()
        {
            var sut = new Percentage(-5);
            sut.ToString().Should().Be("-5%");
        }

        [TestMethod]
        public void Negate_Percentage()
        {
            var sut = -new Percentage(10);
            sut.Should().Be(new Percentage(-10));
        }

        [TestMethod]
        public void Absolute_Percentage_Returns_The_Original_When_Positive()
        {
            var sut = Percentage.Abs(new Percentage(10));
            sut.Value.Should().Be(10);
        }

        [TestMethod]
        public void Absolute_Percentage_Returns_The_Negated_When_Negative()
        {
            var sut = Percentage.Abs(new Percentage(-10));
            sut.Value.Should().Be(10);
        }

        [TestMethod]
        public void Maximum_Percentage_Returns_Correct_Result()
        {
            new Percentage[] { new Percentage(5), new Percentage(10) }.Max().Should().Be(new Percentage(10));
            new Percentage[] { new Percentage(10), new Percentage(5) }.Max().Should().Be(new Percentage(10));
        }

        [TestMethod]
        public void Minimum_Percentage_Returns_Correct_Result()
        {
            new[] { new Percentage(5), new Percentage(10) }.Min().Should().Be(new Percentage(5));
            new[] { new Percentage(10), new Percentage(5) }.Min().Should().Be(new Percentage(5));
        }

        [TestMethod]
        public void Round_The_Percentage()
        {
            var sut = new Percentage(4.6);
            sut.Round().Should().Be(new Percentage(5));
            sut.Round(1).Should().Be(new Percentage(4.6));
        }

        [TestMethod]
        public void Floor_The_Percentage()
        {
            var sut = new Percentage(4.9);
            sut.Floor().Should().Be(new Percentage(4));
        }

        [TestMethod]
        public void Ceiling_The_Percentage()
        {
            var sut = new Percentage(4.01);
            sut.Ceiling().Should().Be(new Percentage(5));
        }


    }
}
