using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests.CrossCutting
{
    [TestClass]
    public class ExtensionMethodsTests
    {
        [TestMethod]
        public void All_Classes_In_ExtensionMethods_Namespace_Are_Static()
        {
            var types =
                typeof(PhoneNumber)
                .Assembly
                .Types()
                .ThatAreInNamespace("Pangea.Domain.ExtensionMethods")
                .ThatArePublicOrInternal();

            using (new AssertionScope())
            {
                foreach (var t in types)
                {
                    t.IsAbstract.Should().BeTrue($"{t.Name} must be static");
                    t.IsSealed.Should().BeTrue($"{t.Name} must be static");
                }
            }
        }

        [TestMethod]
        public void All_Methods_In_ExtensionMethods_Namespace_Are_Extension_Methods()
        {
            var types =
                typeof(PhoneNumber)
                .Assembly
                .Types()
                .ThatAreInNamespace("Pangea.Domain.ExtensionMethods")
                .ThatArePublicOrInternal();

            types
                .Methods()
                .Should()
                .BeDecoratedWith<ExtensionAttribute>();
        }
    }
}
