using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class EmojiFlagTests
    {
        [TestMethod]
        public void Null_Flag_Can_Be_Created()
        {
            EmojiFlag.Create((string)null).Should().BeNull();
        }

        [TestMethod]
        public void When_Country_Is_Less_Than_Two_Characters_Throw_FormatException()
        {
            Action action = () => EmojiFlag.Create("a");
            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void When_Country_Is_More_Than_Two_Characters_Throw_FormatException()
        {
            Action action = () => EmojiFlag.Create("abc");
            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void Case_Of_The_Country_Name_Does_Not_Matter()
        {
            var one = EmojiFlag.Create("nl");
            var other = EmojiFlag.Create("NL");

            one.Should().Be(other);
        }

        [TestMethod]
        public void Print_The_Emoji_Flag_For_The_Netherlands()
        {
            var sut = EmojiFlag.Create("NL");

            sut.ToString().Should().Be("🇳🇱");
        }

        [TestMethod]
        public void Create_Emoji_Flag_From_RegionInfo()
        {
            var sut = EmojiFlag.Create(new RegionInfo(1043));
            sut.ToString().Should().Be("🇳🇱");
        }

        [TestMethod]
        public void Create_Emoji_Flag_From_CultureInfo()
        {
            var sut = EmojiFlag.Create(new CultureInfo(1043));
            sut.ToString().Should().Be("🇳🇱");
        }

        [TestMethod]
        public void Create_Emoji_Flag_From_LCID()
        {
            var sut = EmojiFlag.Create(1043);
            sut.ToString().Should().Be("🇳🇱");
        }
    }
}
