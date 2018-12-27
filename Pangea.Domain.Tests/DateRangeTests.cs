using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Fluent;
using Pangea.Domain.Tests.Util;
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
        public void Negative_Year_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => DateRange.Year(-1);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Negative_Number_Of_Years_Throws_ArgumentOutOfRangeException()
        {
            Action action = () => DateRange.Years(DateTime.Today.Year, -1);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Create_Years_Range_From_Year_Cannot_Be_Less_Than_1()
        {
            Action action = () => DateRange.Years(2018, 0);

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
            DateRange.Always.Should().Be(new DateRange(null, null));
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
            var sut = DateRange.Days(DateTime.Today, 2);
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
            var year2018 = DateRange.Year(2018);
            var year2019 = DateRange.Year(2019);
            var year2021 = DateRange.Year(2021);
            var never = DateRange.Never;

            year2018.IsAdjacentTo(year2019).Should().BeTrue();
            year2019.IsAdjacentTo(year2018).Should().BeTrue();

            year2018.IsAdjacentTo(year2021).Should().BeFalse();
            year2021.IsAdjacentTo(year2018).Should().BeFalse();

            never.IsAdjacentTo(year2021).Should().BeFalse();
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
            var sut = new DateRange(new DateTime(2018, 1, 1), null);

            sut.ToString(null, CultureInfo.InvariantCulture).Should().Be("≥ 01/01/2018");
        }

        [TestMethod]
        public void Deconstruct_Into_Parts()
        {
            var (start, end) = DateRange.Today().Deconstruct();
            start.Should().Be(DateTime.Today);
            end.Should().Be(DateTime.Today);
        }

        [TestMethod]
        public void Serialization_Of_Never()
        {
            var sut = DateRange.Never;
            sut.Should().BeXmlSerializable();
            sut.Should().BeBinarySerializable();
            sut.Should().BeDataContractSerializable();
        }

        [TestMethod]
        public void Serialization_Of_Both_Bounded()
        {
            var sut = DateRange.Today();
            sut.Should().BeXmlSerializable();
            sut.Should().BeBinarySerializable();
            sut.Should().BeDataContractSerializable();
        }

        [TestMethod]
        public void Serialization_Of_End_Bounded()
        {
            var sut = new DateRange(null, DateTime.Today);
            sut.Should().BeXmlSerializable();
            sut.Should().BeBinarySerializable();
            sut.Should().BeDataContractSerializable();
        }

        [TestMethod]
        public void Serialization_Of_Start_Bounded()
        {
            var sut = new DateRange(null, DateTime.Today);
            sut.Should().BeXmlSerializable();
            sut.Should().BeBinarySerializable();
            sut.Should().BeDataContractSerializable();
        }

        [TestMethod]
        public void OverlapsWith_Returns_False_For_Never_And_Always()
        {
            DateRange.Always.OverlapsWith(DateRange.Never).Should().BeFalse();
            DateRange.Never.OverlapsWith(DateRange.Always).Should().BeFalse();
        }

        [TestMethod]
        public void OverlapsWith_Returns_True_For_Always_And_Always()
        {
            DateRange.Always.OverlapsWith(DateRange.Always).Should().BeTrue();
        }

        [TestMethod]
        public void OverlapsWith_Returns_True_For_A_DateRange_That_Surrounds_Another()
        {
            DateRange.Always.OverlapsWith(DateRange.Today()).Should().BeTrue();
        }

        [TestMethod]
        public void OverlapsWith_Returns_True_For_A_DateRange_That_Have_The_Same_StartDate_And_Different_EndDates()
        {
            DateRange.Years(2017, 2).OverlapsWith(DateRange.Years(2017, 3)).Should().BeTrue();
            DateRange.Years(2017, 3).OverlapsWith(DateRange.Years(2017, 2)).Should().BeTrue();
        }

        [TestMethod]
        public void OverlapsWith_Returns_True_For_A_DateRange_That_Have_The_Same_EndDate_And_Different_StartDates()
        {
            var one = new DateRange(new DateTime(2017, 1, 1), new DateTime(2018, 1, 1));
            var other = new DateRange(new DateTime(2017, 2, 1), new DateTime(2018, 1, 1));

            one.OverlapsWith(other).Should().BeTrue();
            other.OverlapsWith(one).Should().BeTrue();
        }

        [TestMethod]
        public void Overlaps_With_Returns_False_When_The_Dates_Are_Not_Overlapping()
        {
            var Jan1 = DateRange.Day(new DateTime(2018, 1, 1));
            var Dec5 = DateRange.Day(new DateTime(2018, 12, 5));

            Jan1.OverlapsWith(Dec5).Should().BeFalse();
            Dec5.OverlapsWith(Jan1).Should().BeFalse();
        }
        [TestMethod]
        public void Overlaps_With_Returns_True_When_The_Ranges_Are_Just_Overlapping()
        {
            var untilJan1 = new DateRange(new DateTime(2017, 1, 1), new DateTime(2018, 1, 1));
            var fromJan1 = new DateRange(new DateTime(2018, 1, 1), new DateTime(2018, 12, 5));

            untilJan1.OverlapsWith(fromJan1).Should().BeTrue();
            fromJan1.OverlapsWith(untilJan1).Should().BeTrue();
        }

        [TestMethod]
        public void DateRange_For_Day_Can_Inline_Year_Month_Day()
        {
            DateRange.Day(2018, 1, 1).Should().Be(DateRange.Day(new DateTime(2018, 1, 1)));
        }

        [TestMethod]
        public void DateRange_Yesterday()
        {
            var sut = DateRange.Yesterday();
            sut.Start.Should().Be(DateTime.Today.AddDays(-1));
            sut.End.Should().Be(DateTime.Today.AddDays(-1));
        }

        [TestMethod]
        public void DateRange_Tomorrow()
        {
            var sut = DateRange.Tomorrow();
            sut.Start.Should().Be(DateTime.Today.AddDays(1));
            sut.End.Should().Be(DateTime.Today.AddDays(1));
        }

        [TestMethod]
        public void DateRange_FromDay()
        {
            var sut = DateRange.From(2018, 1, 1);
            sut.Start.Should().Be(new DateTime(2018, 1, 1));
            sut.End.Should().BeNull();
        }

        [TestMethod]
        public void DateRange_UntilDay()
        {
            var sut = DateRange.Until(2018, 1, 1);
            sut.Start.Should().BeNull();
            sut.End.Should().Be(new DateTime(2018, 1, 1));
        }

        [TestMethod]
        public void DateRange_Month_Year_Argument_Cannot_Be_Zero_Or_Less()
        {
            Action action = () => DateRange.Month(0, 11);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void DateRange_Month_Month_Argument_Cannot_Be_Zero_Or_Less()
        {
            Action action = () => DateRange.Month(2018, 0);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void DateRange_One_Month()
        {
            var nov = DateRange.Month(2018, 11);
            nov.Start.Should().Be(new DateTime(2018, 11, 1));
            nov.End.Should().Be(new DateTime(2018, 11, 30));
        }

        [TestMethod]
        public void DateRange_One_Month_Leap_Year()
        {
            var nov = DateRange.Month(2016, 2);
            nov.Start.Should().Be(new DateTime(2016, 2, 1));
            nov.End.Should().Be(new DateTime(2016, 2, 29));
        }

        [TestMethod]
        public void DateRange_One_Month_No_Leap_Year()
        {
            var nov = DateRange.Month(2015, 2);
            nov.Start.Should().Be(new DateTime(2015, 2, 1));
            nov.End.Should().Be(new DateTime(2015, 2, 28));
        }

        [TestMethod]
        public void DateRange_Multiple_Months_The_Number_Of_Months_Must_Be_Positive()
        {
            Action action = () => DateRange.Months(2018, 1, 0);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void DateRange_Multiple_Months()
        {
            var months = DateRange.Months(2018, 1, 12);

            months.Should().Be(DateRange.Year(2018));
        }

        [TestMethod]
        public void Weeks_Throws_Exception_When_Non_Positive_Number_Of_Weeks()
        {
            Action action = () => DateRange.Weeks(new DateTime(2018, 1, 1), -1);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void TotalDays_Always_Must_Be_Int_MaxValue()
        {
            DateRange.Always.TotalDays.Should().Be(int.MaxValue);
        }


        [TestMethod]
        public void TotalDays_Never_Must_Be_Zero()
        {
            DateRange.Never.TotalDays.Should().Be(0);
        }

        [TestMethod]
        public void TotalDays_Day_Must_Be_One()
        {
            DateRange.Today().TotalDays.Should().Be(1);
        }

        [TestMethod]
        public void TotalDays_Year_2018_Must_Be_365()
        {
            DateRange.Year(2018).TotalDays.Should().Be(365);
        }

        [TestMethod]
        public void Weeks_Number_Of_Days_Equal_To_7()
        {
            var sut = DateRange.Weeks(new DateTime(2018, 1, 1), 1);
            sut.TotalDays.Should().Be(7);
        }

        [TestMethod]
        public void Week_Must_Start_At_The_First_Day_Of_The_Week()
        {
            var expected = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var sut = DateRange.Weeks(new DateTime(2019, 1, 1), 1);
            sut.Start.Value.DayOfWeek.Should().Be(expected, "the first day of the week must match the one specified in the culture");
        }

        // sunday
        [DataRow("2017-01-01", "2017-01-01", DayOfWeek.Sunday)]
        [DataRow("2017-01-01", "2016-12-26", DayOfWeek.Monday)]
        [DataRow("2017-01-01", "2016-12-27", DayOfWeek.Tuesday)]
        [DataRow("2017-01-01", "2016-12-28", DayOfWeek.Wednesday)]
        [DataRow("2017-01-01", "2016-12-29", DayOfWeek.Thursday)]
        [DataRow("2017-01-01", "2016-12-30", DayOfWeek.Friday)]
        [DataRow("2017-01-01", "2016-12-31", DayOfWeek.Saturday)]
        // monday
        [DataRow("2018-01-01", "2017-12-31", DayOfWeek.Sunday)]
        [DataRow("2018-01-01", "2018-01-01", DayOfWeek.Monday)]
        [DataRow("2018-01-01", "2017-12-26", DayOfWeek.Tuesday)]
        [DataRow("2018-01-01", "2017-12-27", DayOfWeek.Wednesday)]
        [DataRow("2018-01-01", "2017-12-28", DayOfWeek.Thursday)]
        [DataRow("2018-01-01", "2017-12-29", DayOfWeek.Friday)]
        [DataRow("2018-01-01", "2017-12-30", DayOfWeek.Saturday)]
        // tuesday
        [DataRow("2019-01-01", "2018-12-30", DayOfWeek.Sunday)]
        [DataRow("2019-01-01", "2018-12-31", DayOfWeek.Monday)]
        [DataRow("2019-01-01", "2019-01-01", DayOfWeek.Tuesday)]
        [DataRow("2019-01-01", "2018-12-26", DayOfWeek.Wednesday)]
        [DataRow("2019-01-01", "2018-12-27", DayOfWeek.Thursday)]
        [DataRow("2019-01-01", "2018-12-28", DayOfWeek.Friday)]
        [DataRow("2019-01-01", "2018-12-29", DayOfWeek.Saturday)]
        // wednesday
        [DataRow("2014-01-01", "2013-12-29", DayOfWeek.Sunday)]
        [DataRow("2014-01-01", "2013-12-30", DayOfWeek.Monday)]
        [DataRow("2014-01-01", "2013-12-31", DayOfWeek.Tuesday)]
        [DataRow("2014-01-01", "2014-01-01", DayOfWeek.Wednesday)]
        [DataRow("2014-01-01", "2013-12-26", DayOfWeek.Thursday)]
        [DataRow("2014-01-01", "2013-12-27", DayOfWeek.Friday)]
        [DataRow("2014-01-01", "2013-12-28", DayOfWeek.Saturday)]
        // thursday
        [DataRow("2015-01-01", "2014-12-28", DayOfWeek.Sunday)]
        [DataRow("2015-01-01", "2014-12-29", DayOfWeek.Monday)]
        [DataRow("2015-01-01", "2014-12-30", DayOfWeek.Tuesday)]
        [DataRow("2015-01-01", "2014-12-31", DayOfWeek.Wednesday)]
        [DataRow("2015-01-01", "2015-01-01", DayOfWeek.Thursday)]
        [DataRow("2015-01-01", "2014-12-26", DayOfWeek.Friday)]
        [DataRow("2015-01-01", "2014-12-27", DayOfWeek.Saturday)]
        // friday
        [DataRow("2010-01-01", "2009-12-27", DayOfWeek.Sunday)]
        [DataRow("2010-01-01", "2009-12-28", DayOfWeek.Monday)]
        [DataRow("2010-01-01", "2009-12-29", DayOfWeek.Tuesday)]
        [DataRow("2010-01-01", "2009-12-30", DayOfWeek.Wednesday)]
        [DataRow("2010-01-01", "2009-12-31", DayOfWeek.Thursday)]
        [DataRow("2010-01-01", "2010-01-01", DayOfWeek.Friday)]
        [DataRow("2010-01-01", "2009-12-26", DayOfWeek.Saturday)]
        // saturday
        [DataRow("2011-01-01", "2010-12-26", DayOfWeek.Sunday)]
        [DataRow("2011-01-01", "2010-12-27", DayOfWeek.Monday)]
        [DataRow("2011-01-01", "2010-12-28", DayOfWeek.Tuesday)]
        [DataRow("2011-01-01", "2010-12-29", DayOfWeek.Wednesday)]
        [DataRow("2011-01-01", "2010-12-30", DayOfWeek.Thursday)]
        [DataRow("2011-01-01", "2010-12-31", DayOfWeek.Friday)]
        [DataRow("2011-01-01", "2011-01-01", DayOfWeek.Saturday)]
        [TestMethod]
        public void Week_Must_Start_At_The_First_Day_Of_The_Week_For_All_Days(string day, string start, DayOfWeek startingDay)
        {
            var dayWithinWeek = DateTime.Parse(day);
            var startDate = DateTime.Parse(start);

            var sut = DateRange.Weeks(dayWithinWeek, 1, startingDay);

            sut.Start.Value.DayOfWeek.Should().Be(startingDay);
            sut.Start.Value.Should().Be(startDate);
        }

        [TestMethod]
        public void Fluent_Interface_Until_Creates_Correct_DateRange()
        {
            new DateTime(2018, 1, 1).Until(new DateTime(2018, 12, 31)).Should().Be(DateRange.Year(2018));
        }

        [TestMethod]
        public void Fluent_Interface_Until_Inline_Date_Creates_Correct_DateRange()
        {
            new DateTime(2018, 1, 1).Until(2018, 12, 31).Should().Be(DateRange.Year(2018));
        }

        [TestMethod]
        public void Fluent_Interface_Until_Forever_Creates_Correct_DateRange()
        {
            var sut = new DateTime(2018, 1, 1).UntilForever();
            sut.Start.Should().Be(new DateTime(2018, 1, 1));
            sut.End.Should().BeNull();
        }

        [TestMethod]
        public void Enumerable_Dates_Must_Be_Lazy()
        {
            var sut = DateRange.Always.Dates();
            Action action = () => sut.Take(2).ToList();

            action.ExecutionTime().Should().BeLessThan(TimeSpan.FromMilliseconds(100));
        }

        [TestMethod]
        public void Enumerable_Dates_Returns_365_Dates_For_A_Year()
        {
            var sut = DateRange.Year(2018).Dates();
            sut.ToList().Count().Should().Be(365);
            sut.First().Should().Be(new DateTime(2018, 1, 1));
            sut.Last().Should().Be(new DateTime(2018, 12, 31));
        }

        [TestMethod]
        public void Enumerable_Dates_Returns_One_Date_For_One_Day()
        {
            var sut = DateRange.Today().Dates();
            sut.ToList().Count().Should().Be(1);
            sut.First().Should().Be(DateTime.Today);
        }

        [TestMethod]
        public void Enumerable_Dates_Returns_0_Days_For_Never()
        {
            var sut = DateRange.Never.Dates();
            sut.ToList().Count().Should().Be(0);
        }
    }
}
