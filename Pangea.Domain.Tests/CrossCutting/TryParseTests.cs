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
    public class TryParseTests
    {
        public static IEnumerable<Type> DomainClasses =>
            typeof(FileSize)
            .Assembly
            .GetTypes()
            .Where(t => t.IsValueType)
            .Where(t => t.BaseType != typeof(Enum))
            .ToList();

        [TestMethod]
        public void All_Structs_That_Have_A_Single_String_Value_Constructor_Must_Implement_TryParse()
        {
            var typesToCheck =
                DomainClasses
                .Where(HasStringParameterConstructor);

            foreach (var t in typesToCheck)
            {
                t
                .GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Where(m => m.Name.Equals("TryParse"))
                .Where(m => m.ReturnParameter.ParameterType == typeof(bool))
                .Where(m => m.GetParameters().Length== 2)
                .Where(m => m.GetParameters()[0].ParameterType == typeof(string))
                .Where(m=> m.GetParameters()[1].IsOut)
                .Any()
                .Should()
                .BeTrue($"{t.Name} must have a TryParse method");
            }
        }

        private bool HasStringParameterConstructor(Type t)
        {
            foreach (var c in t.GetConstructors())
            {
                var parameters = c.GetParameters();
                if (parameters.Length != 1) continue;
                if (parameters[0].ParameterType != typeof(string)) continue;

                return true;
            }
            return false;
        }

    }
}
