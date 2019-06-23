using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class IsbnTests
    {
        [TestMethod]
        public void Creating_A_Null_Isbn_Is_Allowed()
        {
            Action action = () => new Isbn(null);
            action.Should().NotThrow();
        }

        [TestMethod]
        public void Creating_An_Empty_Isbn_Is_Allowed()
        {
            Action action = () => new Isbn(string.Empty);
            action.Should().NotThrow();
        }

        [TestMethod]
        [DataRow(null, true)]
        [DataRow("1", false)]
        [DataRow("12", false)]
        [DataRow("123", false)]
        [DataRow("1234", false)]
        [DataRow("12345", false)]
        [DataRow("123456", false)]
        [DataRow("1234567", false)]
        [DataRow("12345678", false)]
        [DataRow("123456789", false)]
        [DataRow("1234567890", true)]
        [DataRow("12345678901", false)]
        [DataRow("123456789012", false)]
        [DataRow("1234567890123", true)]
        [DataRow("12345678901234", false)]
        public void Length_Is_Correct(string text, bool result)
        {
            Action action = () => new Isbn(text);
            if (result)
            {
                action.Should().NotThrow(text);
            }
            else
            {
                action.Should().Throw<ArgumentOutOfRangeException>(text);
            }
        }

        [TestMethod]
        [DataRow("9780123456789", "0")]
        [DataRow("9781123456789", "1")]
        [DataRow("9782123456789", "2")]
        [DataRow("9783123456789", "3")]
        [DataRow("9784123456789", "4")]
        [DataRow("9785123456789", "5")]
        [DataRow("9786123456789", "612")]
        [DataRow("9787123456789", "7")]
        [DataRow("9788123456789", "81")]
        [DataRow("9789123456789", "91")]
        [DataRow("9789623456789", "962")]
        [DataRow("9789923456789", "9923")]
        [DataRow("9789993456789", "99934")]
        public void Identifier_Group_Correctly_Extracted(string isbn, string identifierGroup)
        {
            var sut = new Isbn(isbn);
            sut.IdentifierGroup.Should().Be(identifierGroup);
        }

        [TestMethod]
        public void Hyphens_Are_Allowed()
        {
            Action action = () => new Isbn("978-0-123456789");
            action.Should().NotThrow();
        }

        [TestMethod]
        public void Only_Digits_And_Hyphens_Are_Allowed()
        {
            Action action = () => new Isbn("978-0-12345678a");
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ToString_Of_Null_Isbn_Is_Allowed()
        {
            new Isbn(null).ToString().Should().BeNull();
        }

        [TestMethod]
        public void Prefix_Of_A_10_Digit_Isbn_Is_Null()
        {
            var sut = new Isbn("0-123456789");
            sut.Prefix.Should().BeNull();
        }

        [TestMethod]
        public void Prefix_Of_A_13_Digit_Isbn_Is_Defined()
        {
            var sut = new Isbn("978-0-123456789");
            sut.Prefix.Should().Be("978");
        }


        [TestMethod]
        public void The_Hyphens_In_An_Isbn_Do_Not_Affect_Equality()
        {
            var one = new Isbn("978-0-123456789");
            var other = new Isbn("9780123456789");

            one.Equals(other).Should().BeTrue();
            (one == other).Should().BeTrue();
        }
        [TestMethod]
        public void The_Hyphens_In_An_Isbn_Do_Not_Affect_Hashcode()
        {
            var one = new Isbn("978-0-123456789");
            var other = new Isbn("9780123456789");

            one.GetHashCode().Equals(other.GetHashCode()).Should().BeTrue();
        }

        [TestMethod]
        public void ToString_Bare_Returns_Without_Hyphens()
        {
            var sut = new Isbn("978-0-123456789");
            sut.ToString("B").Should().Be("9780123456789");
        }

        [TestMethod]
        public void ToString_With_Original_Format()
        {
            var sut = new Isbn("978-0-123456789");
            sut.ToString("O").Should().Be("978-0-123456789");
            sut.ToString("G").Should().Be("978-0-123456789");
            sut.ToString("").Should().Be("978-0-123456789");
            sut.ToString((string)null).Should().Be("978-0-123456789");
        }

        [TestMethod]
        public void ToString_With_Invalid_Format_Throws_FormatException()
        {
            var sut = new Isbn("978-0-123456789");
            Action action = () => sut.ToString("X");

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void ToString_With_Invalid_Format_By_Case_Throws_FormatException()
        {
            var sut = new Isbn("978-0-123456789");
            Action action = () => sut.ToString("o");

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void Creating_An_Unsafe_ISBN_Does_Not_Throw_Exception_On_Invalid_ISBN()
        {
            Action action = () => Isbn.Unsafe("978B0X123456789");

            action.Should().NotThrow<ArgumentOutOfRangeException>();
        }
    }
}
