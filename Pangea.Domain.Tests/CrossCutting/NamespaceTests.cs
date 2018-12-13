using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests.CrossCutting
{
    [TestClass]
    public class NamespaceTests
    {
        private static readonly IEnumerable<Type> _allTypes = typeof(PhoneNumber).Assembly.GetTypes();

        [TestMethod]
        public void All_Types_In_The_Checksums_Namespace_Are_Internal()
        {
            var incorrect =
                typeof(PhoneNumber)
                .Assembly
                .Types()
                .ThatAreInNamespace("Pangea.Domain.Checksums")
                .ToList();

            using (new AssertionScope())
            {
                foreach (var t in incorrect)
                {
                    t.IsPublic.Should().BeFalse($"{t.Name} should be internal");
                }
            }
        }
    }
}
