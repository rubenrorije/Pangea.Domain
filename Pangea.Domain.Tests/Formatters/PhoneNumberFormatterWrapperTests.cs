using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests.Formatters
{
    [TestClass]
    public class PhoneNumberFormatterWrapperTests
    {
        [TestMethod]
        public void PhoneNumberFormatter_Argument_Null_Throws_ArgumentNullException()
        {
            Action action = () => new PhoneNumberFormatterWrapper(null);
            action.Should().Throw<ArgumentNullException>();
        }

    }
}
