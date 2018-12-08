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
    public class AssemblyTests
    {
        [TestMethod]
        public void The_Assemblies_That_Are_Loaded_Must_Have_The_Same_Assembly_Version_And_File_Version()
        {
            AppDomain.CurrentDomain.GetAssemblies().Should().HaveCountGreaterThan(1);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var info = new AssemblyInformation(assembly);

                    // only check the current assemblies that are tested.
                    if (info.Configuration == "Debug")
                    {
                        info.AssemblyVersionInfo.Should().Be(info.FileVersionInfo, $"the File Version and the Assembly Version must be the same for assembly {info.FullName}");
                    }
                }
                catch (NotSupportedException ex)
                {
                    Assert.Fail(assembly.FullName + ex.ToString());
                }
            }
        }
    }
}
