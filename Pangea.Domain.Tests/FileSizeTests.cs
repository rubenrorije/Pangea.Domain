using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class FileSizeTests
    {
        [TestMethod]
        public void Empty_FileSize_ToString_Is_0_Bytes()
        {
            var sut = new FileSize();

            sut.ToString().Should().Be("0B");
        }

        [TestMethod]
        public void Empty_FileSize_Has_0_Bytes()
        {
            var sut = new FileSize();

            sut.TotalBytes.Should().Be(0);
        }

        [TestMethod]
        public void TotalBytes_Has_The_Number_Of_Bytes()
        {
            var sut = new FileSize(1);
            sut.TotalBytes.Should().Be(1);
        }

        [TestMethod]
        public void ToString_In_Bytes_Is_Equal_To_The_Number_In_Bytes()
        {
            var sut = new FileSize(123);
            sut.ToString().Should().Be("123B");
        }

        [TestMethod]
        public void Bytes_Are_Converted_To_KiloBytes()
        {
            var sut = new FileSize(1024);
            sut.TotalKiloBytes.Should().Be(1.0);
        }

        [TestMethod]
        public void More_Than_1KB_ToString_Defaults_To_KB()
        {
            var sut = new FileSize(1024);
            sut.ToString().Should().Be("1KB");
        }

        [TestMethod]
        public void Bytes_Are_Converted_To_MegaBytes()
        {
            var sut = new FileSize(1024 * 1024);
            sut.TotalMegaBytes.Should().Be(1.0);
        }

        [TestMethod]
        public void More_Than_1MB_ToString_Defaults_To_MB()
        {
            // 1 MB
            var sut = new FileSize(1024 * 1024);
            sut.ToString().Should().Be("1MB");
        }

        [TestMethod]
        public void Bytes_Are_Converted_To_GigaBytes()
        {
            // 1 GB
            var sut = new FileSize(1024 * 1024 * 1024);
            sut.TotalGigaBytes.Should().Be(1.0);
        }

        [TestMethod]
        public void More_Than_1GB_ToString_Defaults_To_GB()
        {
            // 1 GB
            var sut = new FileSize(1024 * 1024 * 1024);
            sut.ToString().Should().Be("1GB");
        }

        [TestMethod]
        public void Bytes_Are_Converted_To_TeraBytes()
        {
            // 1 TB
            var sut = new FileSize(1024L * 1024 * 1024 * 1024);
            sut.TotalTeraBytes.Should().Be(1.0);
        }

        [TestMethod]
        public void More_Than_1TB_ToString_Defaults_To_TB()
        {
            // 1 TB
            var sut = new FileSize(1024L * 1024 * 1024 * 1024);
            sut.ToString().Should().Be("1TB");
        }

        [TestMethod]
        public void Create_Based_On_Stream_That_Is_Null()
        {
            var sut = new FileSize((Stream)null);
            sut.TotalBytes.Should().Be(0);
        }

        [TestMethod]
        public void Create_Based_On_A_Stream_That_Uses_The_Length()
        {
            using (var stream = new MemoryStream(new byte[5]))
            {
                var sut = new FileSize(stream);
                sut.TotalBytes.Should().Be(5);
            }
        }

        [TestMethod]
        public void HashCode_Is_Equal_For_Same_Size()
        {
            var sut = new FileSize(5);

            sut.GetHashCode().Should().Be(new FileSize(5).GetHashCode());
        }

        [TestMethod]
        public void Equals_Is_The_Same_When_The_Size_Is_The_Same()
        {
            var sut = new FileSize(5);

            sut.Should().Be(new FileSize(5));
        }

        [TestMethod]
        public void CompareTo_Returns_0_When_Equal()
        {
            var sut = new FileSize(5);

            sut.CompareTo(new FileSize(5)).Should().Be(0);
        }

        [TestMethod]
        public void CompareTo_Returns_1_When_Greater()
        {
            var sut = new FileSize(5);

            sut.CompareTo(new FileSize(4)).Should().Be(1);
        }

        [TestMethod]
        public void CompareTo_Returns_Minus_1_When_Less()
        {
            var sut = new FileSize(5);

            sut.CompareTo(new FileSize(6)).Should().Be(-1);
        }

        [TestMethod]
        public void CompareTo_Can_Compare_With_Long()
        {
            var sut = new FileSize(5);

            sut.CompareTo(6).Should().Be(-1);
            sut.CompareTo(4).Should().Be(1);

        }

        [TestMethod]
        public void Operator_Equals_Should_Work()
        {
            var sut = new FileSize(42);
            (sut == new FileSize(42)).Should().BeTrue();
            (sut == new FileSize(43)).Should().BeFalse();
        }

        [TestMethod]
        public void Operator_NotEquals_Should_Work()
        {
            var sut = new FileSize(42);
            (sut != new FileSize(42)).Should().BeFalse();
            (sut != new FileSize(43)).Should().BeTrue();
        }

        [TestMethod]
        public void Equals_Null_Is_False()
        {
            var sut = new FileSize(123);
            sut.Equals(null).Should().BeFalse();
        }

        [TestMethod]
        public void Equals_For_A_DifferentType_Is_False()
        {
            var sut = new FileSize(123);
            sut.Equals("AB").Should().BeFalse();
        }

        [TestMethod]
        public void Operators_Are_Comparing_FileSizes()
        {
            var less = new FileSize(5);
            var more = new FileSize(6);

            (less > more).Should().BeFalse();
            (more > less).Should().BeTrue();

            (less >= more).Should().BeFalse();
            (more >= less).Should().BeTrue();


            (less < more).Should().BeTrue();
            (more < less).Should().BeFalse();

            (less <= more).Should().BeTrue();
            (more <= less).Should().BeFalse();

        }

        [TestMethod]
        public void ToString_Uses_NumberFormat()
        {
            var sut = new FileSize(1234);
            var result = sut.ToString("N2", CultureInfo.InvariantCulture);

            result.Should().Be("1.21KB");
        }

        [TestMethod]
        public void ToString_Should_Implement_G_Format()
        {
            var sut = new FileSize(123);

            sut.ToString("G", null).Should().Be(sut.ToString());
        }

        [TestMethod]
        public void Serialize_FileSize()
        {
            var sut = new FileSize(1234);
            var text = new StringBuilder();
            using (var writer = XmlWriter.Create(text))
            {
                sut.WriteXml(writer);
            }

            text.ToString().Should().Contain("<value>1234</value>");
        }

        [TestMethod]
        public void Deserialize_FileSize()
        {
            using (var tr = new StringReader("<value>42</value>"))
            using (var reader = XmlReader.Create(tr))
            {
                var sut = new FileSize();
                sut.ReadXml(reader);

                sut.TotalBytes.Should().Be(42);
            }
        }

    }
}
