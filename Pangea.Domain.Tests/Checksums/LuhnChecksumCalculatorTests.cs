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
    public class LuhnChecksumCalculatorTests
    {
        [TestMethod]
        public void When_Subject_Is_Null_Throw_ArgumentNullException()
        {
            var sut = new LuhnChecksumCalculator();

            Action action = () => sut.Calculate(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Calculate_The_Correct_Result()
        {
            var sut = new LuhnChecksumCalculator();

            sut.Calculate("7992739871").Should().Be(3);
            sut.Calculate("123456789").Should().Be(7);
        }
    }
}
