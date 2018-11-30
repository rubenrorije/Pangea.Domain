using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class DateRangeTests
    {
        [TestMethod]
        public void Empty_DateRange_Should_Not_Include_MinValue()
        {
            var sut = new DateRange();
            sut.IsInRange(DateTime.MinValue);
        }

        [TestMethod]
        public void Start_And_End_Are_Set_By_Constructor()
        {
            var sut = new DateRange(new DateTime(2018, 1, 1), new DateTime(2018, 12, 31));
            sut.Start.Should().Be(new DateTime(2018, 1, 1));
            sut.End.Should().Be(new DateTime(2018, 12, 31));
        }

        [TestMethod]
        public void Times_Are_Ignored_When_Creating_DateRange()
        {
            var sut = new DateRange(new DateTime(2018, 1, 1, 13, 0, 0), new DateTime(2018, 1, 1, 14, 0, 0));
            sut.Start.Should().Be(new DateTime(2018, 1, 1));
            sut.End.Should().Be(new DateTime(2018, 1, 1));
        }

        [TestMethod]
        public void Times_Are_Ignored_When_Creating_DateRange_With_Null_Date()
        {
            var sut = new DateRange(null, new DateTime(2018, 1, 1, 14, 0, 0));
            sut.Start.Should().Be(null);
            sut.End.Should().Be(new DateTime(2018, 1, 1));
        }

        [TestMethod]
        public void Range_With_Smaller_End_Date_Than_Start_Date_Is_Not_Allowed()
        {
            Action action = () => new DateRange(new DateTime(2018, 1, 1), new DateTime(2017, 12, 31));

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
        [TestMethod]
        public void IsInRange_Will_Return_True_When_In_Range()
        {
            var sut = new DateRange(new DateTime(2018, 1, 1), new DateTime(2018, 12, 31));

            sut.IsInRange(new DateTime(2018, 1, 1)).Should().BeTrue();
            sut.IsInRange(new DateTime(2018, 12, 31)).Should().BeTrue();
            sut.IsInRange(new DateTime(2017, 12, 31)).Should().BeFalse();
            sut.IsInRange(new DateTime(2019, 1, 1)).Should().BeFalse();
        }

        [TestMethod]
        public void Empty_Range_Includes_No_Dates()
        {
            var sut = new DateRange();
            sut.IsInRange(DateTime.Today).Should().BeFalse();
        }

        [TestMethod]
        public void Indefinite_Range_Includes_All_Dates()
        {
            var sut = new DateRange(null, null);

            sut.IsInRange(new DateTime(2018, 1, 1)).Should().BeTrue();
        }

        [TestMethod]
        public void Create_Year_Range_From_Year()
        {
            var sut = DateRange.Year(2018);
            sut.Start.Should().Be(new DateTime(2018, 1, 1));
            sut.End.Should().Be(new DateTime(2018, 12, 31));
        }

        [TestMethod]
        public void Create_Years_Range_From_Year_Cannot_Be_Less_Than_1()
        {
            Action action = ()=> DateRange.Years(2018, 0);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Never_Is_Equal_To_Empty_Range()
        {
            DateRange.Never.Should().Be(new DateRange());
        }

        [TestMethod]
        public void Always_Is_Equal_To_An_Unbounded_Range()
        {
            DateRange.Always.Should().Be(new DateRange(null,null));
        }


        [TestMethod]
        public void Create_Years_Range_From_Year()
        {
            var sut = DateRange.Years(2018, 2);
            sut.Start.Should().Be(new DateTime(2018, 1, 1));
            sut.End.Should().Be(new DateTime(2019, 12, 31));
        }

        [TestMethod]
        public void Create_Year_Range_From_Date()
        {
            var sut = DateRange.Year(new DateTime(2018, 4, 4));
            sut.Start.Should().Be(new DateTime(2018, 1, 1));
            sut.End.Should().Be(new DateTime(2018, 12, 31));
        }

        [TestMethod]
        public void Operator_Plus_Adds_Two_Ranges()
        {
            var one = DateRange.Year(2018);
            var other = DateRange.Year(2019);

            var sut = one + other;

            sut.Start.Should().Be(new DateTime(2018, 1, 1));
            sut.End.Should().Be(new DateTime(2019, 12, 31));
        }

        [TestMethod]
        public void Today_Is_A_Range_That_Starts_And_Ends_With_Today()
        {
            var sut = DateRange.Today();
            sut.Start.Should().Be(DateTime.Today);
            sut.End.Should().Be(DateTime.Today);
        }

        [TestMethod]
        public void Day_Creates_A_Range_That_Spans_One_Day()
        {
            var sut = DateRange.Day(DateTime.Today);
            sut.Should().Be(DateRange.Today());
        }

        [TestMethod]
        public void IsInRange_Returns_True_When_The_Date_Has_A_Time_Portion()
        {
            var sut = DateRange.Day(new DateTime(2018, 1, 1));

            sut.IsInRange(new DateTime(2018, 1, 1, 23, 59, 59)).Should().BeTrue();
        }


        [TestMethod]
        public void Days_Throws_Exception_When_The_Number_Of_Days_Is_Less_Than_one()
        {
            Action action = () => DateRange.Days(DateTime.Today, 0);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Days_Creates_A_Range_That_Spans_The_Number_Of_Days()
        {
            var sut = DateRange.Days(DateTime.Today,2);
            sut.Start.Should().Be(DateTime.Today);
            sut.End.Should().Be(DateTime.Today.AddDays(1));
        }

        [TestMethod]
        public void Operator_Plus_Throws_Exception_When_The_Ranges_Are_Not_Adjacent()
        {
            var one = DateRange.Year(2018);
            var other = DateRange.Year(2020);

            Action action = () => { var sut = one + other; };

            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Operator_Minus_Cannot_Subtract_A_Larger_Range()
        {
            var one = new DateRange(new DateTime(2018, 1, 1), new DateTime(2018, 12, 31));
            var other = new DateRange(new DateTime(2017, 1, 1), new DateTime(2018, 12, 31));

            Action action = () => { var sut = one - other; };

            action.Should().Throw<ArgumentOutOfRangeException>();

        }

        [TestMethod]
        public void Operator_Minus_Can_Subtract_Bound_Ranges()
        {
            var one = new DateRange(new DateTime(2017, 1, 1), new DateTime(2018, 12, 31));
            var other = new DateRange(new DateTime(2018, 1, 1), new DateTime(2018, 12, 31));

            var sut = one - other;

            sut.Start.Should().Be(new DateTime(2017, 1, 1));
            sut.End.Should().Be(new DateTime(2017, 12, 31));

        }

        [TestMethod]
        public void Operator_Minus_Creates_A_Smaller_DateRange()
        {
            var one = new DateRange(null, null);
            var other = new DateRange(null, new DateTime(2018, 12, 31));

            var sut = one - other;

            sut.Start.Should().Be(new DateTime(2019, 1, 1));
            sut.End.Should().BeNull();
        }

        [TestMethod]
        public void Operator_Minus_When_Subtracting_The_Same_Ranges_Returns_Empty()
        {
            var one = new DateRange(null, null);
            var other = DateRange.Year(2018);

            var sut = (one - one);

            sut.IsEmpty.Should().BeTrue();

        }

        [TestMethod]
        public void IsAdjacentTo_Returns_The_Right_Value_For_Adjacent_Ranges()
        {
            var one = DateRange.Year(2018);
            var other = DateRange.Year(2019);
            var wrong = DateRange.Year(2021);
            var indefinite = new DateRange();

            one.IsAdjacentTo(other).Should().BeTrue();
            other.IsAdjacentTo(one).Should().BeTrue();

            one.IsAdjacentTo(wrong).Should().BeFalse();
            wrong.IsAdjacentTo(one).Should().BeFalse();

            indefinite.IsAdjacentTo(wrong).Should().BeFalse();
        }

        [TestMethod]
        public void Operator_Equals_Returns_True_For_Same_Ranges()
        {
            var one = DateRange.Year(2018);
            var other = DateRange.Year(2018);

            (one == other).Should().BeTrue();
            (one != other).Should().BeFalse();
        }

        [TestMethod]
        public void Hashcode_Does_Not_Throw_Exception_On_EmptyDates()
        {
            var sut = new DateRange();
            sut.GetHashCode().Should().Be(0);
        }

        [TestMethod]
        public void ToString_Returns_The_Default_Format()
        {
            var sut = DateRange.Year(2018);

            sut.ToString(null, CultureInfo.InvariantCulture).Should().Be("01/01/2018 - 12/31/2018");
        }

        [TestMethod]
        public void ToString_For_Empty_Range_Returns_Never()
        {
            var sut = new DateRange();

            sut.ToString(null, CultureInfo.InvariantCulture).Should().Be("Never");
        }

        [TestMethod]
        public void ToString_For_Unbound_Range_Returns_Always()
        {
            var sut = new DateRange(null, null);

            sut.ToString(null, CultureInfo.InvariantCulture).Should().Be("Always");
        }

        [TestMethod]
        public void ToString_For_Range_That_Is_Not_Bounded_At_The_Start_Returns_Less_Than_End()
        {
            var sut = new DateRange(null, new DateTime(2018, 12, 31));

            sut.ToString(null, CultureInfo.InvariantCulture).Should().Be("≤ 12/31/2018");
        }
        [TestMethod]
        public void ToString_For_Range_That_Is_Not_Bounded_At_The_End_Returns_Greater_Than_Start()
        {
            var sut = new DateRange(new DateTime(2018,1,1), null);

            sut.ToString(null, CultureInfo.InvariantCulture).Should().Be("≥ 01/01/2018");
        }


        [TestMethod]
        public void Xml_Serialization_RoundTrip_Unbounded()
        {
            var expected = DateRange.Always;
            var sut = expected.RoundTrip();

            sut.Start.Should().Be(expected.Start);
            sut.End.Should().Be(expected.End);
        }

        [TestMethod]
        public void Xml_Serialization_RoundTrip_Empty()
        {
            var expected = DateRange.Never;
            var sut = expected.RoundTrip();

            sut.Start.Should().Be(expected.Start);
            sut.End.Should().Be(expected.End);
        }

        [TestMethod]
        public void Xml_Serialization_RoundTrip_StartBounded()
        {
            var expected = new DateRange(DateTime.Today, null);
            var sut = expected.RoundTrip();

            sut.Start.Should().Be(expected.Start);
            sut.End.Should().Be(expected.End);
        }

        [TestMethod]
        public void Xml_Serialization_RoundTrip_EndBounded()
        {
            var expected = new DateRange(null,DateTime.Today);
            var sut = expected.RoundTrip();

            sut.Start.Should().Be(expected.Start);
            sut.End.Should().Be(expected.End);
        }

        [TestMethod]
        public void Xml_Serialization_RoundTrip_Bounded()
        {
            var expected = DateRange.Today();
            var sut = expected.RoundTrip();

            sut.Start.Should().Be(expected.Start);
            sut.End.Should().Be(expected.End);
        }

    }
}
