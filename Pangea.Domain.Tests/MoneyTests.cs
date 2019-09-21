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
    public class MoneyTests
    {
        [TestMethod]
        public void Currency_Must_Be_Specified()
        {
            Action action = () => new Money(null, 5);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Default_Is_Empty()
        {
            default(Money).ToString().Should().BeEmpty();
        }
    }
}
