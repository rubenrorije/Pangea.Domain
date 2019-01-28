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


    }
}
