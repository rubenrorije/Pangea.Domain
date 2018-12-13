using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Pangea.Domain.Tests.Util
{
    public static class HasReadEndElement
    {
        public static AndConstraint<ObjectAssertions> BeXmlSerializableAndLeaveReaderInCorrectWayWhenFinished
            (this ObjectAssertions assertions, string because = "", params object[] becauseArgs)
        {

            var subject = assertions.Subject as IXmlSerializable;

            if (subject == null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith($"Expected {subject} to implement {nameof(IXmlSerializable)}");
            }
            var clone = (IXmlSerializable)Activator.CreateInstance(subject.GetType());

            var builder = new StringBuilder();
            using (var writer = XmlWriter.Create(builder))
            {
                writer.WriteStartElement(subject.GetType().Name);
                subject.WriteXml(writer);
                writer.WriteEndElement();
            }
            using (var sr = new StringReader(builder.ToString()))
            {
                using (var reader = XmlReader.Create(sr))
                {
                    clone.ReadXml(reader);

                    if (reader.NodeType != XmlNodeType.None)
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith($"Expected {subject} ReadXml method to correctly close the xml document after reading, the current node is {reader.NodeType}");
                    }
                }
            }

            return new AndConstraint<ObjectAssertions>(assertions).And.BeXmlSerializable();
        }


    }
}
