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
    public class DateRangeComparerTests
    {
        [TestMethod]
        public void Never_Must_Be_Smallest()
        {
            var sut = new DateRangeComparer();

            sut.Compare(DateRange.Never, DateRange.Always).Should().Be(-1);
            sut.Compare(DateRange.Always, DateRange.Never).Should().Be(1);
        }

        [TestMethod]
        public void Unbounded_Compare_Returns_Zero()
        {
            var sut = new DateRangeComparer();

            sut.Compare(DateRange.Always, DateRange.Always).Should().Be(0);
        }

        [TestMethod]
        public void When_Different_Start_Dates_Compare_By_StartDate()
        {
            var sut = new DateRangeComparer();

            sut.Compare(DateRange.Year(2017), DateRange.Year(2018)).Should().Be(-1);
            sut.Compare(DateRange.Year(2018), DateRange.Year(2017)).Should().Be(1);
        }

        [TestMethod]
        public void When_Same_Start_Dates_Compare_By_End_Date()
        {
            var sut = new DateRangeComparer();

            sut.Compare(DateRange.Year(2017), DateRange.Years(2017, 2)).Should().Be(-1);
            sut.Compare(DateRange.Years(2017, 2), DateRange.Year(2017)).Should().Be(1);
        }

        [TestMethod]
        public void Order_Collection_By_Comparer()
        {
            var list = new List<DateRange>
            {
                DateRange.Always,
                DateRange.Year(2018),
                DateRange.Year(2017),
                DateRange.Years(2018,2),
                DateRange.Day(new DateTime(2018,1,1)),
                DateRange.Days(new DateTime(2018,1,1),2),
                DateRange.Never
            };

            var ordered = list.OrderBy(x => x, new DateRangeComparer()).ToList();

            ordered[0].Should().Be(DateRange.Never);
            ordered[1].Should().Be(DateRange.Always);
            ordered[2].Should().Be(DateRange.Year(2017));
            ordered[3].Should().Be(DateRange.Day(new DateTime(2018, 1, 1)));
            ordered[4].Should().Be(DateRange.Days(new DateTime(2018, 1, 1), 2));
            ordered[5].Should().Be(DateRange.Year(2018));
            ordered[6].Should().Be(DateRange.Years(2018, 2));
        }
    }
}
