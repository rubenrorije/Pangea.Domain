using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
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
                AssemblyUnderTest
                .Instance
                .Types()
                .ThatAreInNamespace("Pangea.Domain.Checksums");

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
