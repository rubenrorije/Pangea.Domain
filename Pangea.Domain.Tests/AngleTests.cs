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

        [TestMethod]
        public void Negative_Angle_Is_Allowed()
        {
            var sut = new Angle(-90);
        }

        [TestMethod]
        public void Negative_Angle_Is_Simplified()
        {
            var sut = new Angle(-90);
            sut.Degrees.Should().Be(270);
        }

        [TestMethod]
        public void Negative_Angle_Multiple_Of_360_Is_Simplified_()
        {
            var sut = new Angle(-540);
            sut.Degrees.Should().Be(180);
        }

        [TestMethod]
        public void Angle_Is_Simplified()
        {
            var sut = new Angle(540);
            sut.Degrees.Should().Be(180);
        }

        [TestMethod]
        public void Negative_Angle_And_Positive_Angle_Are_The_Same()
        {
            var lhs = new Angle(-90);
            var rhs = new Angle(270);

            lhs.Should().Be(rhs);
        }
    }
}
