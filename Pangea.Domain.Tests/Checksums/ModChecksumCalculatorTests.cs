using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Checksums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests.Checksums
{
    [TestClass]
    public class ModChecksumCalculatorTests
    {
        [TestMethod]
        public void Modulo_Cannot_Be_Negative()
        {
            Action action = () => new ModChecksumCalculator(-1);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Modulo_Cannot_Be_Zero()
        {
            Action action = () => new ModChecksumCalculator(0);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Calculating_For_A_Null_String_Throws_ArgumentNullException()
        {
            var sut = new ModChecksumCalculator(97);

            Action action = () => sut.Calculate(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Calculate_Returns_Correct_Result()
        {
            var sut = new ModChecksumCalculator(97);

            sut.Calculate("0").Should().Be(0);
            sut.Calculate("97").Should().Be(0);
            sut.Calculate("3").Should().Be(3);
            sut.Calculate("975").Should().Be(5);
        }
    }
}
