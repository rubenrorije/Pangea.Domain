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

    }
}
