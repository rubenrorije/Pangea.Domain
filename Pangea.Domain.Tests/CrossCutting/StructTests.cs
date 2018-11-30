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
    public class StructTests
    {
        public static IEnumerable<Type> DomainClasses =>
            typeof(FileSize)
            .Assembly
            .GetTypes()
            .Where(t => t.IsValueType)
            .Where(t => t.BaseType != typeof(Enum))
            .ToList();

        [TestMethod]
        public void A_Struct_Must_Implement_IEquatable()
        {
            foreach (var t in DomainClasses)
            {
                var interfaces = t.GetInterfaces().Where(i => i.Name.Equals("IEquatable`1")).ToList();

                interfaces.Any(i => i.GenericTypeArguments.Single() == t).Should().BeTrue($"{t.Name} should implement IEquatable<{t.Name}>");
            }
        }

        [TestMethod]
        public void A_Struct_That_Implements_IEquatable_Must_Override_Equals()
        {
            foreach (var t in DomainClasses)
            {
                var interfaces = t.GetInterfaces().Where(i => i.Name.Equals("IEquatable`1")).ToList();

                if (interfaces.Any(i => i.GenericTypeArguments.Single() == t))
                {
                    var equalsMethod = t.GetMethod("Equals", new Type[] { typeof(object) });
                    equalsMethod.DeclaringType.Should().Be(t, $"Equals function must be overridden in {t.Name}");
                }
            }

        }
    }
}
