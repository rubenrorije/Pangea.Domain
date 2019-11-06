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

        [TestMethod]
        public void Default_Representation_Of_Angle_Includes_Degree_Symbol()
        {
            var sut = new Angle(90);
            sut.ToString().Should().Be("90°");
        }

        [TestMethod]
        public void Numeric_Format_String_Does_Not_Include_Degree_Symbol()
        {
            var sut = new Angle(90);
            sut.ToString("N0").Should().Be("90");
        }

        [TestMethod]
        public void Compare_Angles()
        {
            (new Angle(180) > new Angle(90)).Should().BeTrue();
            (new Angle(180) >= new Angle(90)).Should().BeTrue();
            (new Angle(180) < new Angle(90)).Should().BeFalse();
            (new Angle(180) <= new Angle(90)).Should().BeFalse();

            (new Angle(90) > new Angle(180)).Should().BeFalse();
            (new Angle(90) >= new Angle(180)).Should().BeFalse();
            (new Angle(90) < new Angle(180)).Should().BeTrue();
            (new Angle(90) <= new Angle(180)).Should().BeTrue();
        }

        [TestMethod]
        public void Negated_Angle_Returns_Correct_Result()
        {
            (-new Angle(90)).Should().Be(new Angle(270));
            (-new Angle(180)).Should().Be(new Angle(180));
        }

        [TestMethod]
        public void Add_Angle_Will_Modulo_360()
        {
            var lhs = new Angle(270);
            var rhs = new Angle(270);

            var result = lhs + rhs;

            result.Should().Be(new Angle(180));
        }

        [TestMethod]
        public void Subtract_Angle_Will_Simplify()
        {
            var lhs = new Angle(180);
            var rhs = new Angle(270);

            var result = lhs - rhs;

            result.Should().Be(new Angle(-90));
        }

        [TestMethod]
        public void ToRadians_Will_Be_Correct_Result()
        {
            new Angle(0).ToRadians().Should().Be(0);
            new Angle(180).ToRadians().Should().Be(Math.PI);
            new Angle(30).ToRadians().Should().BeApproximately(0.5236, 0.0001);
        }

        [TestMethod]
        [DataRow("N", 0)]
        [DataRow("NNE", 22.5)]
        [DataRow("NE", 45)]
        [DataRow("ENE", 67.5)]
        [DataRow("E", 90)]
        [DataRow("ESE", 112.5)]
        [DataRow("SE", 135)]
        [DataRow("SSE", 157.5)]
        [DataRow("S", 180)]
        [DataRow("SSW", 202.5)]
        [DataRow("SW", 225)]
        [DataRow("WSW", 247.5)]
        [DataRow("W", 270)]
        [DataRow("WNW", 292.5)]
        [DataRow("NW", 315)]
        [DataRow("NNW", 337.5)]
        public void ToCardinalDirection_Returns_Right_Direction_For_Existing_Directions(string direction, double angle)
        {
            var sut = new Angle(angle);
            sut.ToCardinalDirection().Should().Be(direction);
        }

        [TestMethod]
        [DataRow("N", 0)]
        [DataRow("N", 11)]
        [DataRow("NNE", 11.25)]
        [DataRow("NNE", 11.26)]
        [DataRow("N", -11)]
        [DataRow("N", -11.24)]
        [DataRow("N", -11.25)]
        public void ToCardinalDirection_Returns_The_Closest_Direction(string direction, double angle)
        {
            var sut = new Angle(angle);
            sut.ToCardinalDirection().Should().Be(direction);
        }

        [TestMethod]
        [DataRow("N", 0)]
        [DataRow("E", 46)]
        [DataRow("E", 91)]
        [DataRow("S", 183)]
        [DataRow("W", 280)]
        [DataRow("N", 345)]
        public void ToCardinalDirection_Only_Using_Cardinal_Directions(string direction, int angle)
        {
            var sut = new Angle(angle);
            sut.ToCardinalDirection(Direction.Cardinal).Should().Be(direction);
        }
    }
}
