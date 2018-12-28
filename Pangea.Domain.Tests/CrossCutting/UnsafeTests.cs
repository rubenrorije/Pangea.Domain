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
    public class UnsafeTests
    {
        [TestMethod]
        public void Struct_That_Has_A_String_Constructor_Must_Have_A_Static_Unsafe_Method_To_Create_An_Instance()
        {
            var types =
                AssemblyUnderTest
                .Instance
                .Types()
                .ThatAreStructs()
                .ThatHaveAConstructorWithArguments<string>();

            using (new AssertionScope())
            {
                foreach (var type in types)
                {
                    if (type == typeof(PhoneNumber)) continue; // ignore
                    if (type == typeof(SmartFolder)) continue; // ignore

                    var methods =
                        type
                        .GetMethods()
                        .Where(m => m.ReturnType == type)
                        .Where(m => m.Name == "Unsafe")
                        .Where(m => m.IsStatic)
                        .Where(m => m.GetParameters().Count() == 1)
                        .Where(m => m.GetParameters()[0].ParameterType == typeof(string))
                        .ToList();

                    methods.Any().Should().BeTrue($"when {type.Name} has a constructor with a string argument, it also must have a static Unsafe(string) method");
                }
            }
        }
    }
}
