﻿using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pangea.Domain.Tests.CrossCutting
{
    [TestClass]
    public class StructTests
    {
        private static TypeSelector Structs =>
            AssemblyUnderTest
            .Instance
            .Types()
            .ThatAreStructs();

        [TestMethod]
        public void A_Struct_Must_Implement_IEquatable()
        {
            using (new AssertionScope())
            {
                foreach (var t in Structs)
                {
                    var interfaces = t.GetInterfaces().Where(i => i.Name.Equals("IEquatable`1")).ToList();

                    interfaces.Any(i => i.GenericTypeArguments.Single() == t).Should().BeTrue($"{t.Name} should implement IEquatable<{t.Name}>");
                }
            }
        }

        [TestMethod]
        public void A_Struct_That_Implements_IEquatable_Must_Override_Equals()
        {
            using (new AssertionScope())
            {
                foreach (var t in Structs)
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


        [TestMethod]
        public void ToString_Must_Be_Overridden_For_All_Structs()
        {
            using (new AssertionScope())
            {
                foreach (var t in Structs)
                {
                    var toStringMethod = t.GetMethod(nameof(ToString), new Type[] { });
                    toStringMethod.DeclaringType.Should().Be(t, $"ToString() function must be overridden in {t.Name}");
                }
            }
        }

        [TestMethod]
        public void Structs_Must_Implement_IXmlSerializable()
        {
            using (new AssertionScope())
            {
                foreach (var type in Structs)
                {
                    type.Should().Implement<IXmlSerializable>();
                }
            }
        }

        [TestMethod]
        public void GetHashcode_Does_Not_Throw_When_Called_From_A_Default_Instance()
        {
            using (new AssertionScope())
            {
                foreach (var type in Structs)
                {
                    var instance = Activator.CreateInstance(type);
                    Action action = () => instance.GetHashCode();
                    action.Should().NotThrow<Exception>();
                }
            }
        }
        [TestMethod]
        public void Equals_For_Default_Instances_Returns_True()
        {
            using (new AssertionScope())
            {
                foreach (var type in Structs)
                {
                    var lhs = Activator.CreateInstance(type);
                    var rhs = Activator.CreateInstance(type);

                    lhs.Equals(rhs).Should().BeTrue();
                }
            }
        }
    }
}
