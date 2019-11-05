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
    public class AngleTests
    {
        [TestMethod]
        public void Empty_Angle_Is_Zero()
        {
            var sut = new Angle();

            sut.Degrees.Should().Be(0);
        }

    }
}
