using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pangea.Domain.Tests.Util;
using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class AssemblyInformationTests
    {
        [TestMethod]
        public void Create_AssemblyInformation_For_Calling_Assembly()
        {
            var sut = AssemblyInformation.ForCallingAssembly();
            sut.FullName.Should().Contain("Pangea.Domain.Tests,");
        }

        [TestMethod]
        public void Create_AssemblyInformation_For_Entry_Assembly()
        {
            // test runner does not have an entry assembly
            Action action = () => AssemblyInformation.ForEntryAssembly();

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Create_AssemblyInformation_For_Executing_Assembly()
        {
            var sut = AssemblyInformation.ForExecutingAssembly();
            sut.FullName.Should().Contain("Pangea.Domain,");
        }

        [TestMethod]
        public void Create_AssemblyInformation_For_A_Type_That_Is_Null_Throws_ArgumentNullException()
        {
            Action action = () => AssemblyInformation.ForAssemblyOf(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Create_AssemblyInformation_For_Assembly_Of_Given_Type()
        {
            var sut = AssemblyInformation.ForAssemblyOf(typeof(CreditCard));
            sut.FullName.Should().Contain("Pangea.Domain,");
        }

        [TestMethod]
        public void Create_AssemblyInformat_For_Assembly_Of_Type_Generic()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.FullName.Should().Contain("Pangea.Domain,");
        }

        [TestMethod]
        public void Get_Description_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.Description.Should().BeNull();
        }

        [TestMethod]
        public void Get_Company_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.Company.Should().Be("RR Consulting");
        }

        [TestMethod]
        public void Get_Configuration_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.Configuration.Should().Be("Debug");
        }

        [TestMethod]
        public void Get_Copyright_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.Copyright.Should().Be("Copyright © 2018");
        }

        [TestMethod]
        public void Get_Culture_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.Culture.Should().BeNull();
        }

        [TestMethod]
        public void Get_DefaultAlias_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.DefaultAlias.Should().BeNull();
        }

        [TestMethod]
        public void Get_DelaySign_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.DelaySign.Should().BeFalse();
        }

        [TestMethod]
        public void Get_FileVersion_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.FileVersion.Should().Be("1.0.0.0");
        }

        [TestMethod]
        public void Get_FileVersionInfo_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.FileVersionInfo.Should().Be(new Version(1, 0, 0, 0));
        }

        [TestMethod]
        public void Get_Flags_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);

            sut.Flags.Should().Be(AssemblyNameFlags.None);

            sut.EnableJITcompileOptimizer.Should().BeFalse();
            sut.EnableJITcompileTracking.Should().BeFalse();
            sut.HasPublicKey.Should().BeFalse();
            sut.Retargetable.Should().BeFalse();
        }

        [TestMethod]
        public void Get_InformationalVersion_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.InformationalVersion.Should().Be("1.0.0");
        }

        [TestMethod]
        public void Get_KeyFile_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.KeyFile.Should().BeNull();
        }


        [TestMethod]
        public void Get_KeyName_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.KeyName.Should().BeNull();
        }

        [TestMethod]
        public void Get_MetaData_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.MetaData.Should().HaveCount(0);
        }

        [TestMethod]
        public void Get_MetaData_Value_By_Key_That_Does_Not_Exist_Throws_KeyException()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            Action action = () => { var x = sut["abc"]; };
            action.Should().Throw<KeyNotFoundException>();
        }

        [TestMethod]
        public void Get_Product_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.Product.Should().Be("Pangea.Domain");
        }

        [TestMethod]
        public void Get_PublicKey_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.PublicKey.Should().BeNull();
        }

        [TestMethod]
        public void Get_Countersignature_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.Countersignature.Should().BeNull();
        }

        [TestMethod]
        public void Get_Title_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.Title.Should().Be("Pangea.Domain");
        }

        [TestMethod]
        public void Get_Trademark_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.Trademark.Should().BeNull();
        }
        
        [TestMethod]
        public void Get_AssemblyVersionInfo_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.AssemblyVersionInfo.Should().Be(new Version(1,0,0,0));
        }

        [TestMethod]
        public void Get_TargetFrameworkName_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.TargetFrameworkName.Should().Contain(".NETStandard");
        }

        [TestMethod]
        public void Get_TargetFrameworkDisplayName_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.TargetFrameworkDisplayName.Should().BeEmpty();
        }

        [TestMethod]
        public void Get_AlgorithmId_Of_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.AlgorithmId.Should().Be(AssemblyHashAlgorithm.None);
        }

        [TestMethod]
        public void ToString_Returns_The_FullName_Of_The_Assembly()
        {
            var sut = new AssemblyInformation(AssemblyUnderTest.Instance);
            sut.ToString().Should().Be(sut.FullName);
        }
    }
}
