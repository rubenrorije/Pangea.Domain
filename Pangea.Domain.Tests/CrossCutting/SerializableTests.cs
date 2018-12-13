using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Types;
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
    public class SerializableTests
    {
        private static TypeSelector Structs
        {
            get
            {
                return
                   typeof(CreditCard)
                   .Assembly
                   .Types()
                   .ThatAreStructs();
            }
        }


        [TestMethod]
        public void Structs_Must_Be_Binary_Serializable()
        {
            using (new AssertionScope())
            {
                foreach (var type in Structs)
                {
                    var @object = Activator.CreateInstance(type);
                    @object.Should().BeBinarySerializable($"{type.Name} should be binary serializable");
                }
            }
        }

        [TestMethod]
        public void Structs_Must_Be_Xml_Serializable()
        {
            using (new AssertionScope())
            {
                foreach (var type in Structs)
                {
                    var @object = Activator.CreateInstance(type);
                    @object.Should().BeXmlSerializable($"{type.Name} should be xml serializable");
                }
            }
        }

        [TestMethod]
        public void Structs_Must_Be_DataContract_Serializable()
        {
            using (new AssertionScope())
            {
                foreach (var type in Structs)
                {
                    var @object = Activator.CreateInstance(type);
                    @object.Should().BeDataContractSerializable($"{type.Name} should be data contract serializable");
                }
            }
        }
    }
}
