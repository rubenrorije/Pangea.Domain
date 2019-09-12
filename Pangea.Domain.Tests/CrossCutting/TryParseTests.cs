using FluentAssertions;
using FluentAssertions.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests.CrossCutting
{
    [TestClass]
    public class TryParseTests
    {
        private static TypeSelector Structs =>
            AssemblyUnderTest
            .Instance
            .Types()
            .ThatAreStructs();

        [TestMethod]
        public void All_Structs_That_Have_A_Single_String_Value_Constructor_Must_Implement_TryParse()
        {
            var typesToCheck =
                Structs
                .Where(HasStringParameterConstructor);

            foreach (var t in typesToCheck)
            {
                t
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => IsTryParseMethod(m, typeof(string)))
                .Any()
                .Should()
                .BeTrue($"{t.Name} must have a TryParse method");
            }
        }

        [TestMethod]
        public void All_TryParse_Functions_Must_Succeed_When_Passing_A_Null_String()
        {
            var typesToCheck =
                Structs
                .Where(HasStringParameterConstructor);


            foreach (var t in typesToCheck)
            {
                var tryParseMethod =
                    t
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Where(m => IsTryParseMethod(m, typeof(string)))
                    .FirstOrDefault();
                if (tryParseMethod == null) continue;


                var result = tryParseMethod.Invoke(null, new object[] { null, null });
                result.Should().Be(true, $"{t.Name}.TryParse(null) should return true");
            }

        }

        private static bool IsTryParseMethod(MethodInfo method, params Type[] typesExceptOutParameter)
        {
            if (method.Name != "TryParse") return false;
            if (method.ReturnType != typeof(bool)) return false;

            var parameters = method.GetParameters();

            if (parameters.Length != typesExceptOutParameter.Length + 1) return false;
            if (!parameters[typesExceptOutParameter.Length].IsOut) return false;

            for (var index = 0; index < typesExceptOutParameter.Length; index++)
            {
                if (parameters[index].ParameterType != typesExceptOutParameter[index]) return false;
            }

            return true;
        }

        private static bool HasStringParameterConstructor(Type t)
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
