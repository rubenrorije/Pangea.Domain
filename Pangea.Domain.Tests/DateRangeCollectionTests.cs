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
    public class DateRangeCollectionTests
    {
        [TestMethod]
        public void Create_With_A_Single_DateRange()
        {
            var sut = new DateRangeCollection(DateRange.Today());
            sut.Count.Should().Be(1);
        }

        [TestMethod]
        public void Create_With_Multiple_DateRanges()
        {
            var sut = new DateRangeCollection(DateRange.Today(), DateRange.Today());
            sut.Count.Should().Be(2);
        }

        [TestMethod]
        public void Must_Implement_IEnumerable()
        {
            var one = DateRange.Today();
            var second = DateRange.Today();

            var sut = new DateRangeCollection(one, second);

            sut.ToList().Should().BeEquivalentTo(new[] { one, second });
        }

        [TestMethod]
        public void Collection_Initialization_Is_Possible()
        {
            var sut = new DateRangeCollection
            {
                DateRange.Today(),
                DateRange.Today()
            };

            sut.Count.Should().Be(2);
        }

        [TestMethod]
        [DataRow(2017, 2018, 2017, true)] // simple valid case
        [DataRow(2017, 2018, 2019, false)] // simple invalid case
        [DataRow(2017, 2017, 2017, true)] // multiple ranges that are valid
        [DataRow(2017, 2017, 2018, false)]// multiple ranges that are invalid
        public void Contains_Returns_The_Right_Result_For_A_Date(int year1, int year2, int yearDate, bool result)
        {
            var sut = new DateRangeCollection
            {
                DateRange.Year(year1),
                DateRange.Year(year2)
            };

            sut.IsInRange(new DateTime(yearDate, 6, 6)).Should().Be(result);
        }

        [TestMethod]
        [DataRow(null, null, null, null, true)] // both unbounded
        [DataRow(null, "2018-01-01", "2018-01-01", null, true)] // just overlapping
        [DataRow(null, "2017-12-31", "2018-01-01", null, false)] // just not overlapping
        [DataRow(null, null, "2018-01-01", "2018-01-01", true)] // containing
        [DataRow("2018-01-01", "2018-01-01", null, null, true)] // containing other way
        [DataRow("2018-01-01", "2018-12-31", "2019-06-01", "2019-06-30", false)] // bounded not overlapping dates
        public void IsOverlapping_Returns_The_Right_Result(string start1, string end1, string start2, string end2, bool result)
        {
            var s1 = start1 == null ? (DateTime?)null : DateTime.Parse(start1);
            var e1 = end1 == null ? (DateTime?)null : DateTime.Parse(end1);
            var s2 = start2 == null ? (DateTime?)null : DateTime.Parse(start2);
            var e2 = end2 == null ? (DateTime?)null : DateTime.Parse(end2);

            var sut = new DateRangeCollection
            {
                new DateRange(s1, e1),
                new DateRange(s2, e2)
            };
            sut.IsOverlapping().Should().Be(result);
        }

        [TestMethod]
        public void Ordered_Collection()
        {

            var collection = new DateRangeCollection
            {
                DateRange.Always,
                DateRange.Year(2018),
                DateRange.Year(2017),
                DateRange.Years(2018, 2),
                DateRange.Day(new DateTime(2018, 1, 1)),
                DateRange.Days(new DateTime(2018, 1, 1),2),
                DateRange.Never
            };

            var ordered = collection.Ordered();

            ordered[0].Should().Be(DateRange.Never);
            ordered[1].Should().Be(DateRange.Always);
            ordered[2].Should().Be(DateRange.Year(2017));
            ordered[3].Should().Be(DateRange.Day(new DateTime(2018, 1, 1)));
            ordered[4].Should().Be(DateRange.Days(new DateTime(2018, 1, 1), 2));
            ordered[5].Should().Be(DateRange.Year(2018));
            ordered[6].Should().Be(DateRange.Years(2018, 2));
        }

        [TestMethod]
        public void IsInRangeIndexed_Returns_The_First_Index_Of_The_Range_That_Contains_The_Date()
        {

            var collection = new DateRangeCollection
            {
                DateRange.Year(2016),
                DateRange.Year(2017),
                DateRange.Year(2018),
                DateRange.Years(2018, 2),
                DateRange.Day(new DateTime(2019, 1, 1)),
            };

            collection.InRangeIndexed(new DateTime(2016, 6, 6)).Should().Be(0);
            collection.InRangeIndexed(new DateTime(2018, 6, 6)).Should().Be(2);
            collection.InRangeIndexed(new DateTime(2019, 1, 1)).Should().Be(3);
            collection.InRangeIndexed(new DateTime(2020, 1, 2)).Should().Be(-1);
        }

        [TestMethod]
        public void IsChain_With_Adjacent_Ranges_Should_Be_True()
        {
            var collection = new DateRangeCollection
            {
                DateRange.Year(2016),
                DateRange.Year(2018),
                DateRange.Year(2017),
            };

            collection.IsSingleChain().Should().BeTrue();
            collection.GetSingleChain().Should().Be(DateRange.Years(2016, 3));
        }

        [TestMethod]
        public void IsChain_With_Overlapping_Ranges_Should_Be_False()
        {
            var collection = new DateRangeCollection
            {
                DateRange.Year(2016),
                DateRange.Year(2018),
                DateRange.Year(2017),
                DateRange.Year(2018),
            };

            collection.IsSingleChain().Should().BeFalse();
            collection.GetSingleChain().IsEmpty.Should().BeTrue();
        }

        [TestMethod]
        public void IsChain_With_Non_Adjacent_Ranges_Should_Be_False()
        {
            var collection = new DateRangeCollection
            {
                DateRange.Year(2016),
                DateRange.Year(2018),
                DateRange.Year(2019),
            };

            collection.IsSingleChain().Should().BeFalse();
            collection.GetSingleChain().IsEmpty.Should().BeTrue();
        }

        [TestMethod]
        public void Chain_With_Unbounded_Ranges_Creates_Always_Single_Chain()
        {
            var collection = new DateRangeCollection
            {
                new DateRange(null, new DateTime(2017,12,31)),
                new DateRange(new DateTime(2018,1,1), null),
            };

            collection.IsSingleChain().Should().BeTrue();
            collection.GetSingleChain().Should().Be(DateRange.Always);
        }

        [TestMethod]
        public void IsSingleChain_Not_Allowed_When_There_Are_No_Ranges()
        {
            var collection = new DateRangeCollection();
            Action action = () => collection.IsSingleChain();
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void GetSingleChain_Returns_Never_When_There_Are_No_Ranges_Specified()
        {
            var collection = new DateRangeCollection();
            collection.GetSingleChain().IsEmpty.Should().BeTrue();
        }
    }
}
