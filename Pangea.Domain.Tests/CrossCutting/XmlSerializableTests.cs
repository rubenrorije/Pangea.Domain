using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Pangea.Domain.Tests.CrossCutting
{
    [TestClass]
    public class XmlSerializableTests
    {
        public static IEnumerable<Type> DomainClasses =>
            typeof(FileSize)
            .Assembly
            .GetTypes()
            .Where(t => t.GetInterface(nameof(IXmlSerializable)) != null)
            .ToList();

        [TestMethod]
        public void Throws_When_XmlReader_Is_Null()
        {
            foreach (var t in DomainClasses)
            {
                var sut = (IXmlSerializable)Activator.CreateInstance(t);
                Action action = () => sut.ReadXml(null);
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public void GetSchema_Returns_Null()
        {
            foreach (var t in DomainClasses)
            {
                var sut = (IXmlSerializable)Activator.CreateInstance(t);
                sut.GetSchema().Should().BeNull();
            }
        }

        [TestMethod]
        public void Throws_When_XmlWriter_Is_Null()
        {
            foreach (var t in DomainClasses)
            {
                var sut = (IXmlSerializable)Activator.CreateInstance(t);
                Action action = () => sut.WriteXml(null);
                action.Should().Throw<ArgumentNullException>();
            }
        }


        [TestMethod]
        public void Must_Implement_IXmlSerializable_Explicit()
        {
            using (new AssertionScope())
            {
                foreach (var type in DomainClasses)
                {
                    type.ImplementsExplicitly<IXmlSerializable>().Should().BeTrue($"IXmlSerializable should be implemented explicitly for {type.Name}");
                }
            }
        }
        
    }
}
