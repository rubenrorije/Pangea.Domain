using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class OperatorTests
    {
        public static IEnumerable<Type> DomainClasses =>
            typeof(FileSize)
            .Assembly
            .GetTypes()
            .Where(t => t.IsValueType)
            .Where(t => t.BaseType != typeof(Enum))
            .ToList();

        [TestMethod]
        public void Struct_Implements_Equals_Operator()
        {
            foreach (var t in DomainClasses)
            {
                FindOperator(t, "op_Equality").Should().NotBeNull($"a struct should implement the == operator for {t.Name}");
            }

        }
        [TestMethod]
        public void Struct_Implements_NotEquals_Operator()
        {
            foreach (var t in DomainClasses)
            {
                FindOperator(t, "op_Inequality").Should().NotBeNull($"a struct should implement the != operator for {t.Name}");
            }
        }

        [TestMethod]
        public void When_Struct_Implements_GreaterThan_Must_Also_Implement_IComparable()
        {
            foreach (var t in DomainClasses)
            {
                var operators = GetOperators(t);
                var greaterThanOperators = operators.Where(o => o.Name.Equals("op_GreaterThan"));

                var parameterTypes =
                    greaterThanOperators
                    .SelectMany(o => o.GetParameters())
                    .Select(p => p.ParameterType)
                    .Distinct()
                    .Where(p => p != t)
                    .ToList();
                foreach (var p in parameterTypes)
                {
                    // parameter types are all types that have a greater than implementation
                    // e.g >(Percentage lhs, int rhs)
                    // if that is the case, they must also implement IComparable
                    var hasInterface =
                        t.GetInterfaces()
                        .Where(i => i.Name == "IComparable`1")
                        .Where(i => i.GetGenericArguments().Count() == 1)
                        .Where(i => i.GetGenericArguments()[0] == p)
                        .Any();

                    hasInterface.Should().BeTrue($"struct {t.Name} should implement IComparable<{p.Name}> because it has an operator > for {p.Name}");
                }

            }
        }

        [TestMethod]
        public void When_Struct_Implements_IComparable_It_Must_IComparable_Generic_And_Vice_Versa()
        {
            foreach (var t in DomainClasses)
            {
                var hasGeneric = t.GetInterface("IComparable") != null;
                var HasSpecific = t.GetInterfaces().Where(i => i.Name == "IComparable`1").Any(i => i.GetGenericArguments()[0] == t);

                (hasGeneric == HasSpecific).Should().BeTrue($"When struct {t.Name} implements IComparable it must also implement IComparable<{t.Name}>");
            }

        }

        [TestMethod]
        public void A_Struct_That_Implements_GreaterThan_With_A_Different_Type_Must_Implement_It_Both_Ways()
        {
            A_Struct_That_Implement_An_Operator_Must_Implement_It_Both_Ways("op_GreaterThan");
        }

        [TestMethod]
        public void A_Struct_That_Implements_GreaterThanOrEqual_With_A_Different_Type_Must_Implement_It_Both_Ways()
        {
            A_Struct_That_Implement_An_Operator_Must_Implement_It_Both_Ways("op_GreaterThanOrEqual");
        }

        [TestMethod]
        public void A_Struct_That_Implements_LessThan_With_A_Different_Type_Must_Implement_It_Both_Ways()
        {
            A_Struct_That_Implement_An_Operator_Must_Implement_It_Both_Ways("op_LessThan");
        }

        [TestMethod]
        public void A_Struct_That_Implements_LessThanOrEqual_With_A_Different_Type_Must_Implement_It_Both_Ways()
        {
            A_Struct_That_Implement_An_Operator_Must_Implement_It_Both_Ways("op_LessThanOrEqual");
        }

        private void A_Struct_That_Implement_An_Operator_Must_Implement_It_Both_Ways(string operatorFunctionName)
        {
            foreach (var t in DomainClasses)
            {
                var operators = GetOperators(t);
                var allOperators = operators.Where(o => o.Name.Equals(operatorFunctionName));

                var parameterTypes =
                    allOperators
                    .SelectMany(o => o.GetParameters())
                    .Select(p => p.ParameterType)
                    .Distinct()
                    .Where(p => p != t)
                    .ToList();

                foreach (var p in parameterTypes)
                {
                    allOperators.Any(o => o.GetParameters()[0].ParameterType == t && o.GetParameters()[1].ParameterType == p).Should().BeTrue($"when a struct implements {operatorFunctionName} for {p.Name} it must implement it both ways");
                }
            }
        }

        private static IEnumerable<MethodBase> GetOperators(Type type)
        {
            return
                 type
                 .GetMethods(BindingFlags.Public | BindingFlags.Static)
                 .Where(m => m.Attributes.HasFlag(MethodAttributes.HideBySig))
                 .Where(m => m.Attributes.HasFlag(MethodAttributes.SpecialName))
                 .Where(m => m.ReturnType == typeof(Boolean))
                 .Where(m => m.GetParameters().Count() == 2);
        }


        private static MethodBase FindOperator(Type type, string operatorFunctionName)
        {
            return
                GetOperators(type)
                .Where(m => m.GetParameters().ElementAt(0).ParameterType == type)
                .Where(m => m.GetParameters().ElementAt(1).ParameterType == type)
                .Where(m => m.Name == operatorFunctionName)
                .SingleOrDefault();

        }
    }
}
