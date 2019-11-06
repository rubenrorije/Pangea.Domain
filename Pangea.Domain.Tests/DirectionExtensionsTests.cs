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
    public class DirectionExtensionsTests
    {
        [TestMethod]
        public void Get_Returns_The_Right_Number_Of_Directions()
        {
            Direction.Cardinal.Get().Should().HaveCount(4);
            Direction.InterCardinal.Get().Should().HaveCount(4);
            Direction.SecondaryInterCardinal.Get().Should().HaveCount(8);
        }
        [TestMethod]
        public void GetAll_Returns_The_Right_Number_Of_Directions()
        {
            Direction.Cardinal.GetAll().Should().HaveCount(4);
            Direction.InterCardinal.GetAll().Should().HaveCount(8);
            Direction.SecondaryInterCardinal.GetAll().Should().HaveCount(16);
        }

        [TestMethod]
        [DataRow("N", "↑")]
        [DataRow("E", "→")]
        [DataRow("S", "↓")]
        [DataRow("W", "←")]
        public void Convert_Cardinal_Direction_Into_Arrow(string direction, string arrow)
        {
            DirectionExtensions.ToArrow(direction).Should().Be(arrow);
        }

        [TestMethod]
        [DataRow("NE", "↗")]
        [DataRow("SE", "↘")]
        [DataRow("SW", "↙")]
        [DataRow("NW", "↖")]
        public void Convert_InterCardinal_Direction_Into_Arrow(string direction, string arrow)
        {
            DirectionExtensions.ToArrow(direction).Should().Be(arrow);
        }

        [TestMethod]
        public void Direction_That_Is_LowerCase_Can_Be_Converted_Into_An_Arrow()
        {
            DirectionExtensions.ToArrow("n").Should().Be("↑");
        }

        [TestMethod]
        public void An_Empty_Direction_Cannot_Be_Converted_Into_An_Arrow()
        {
            Action action = () => DirectionExtensions.ToArrow(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Cannot_Convert_SecondaryInterCardinalDirection_Into_Arrow()
        {
            Action action = () => DirectionExtensions.ToArrow("NNE");
            action.Should().Throw<ArgumentException>();
        }
    }
}
