using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Pangea.Domain.Tests
{
    public static class XmlHelper
    {
        public static string Serialize(this IXmlSerializable value)
        {
            var text = new StringBuilder();
            using (var writer = XmlWriter.Create(text, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                value.WriteXml(writer);
            }
            return text.ToString();
        }

        public static T Deserialize<T>(string xml) where T : IXmlSerializable, new()
        {
            var result = new T();
            using (var tr = new StringReader(xml))
            using (var reader = XmlReader.Create(tr))
            {
                result.ReadXml(reader);
            }
            return result;
        }

        public static T RoundTrip<T>(this T value) where T : IXmlSerializable, new()
        {
            return Deserialize<T>(value.Serialize());
        }

    }
}
