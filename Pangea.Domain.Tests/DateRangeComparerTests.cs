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
        public void Unbounded_Compare_Returns_Zero()
        {
            var sut = new DateRangeComparer();

            Assert.Inconclusive();
        }
    }
}
