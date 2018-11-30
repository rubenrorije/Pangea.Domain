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
    public class FormattableTests
    {
        public static IEnumerable<Type> DomainClasses =>
            typeof(FileSize)
            .Assembly
            .GetTypes()
            .Where(t => t.GetInterface(nameof(IFormattable)) != null)
            .ToList();



        [TestMethod]
        public void When_Implementing_IFormattable_It_Has_An_Overload_Without_The_FormatProvider()
        {
            var invalid =
                DomainClasses
                .Where(t => t.GetMethod("ToString", new[] { typeof(string) }) == null)
                .ToList();

            invalid.Should().HaveCount(0, $"{string.Join(", ", invalid.Select(t => t.Name))} should implement a ToString(string format) overload (no IFormatProvider)");

        }


        [TestMethod]
        public void When_Implementing_IFormattable_The_G_Format_Is_The_Same_As_The_Null_Format()
        {
            var invalid =
                DomainClasses
                .Where(t => t.GetConstructors().Any(c => c.GetParameters().Count() == 0))
                .Select(t => Activator.CreateInstance(t))
                .Cast<IFormattable>()
                .Select(o =>
                    new
                    {
                        Type = o.GetType(),
                        G = o.ToString("G", null),
                        D = o.ToString(null, null)
                    })
                .Where(o => o.G != o.D)
                .ToList();

            invalid.Should().HaveCount(0, $"{string.Join(", ", invalid.Select(o => o.Type))} should have the same result for the \"G\" and the <null> format");
        }

    }
}
