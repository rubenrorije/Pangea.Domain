using FluentAssertions;
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
                _allTypes
                .Where(t => t.Namespace != null)
                .Where(t => t.Namespace.Contains(".Checksums"))
                .Where(t => t.IsPublic)
                .ToArray();

            incorrect.Should().HaveCount(0, $"{string.Join(", ", incorrect.Select(t => t.Name))} should not be public in namespace *.Checksums");
        }
    }
}
