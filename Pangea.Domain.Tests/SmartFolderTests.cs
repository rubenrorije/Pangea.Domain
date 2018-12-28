using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class SmartFolderTests
    {
        private static readonly string _invalidPathString = System.IO.Path.GetInvalidPathChars().First().ToString();

        [TestMethod]
        public void Null_Path_Throws_Exception()
        {
            Action action = () => new SmartFolder((string)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Invalid_Path_Throws_Exception()
        {
            Action action = () => new SmartFolder(_invalidPathString);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Path_Must_Be_Valid()
        {
            var sut = new SmartFolder(@"C:\Temp\");
            sut.ToString().Should().Be(@"C:\Temp\");
        }

        [TestMethod]
        public void Add_Sub_Path_By_Using_Add_Operator_With_SmartPath()
        {
            var sut = new SmartFolder(@"C:\");
            sut += new SmartFolder("temp");
            sut.ToString().Should().Be(@"C:\temp\");
        }

        [TestMethod]
        public void Add_Sub_Path_By_Using_Add_Operator_With_String()
        {
            var sut = new SmartFolder(@"C:\");
            sut += "temp";
            sut.ToString().Should().Be(@"C:\temp\");
        }

        [TestMethod]
        public void Volume_When_Letter_Null_Throws_Exception()
        {
            Action action = () => SmartFolder.Volume(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Volume_When_Letter_Empty_Throws_Exception()
        {
            Action action = () => SmartFolder.Volume(string.Empty);
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Easy_Creation_Of_Path()
        {
            var sut = SmartFolder.Volume("C") + "Windows" + "Temp";
            sut.ToString().Should().Be(@"C:\Windows\Temp\");
        }

        [TestMethod]
        public void Expand_EnvironmentVariable()
        {
            var sut = new SmartFolder("%Temp%");
            sut.ToString().Should().Be(ExpandEnvironmentVariables("%Temp%") + @"\");
        }

        [TestMethod]
        public void ToString_With_Null_Format_Expands_Environment_Variable()
        {
            var sut = new SmartFolder("%Temp%");
            sut.ToString(null).Should().Be(ExpandEnvironmentVariables("%Temp%") + @"\");
        }

        [TestMethod]
        public void ToString_With_Empty_Format_Expands_Environment_Variable()
        {
            var sut = new SmartFolder("%Temp%");
            sut.ToString("").Should().Be(ExpandEnvironmentVariables("%Temp%") + @"\");
        }

        [TestMethod]
        public void ToString_With_G_Format_Expands_Environment_Variable()
        {
            var sut = new SmartFolder("%Temp%");
            sut.ToString("G").Should().Be(ExpandEnvironmentVariables("%Temp%") + @"\");
        }

        [TestMethod]
        public void ToString_With_O_Format_Returns_The_Original_Path()
        {
            var sut = new SmartFolder("%Temp%");
            sut.ToString("O").Should().Be("%Temp%");
        }

        [TestMethod]
        public void ToString_With_E_Format_Returns_The_Escaped_Path()
        {
            var sut = new SmartFolder(@"C:\Users\John Doe\");
            sut.ToString("E").Should().Be("\"C:\\Users\\John Doe\\\"");
        }

        [TestMethod]
        public void ToString_With_R_Format_Returns_The_Relative_Path()
        {
            var sut = new SmartFolder("..");
            sut.ToString("R").Should().Be("..");
            sut.ToString("G").Should().NotBe("..");
        }

        [TestMethod]
        public void ToString_With_L_Format_Returns_The_Path_Separated_By_Slashes()
        {
            var sut = new SmartFolder(@"C:\Users\John Doe\");
            sut.ToString("L").Should().Be("C:/Users/John Doe/");
        }

        [TestMethod]
        public void Create_Using_Environment_Variable_Throws_Exception_When_Null()
        {
            Action action = () => SmartFolder.FromEnvironmentVariable(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Create_Using_Environment_Variable()
        {
            var sut = SmartFolder.FromEnvironmentVariable("TEMP");
            sut.ToString("O").Should().Be("%TEMP%");
            sut.ToString("G").Should().Be(ExpandEnvironmentVariables("%TEMP%") + @"\");
        }

        [TestMethod]
        public void SpecialFolder_That_Exists_Is_Resolved()
        {
            var sut = new SmartFolder(SpecialFolder.Desktop);
            sut.ToString().Should().Contain(@"\Desktop\");
        }

        [TestMethod]
        public void Operator_MinMin_Moves_One_Directory_Up()
        {
            var sut = new SmartFolder(@"C:\Windows\Temp\");
            sut--;
            sut.ToString().Should().Be(@"C:\Windows\");
        }

        [TestMethod]
        public void TryParse_Returns_False_When_Path_Contains_Invalid_Characters()
        {
            SmartFolder.TryParse(_invalidPathString, out var _).Should().BeFalse();
        }

        [TestMethod]
        public void RelativePath_Is_Resolved_To_AbsolutePath()
        {
            var sut = new SmartFolder(".");
            sut.ToString().Should().Be(Environment.CurrentDirectory + @"\");
        }

        [TestMethod]
        public void CurrentDirectory()
        {
            var sut = SmartFolder.CurrentDirectory;
            sut.ToString("O").Should().Be(".");
        }

        [TestMethod]
        public void Equality_For_Relative_And_Absolute_Paths()
        {
            var lhs = new SmartFolder(".");
            var rhs = new SmartFolder(Environment.CurrentDirectory);

            lhs.Equals(rhs).Should().BeTrue();
        }

        [TestMethod]
        public void Absolute_Returns_The_Folder_With_An_Absolute_Path()
        {
            var lhs = SmartFolder.CurrentDirectory;
            var rhs = lhs.ToAbsolute();

            lhs.ToString("O").Should().NotBe(rhs.ToString("O"));
        }

        [TestMethod]
        public void Absolute_Returns_The_Folder_With_An_Absolute_Path_And_Expands_Variables()
        {
            var lhs = SmartFolder.FromEnvironmentVariable("temp");
            var rhs = lhs.ToAbsolute();

            rhs.ToString("O").Should().NotContain("%", "environment variables must be expanded");
        }

        [TestMethod]
        public void A_Folder_With_And_Without_Trailing_Separator_Are_The_Same()
        {
            var lhs = new SmartFolder(@"C:\Windows\");
            var rhs = new SmartFolder(@"C:\Windows");

            lhs.Equals(rhs).Should().BeTrue();
        }

        [TestMethod]
        public void Equality_Case_Sensitive_Or_Not_Can_Be_Influenced()
        {
            var lhs = new SmartFolder(@"C:\Windows\");
            var rhs = new SmartFolder(@"c:\windows");

            lhs.Equals(rhs, StringComparison.CurrentCultureIgnoreCase).Should().BeTrue();
            lhs.Equals(rhs, StringComparison.CurrentCulture).Should().BeFalse();
        }

        [TestMethod]
        public void An_Environment_Variable_Is_Equal_To_The_Path_It_Resolves_To()
        {
            var lhs = SmartFolder.FromEnvironmentVariable("TEMP");
            var rhs = new SmartFolder(ExpandEnvironmentVariables("%TEMP%"));

            lhs.Equals(rhs).Should().BeTrue();
        }

        [TestMethod]
        public void Operator_Minus_Subtracts_Partial_Path_From_Path()
        {
            var lhs = new SmartFolder(@"C:\Windows\Temp\");
            var rhs = new SmartFolder("Temp");

            var sut = lhs - rhs;
            sut.ToString().Should().Be(@"C:\Windows\");
        }
        [TestMethod]
        public void Operator_Minus_Subtracts_Partial_Paths_From_Path()
        {
            var lhs = new SmartFolder(@"C:\Windows\Temp\");
            var rhs = new SmartFolder(@"Windows\Temp");

            var sut = lhs - rhs;
            sut.ToString().Should().Be(@"C:\");
        }

        [TestMethod]
        public void Subtract_Throws_Exception_When_Path_Is_Null()
        {
            Action action = () => { var _ = SmartFolder.CurrentDirectory.Subtract(null); };
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Subtract_Returns_The_Same_Path_When_Subtracting_An_Empty_String()
        {
            var sut = SmartFolder.CurrentDirectory.Subtract(string.Empty);
            sut.Should().Be(SmartFolder.CurrentDirectory);
        }

        [TestMethod]
        public void Operator_Minus_Throws_An_Exception_When_The_Path_To_Subtract_Is_Not_Relative()
        {
            var lhs = new SmartFolder(@"C:\Windows\Temp\");
            var rhs = new SmartFolder(@"C:\Windows\Temp\");

            Action action = () => { var x = lhs - rhs; };
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Operator_Minus_Throws_An_Exception_When_The_Path_To_Subtract_Do_Not_Match()
        {
            var lhs = new SmartFolder(@"C:\Windows\Temp\");
            var rhs = new SmartFolder(@"Program Files\Temp\");

            Action action = () => { var x = lhs - rhs; };
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Ignore_Leading_Directory_Separator_Char()
        {
            var windows = new SmartFolder(@"C:\Windows\");
            var one = windows + @"\Temp\";
            var other = windows + "Temp";

            one.Should().Be(other);
        }

        [TestMethod]
        public void Add_Throws_Exception_When_The_Addition_Is_Absolute()
        {
            var temp = new SmartFolder(@"C:\Temp\");
            Action action = () => { var _ = temp + "C:"; };
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Add_Throws_Exception_When_The_Addition_Is_Null()
        {
            var temp = new SmartFolder(@"C:\Temp\");
            Action action = () => { var _ = temp + null; };
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Add_Returns_The_Original_Path_When_The_Addition_Is_Empty()
        {
            var temp = new SmartFolder(@"C:\Temp\");
            var sut = temp + string.Empty;
            sut.Should().Be(temp);
        }

        [TestMethod]
        public void Is_Absolute_Is_False_For_Relative_Path()
        {
            var sut = new SmartFolder(".");
            sut.IsAbsolute.Should().BeFalse();
        }

        [TestMethod]
        public void Is_Absolute_Is_True_For_Absolute_Path()
        {
            var sut = new SmartFolder(Environment.CurrentDirectory);
            sut.IsAbsolute.Should().BeTrue();
        }


        [TestMethod]
        public void Create_From_DirectoryInfo_Null_Throws_ArgumentNullException()
        {
            Action action = () => new SmartFolder((DirectoryInfo)null);
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Create_From_DirectoryInfo()
        {
            var sut = new SmartFolder(new DirectoryInfo("."));
            sut.Should().Be(SmartFolder.CurrentDirectory);
        }

        [TestMethod]
        public void Create_From_DirectoryInfo_Relative()
        {
            var sut = new SmartFolder(new DirectoryInfo("."));
            sut.ToString("O").Should().Be(".");
        }

        [TestMethod]
        public void Create_From_DirectoryInfo_Relative_Parent()
        {
            var sut = new SmartFolder(new DirectoryInfo(".."));
            sut.ToString("O").Should().Be("..");
            sut.ToString("G").Should().Be(new DirectoryInfo(Environment.CurrentDirectory).Parent.FullName + @"\");
        }

        [TestMethod]
        public void Cast_To_DirectoryInfo()
        {
            var sut = SmartFolder.CurrentDirectory;
            var dir = (DirectoryInfo)sut;

            dir.FullName.Should().Be(Environment.CurrentDirectory);
        }

        [TestMethod]
        public void Cast_To_String()
        {
            var sut = SmartFolder.CurrentDirectory;

            ((string)sut).Should().Be(sut.ToString());
        }

        [TestMethod]
        public void Relative_Params_Smart_Folder()
        {
            SmartFolder.Relative("windows", "temp", "bar").ToString("O").Should().Be(@"windows\temp\bar");
        }

        [TestMethod]
        public void Absolute_Params_Smart_Folder()
        {
            SmartFolder.Absolute("C", "windows", "temp", "bar").ToString("O").Should().Be(@"C:\windows\temp\bar");
        }

        [TestMethod]
        public void Create_Folder_From_FileInfo()
        {
            var sut = SmartFolder.FromFile(new FileInfo(@"C:\temp\test.txt"));
            sut.Should().Be(new SmartFolder(@"C:\temp"));
        }
        
        [TestMethod]
        public void Equality_For_String()
        {
            var sut = new SmartFolder(@"C:\temp\");
            sut.Equals((object)@"C:\temp").Should().BeTrue();
            sut.Should().Be(@"C:\temp");
            sut.Should().Be(@"C:\temp\");
        }

        [TestMethod]
        public void Format_S_Smart_Escapes_The_Folder_Only_When_Necessary()
        {
            new SmartFolder(@"C:\Program Files").ToString("S").Should().Contain("\"");
            new SmartFolder(@"C:\ProgramFiles").ToString("S").Should().NotContain("\"");
        }

        [TestMethod]
        public void ToString_Throws_Format_Exception_When_Unknown_Format_Is_Used()
        {
            Action action = () => SmartFolder.CurrentDirectory.ToString("X");
            action.Should().Throw<FormatException>();
        }
    }
}
