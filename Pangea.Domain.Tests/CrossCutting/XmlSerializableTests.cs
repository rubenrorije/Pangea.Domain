using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void Serialize_And_Deserialize_Does_Not_Throw_An_Exception()
        {

            foreach (var t in DomainClasses)
            {
                if (t == typeof(DateRange)) continue;
                var sut = (IXmlSerializable)Activator.CreateInstance(t);
                var text = new StringBuilder();
                using (var writer = XmlWriter.Create(text, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    sut.WriteXml(writer);
                }

                using (var tr = new StringReader(text.ToString()))
                using (var reader = XmlReader.Create(tr))
                {
                    sut.ReadXml(reader);
                }
            }
        }
    }
}
