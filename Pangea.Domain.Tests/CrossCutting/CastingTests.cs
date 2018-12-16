﻿using FluentAssertions;
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
    public class CastingTests
    {

        [TestMethod]
        public void When_A_Constructor_With_A_Single_String_Argument_Then_Must_Have_A_String_Cast_Operator()
        {
            var structs =
                typeof(CreditCard)
                .Assembly
                .Types()
                .ThatAreStructs()
                .ThatHaveAConstructorWithArguments<string>();

            using (new AssertionScope())
            {
                foreach (var type in structs)
                {
                    var castingOperators =
                        type
                        .GetMethods()
                        .Where(m => m.Name == "op_Explicit")
                        .Where(m => m.ReturnType == typeof(string))
                        .ToList();

                    castingOperators.Should().HaveCount(1, $"expected a string cast operator for {type.Name}");
                }
            }
        }
    }
}
