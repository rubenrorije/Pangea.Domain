using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Types;
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
    public class ConvertibleTests
    {
        private TypeSelector Structs =>
            AssemblyUnderTest
            .Instance
            .Types()
            .ThatAreStructs();

        [TestMethod]
        public void Structs_Implement_IConvertible()
        {
            var incorrect =
                Structs
                .ThatDoNotImplement<IConvertible>()
                .ThatHaveAConstructorWith(c => c.GetParameters().Count() == 1)
                .ToList();

            incorrect.Should().BeEmpty();
        }

        [TestMethod]
        public void All_IConvertible_Methods_Must_Be_Explicitly_Defined()
        {
            var types =
                Structs
                .ThatHaveAConstructorWith(c => c.GetParameters().Count() == 1)
                .ThatImplement<IConvertible>()
                .ToList();

            var methods = typeof(IConvertible).GetMethods();


            using (new AssertionScope())
            {
                foreach (var type in types)
                {
                    foreach (var method in methods)
                    {
                        type.ImplementsExplicitly<IConvertible>(method).Should().BeTrue($"{type.Name} should implement {method.Name} explicitly");
                    }
                }
            }
        }
    }
}
