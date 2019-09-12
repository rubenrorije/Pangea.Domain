using Pangea.Domain.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using static System.Environment;
using static System.IO.Path;
using Pangea.Domain.Properties;

namespace Pangea.Domain
{
    /// <summary>
    /// A way to represent paths to have easy ways to handle paths
    /// </summary>
    [Serializable]
    public struct SmartFolder
        : IEquatable<SmartFolder>
        , IEquatable<string>
        , IConvertible
        , IXmlSerializable
        , IFormattable
    {
        private string _value;

        private static StringComparison DefaultComparison
        {
            get
            {
                // lame check to see whether the OS is linux and therefore the file paths are
                // case sensitive
                if (DirectorySeparatorChar == '/') return StringComparison.CurrentCulture;
                else return StringComparison.CurrentCultureIgnoreCase;
            }
        }
        /// <summary>
        /// Is this a relative or an absolute folder path
        /// </summary>
        public bool IsAbsolute => IsPathRooted(_value);

        /// <summary>
        /// Create a smart path from a given path.
        /// </summary>
        /// <exception cref="ArgumentNullException">When the path is null</exception>
        public SmartFolder(string folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            if (folder.ContainsAny(GetInvalidPathChars())) throw new ArgumentOutOfRangeException(nameof(folder));

            _value = folder;
        }
        /// <summary>
        /// Create a smart folder from the given directory
        /// </summary>
        /// <param name="directory"></param>
        public SmartFolder(DirectoryInfo directory)
        {
            if (directory == null) throw new ArgumentNullException(nameof(directory));
            _value = directory.ToString();
        }

        /// <summary>
        /// Creates a path for the given special folder. When the 
        /// </summary>
        /// <param name="folder"></param>
        public SmartFolder(SpecialFolder folder)
        {
            var path = GetFolderPath(folder, SpecialFolderOption.None);
            _value = path;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _value?.GetHashCode() ?? 0;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is string) return Equals(new SmartFolder((string)obj), DefaultComparison);
            else return Equals((SmartFolder)obj, DefaultComparison);
        }

        /// <summary>
        /// Are the given folder paths equal. 
        /// Relative paths and absolute paths are compared using the current directory as a base
        /// Environment variables are expanded and then compared.
        /// By default the comparison is case sensitive.
        /// </summary>
        /// <param name="other">the folder to compare to</param>
        /// <returns>Whether the folder paths are equal</returns>
        public bool Equals(SmartFolder other)
        {
            return Equals(other, DefaultComparison);
        }

        /// <summary>
        /// Are the given folder paths equal. 
        /// Relative paths and absolute paths are compared using the current directory as a base
        /// Environment variables are expanded and then compared.
        /// By default the comparison is case sensitive.
        /// </summary>
        /// <param name="other">the folder to compare to</param>
        /// <returns>Whether the folder paths are equal</returns>
        public bool Equals(string other)
        {
            return Equals(new SmartFolder(other), DefaultComparison);
        }

        /// <summary>
        /// Are the given folder paths equal. 
        /// Relative paths and absolute paths are compared using the current directory as a base
        /// Environment variables are expanded and then compared.
        /// By default the comparison is case sensitive.
        /// </summary>
        /// <param name="other">the folder to compare to</param>
        /// <param name="comparison">The way to compare the paths, case sensitive or case insensitive</param>
        /// <returns>Whether the folder paths are equal</returns>
        public bool Equals(SmartFolder other, StringComparison comparison)
        {
            if (_value == null && other._value == null) return true;
            if ((_value == null) != (other._value == null)) return false;

            var thisFull = new DirectoryInfo(ExpandEnvironmentVariables(_value)).FullName;
            var otherFull = new DirectoryInfo(ExpandEnvironmentVariables(other._value)).FullName;

            if (!thisFull.EndsWith(DirectorySeparatorChar)) thisFull += DirectorySeparatorChar;
            if (!otherFull.EndsWith(DirectorySeparatorChar)) otherFull += DirectorySeparatorChar;

            return string.Equals(thisFull, otherFull, comparison);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <inheritdoc/>
        public static bool operator ==(SmartFolder lhs, SmartFolder rhs) => lhs.Equals(rhs, DefaultComparison);
        /// <inheritdoc/>
        public static bool operator !=(SmartFolder lhs, SmartFolder rhs) => !lhs.Equals(rhs, DefaultComparison);

        /// <summary>
        /// Combine the given paths
        /// </summary>
        public static SmartFolder operator +(SmartFolder lhs, SmartFolder rhs) => lhs.Add(rhs);
        /// <summary>
        /// Combine the given paths
        /// </summary>
        public static SmartFolder operator +(SmartFolder lhs, string rhs) => lhs.Add(rhs);
        /// <summary>
        /// Add the file to the folder and return the full path
        /// </summary>
        public static string operator +(SmartFolder lhs, FileInfo rhs) => lhs.Add(rhs);
        /// <inheritdoc/>
        public static SmartFolder operator -(SmartFolder lhs, SmartFolder rhs) => lhs.Subtract(rhs);
        /// <inheritdoc/>
        public static SmartFolder operator -(SmartFolder lhs, string rhs) => lhs.Subtract(rhs);

        /// <summary>
        /// Subtract the whole path from the current folder path.
        /// </summary>
        /// <param name="folder">the relative path to subtract from the given path</param>
        /// <returns>The (shortened) folder path</returns>
        public SmartFolder Subtract(SmartFolder folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            if (string.IsNullOrEmpty(folder._value)) return this;
            if (folder == this) throw new ArgumentOutOfRangeException(nameof(folder), Resources.SmartFolder_SubtractIdentical);
            if (folder.IsAbsolute && !IsAbsolute) throw new ArgumentOutOfRangeException(nameof(folder), Resources.SmartFolder_SubtractAbsolute);

            var current = (DirectoryInfo)this;
            if (!folder.IsAbsolute)
            {
                var parts = folder._value.Split(new char[] { DirectorySeparatorChar });
                foreach (var part in parts.Reverse())
                {
                    if (
                        current.FullName.EndsWith(part, DefaultComparison) ||
                        current.FullName.EndsWith(part + DirectorySeparatorChar, DefaultComparison))
                    {
                        current = current.Parent;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(folder), Resources.SmartFolder_SubtractNoCommonAncestor);
                    }
                }
                return new SmartFolder(current.FullName);
            }
            else if (current.FullName.StartsWith(folder.ToString() + DirectorySeparatorChar, DefaultComparison))
            {
                return new SmartFolder(current.FullName.Substring(folder._value.Length + 1));
            }
            else if (current.FullName.StartsWith(folder.ToString(), DefaultComparison))
            {
                return new SmartFolder(current.FullName.Substring(folder.ToString().Length));
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(folder), Resources.SmartFolder_SubtractNoCommonAncestor);
            }
        }

        /// <summary>
        /// Subtract the whole path from the current folder path.
        /// </summary>
        /// <param name="folder">the relative path to subtract from the given path</param>
        /// <returns>The (shortened) folder path</returns>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="folder"/> is absolute</exception>
        public SmartFolder Subtract(string folder)
        {
            return Subtract(new SmartFolder(folder));
        }

        /// <summary>
        /// Cast to a directory info
        /// </summary>
        /// <param name="folder">The smart folder</param>
        public static explicit operator DirectoryInfo(SmartFolder folder)
        {
            return folder.ToDirectoryInfo();
        }

        /// <summary>
        /// Cast the path to a string representation. This is equal to calling the ToString() function
        /// </summary>
        public static explicit operator string(SmartFolder path)
        {
            return path.ToString();
        }

        /// <summary>
        /// Convert to a directory info
        /// </summary>
        public DirectoryInfo ToDirectoryInfo()
        {
            return new DirectoryInfo(_value);
        }
        /// <summary>
        /// Move one folder up, same as calling <c>Up()</c>
        /// </summary>
        /// <param name="folder">the folder path</param>
        /// <returns>the parent folder of the given folder</returns>
        public static SmartFolder operator --(SmartFolder folder)
        {
            return Decrement(folder);
        }

        /// <summary>
        /// Move one folder up
        /// </summary>
        /// <returns>the parent folder of the current folder</returns>
        public SmartFolder Up()
        {
            return Decrement(this);
        }

        /// <summary>
        /// Returns the absolute folder path for the given path.
        /// This will expand environment variables and convert relative paths into absolute paths
        /// </summary>
        public SmartFolder ToAbsolute()
        {
            return new SmartFolder(new DirectoryInfo(ExpandEnvironmentVariables(_value)).FullName);
        }

        /// <summary>
        /// Move one folder up
        /// </summary>
        /// <returns>the parent folder of the current folder</returns>
        public static SmartFolder Decrement(SmartFolder path)
        {
            var current = new DirectoryInfo(path._value);
            return new SmartFolder(current.Parent.FullName);
        }

        /// <summary>
        /// Add the other (relative) path to the current path
        /// </summary>
        /// <param name="other">the relative path to add to the current path</param>
        /// <returns>The combined path</returns>
        private SmartFolder Add(string other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (string.IsNullOrEmpty(other)) return this;
            var safe = other;
            if (safe.StartsWith(DirectorySeparatorChar))
            {
                safe = safe.Substring(1);
            }
            if (IsPathRooted(safe)) throw new ArgumentOutOfRangeException(nameof(other), Resources.SmartFolder_CannotAddAbsolute);

            return new SmartFolder(Combine(_value, safe));
        }

        private string Add(FileInfo file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            return Combine(_value, file.Name);
        }

        /// <summary>
        /// Add the other (relative) path to the current path
        /// </summary>
        /// <param name="other">the relative path to add to the current path</param>
        /// <returns>The combined path</returns>
        public SmartFolder Add(SmartFolder other)
        {
            return Add(other._value);
        }


        /// <summary>
        /// Try to create a folder from the given text. Returns true and the resulting path when succeeds, false and the default path otherwise.
        /// </summary>
        /// <param name="folderPath">The string representation of the folder path to parse</param>
        /// <param name="result">The parsed path</param>
        /// <returns>Whether the parsing was succesful</returns>
        public static bool TryParse(string folderPath, out SmartFolder result)
        {
            if (folderPath != null && !folderPath.ContainsAny(GetInvalidPathChars()))
            {
                result = new SmartFolder(folderPath);
                return true;
            }
            else if (folderPath == null)
            {
                result = default;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Get the folder representing the current directory
        /// </summary>
        public static SmartFolder CurrentDirectory => new SmartFolder(".");

        /// <summary>
        /// Create a smartpath for the given volume (drive)
        /// </summary>
        /// <param name="volume">the name (letter) of the volume</param>
        /// <returns>The path</returns>
        /// <exception cref="ArgumentNullException">When the volume is null</exception>
        /// <exception cref="ArgumentException">When the volume is empty</exception>
        public static SmartFolder Volume(string volume)
        {
            if (volume == null) throw new ArgumentNullException(nameof(volume));
            if (string.IsNullOrEmpty(volume)) throw new ArgumentException(Resources.SmartFolder_VolumeCannotBeEmpty, nameof(volume));
            return new SmartFolder(volume + VolumeSeparatorChar + DirectorySeparatorChar);
        }

        /// <summary>
        /// Create a path for the given environment variable
        /// </summary>
        /// <param name="variable">The variable</param>
        /// <returns>The path</returns>
        public static SmartFolder FromEnvironmentVariable(string variable)
        {
            if (variable == null) throw new ArgumentNullException(nameof(variable));
            if (string.IsNullOrEmpty(variable)) throw new ArgumentException(Resources.SmartFolder_VariableCannotBeEmpty, nameof(variable));

            return new SmartFolder("%" + variable + "%");
        }

        TypeCode IConvertible.GetTypeCode() => TypeCode.Object;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(_value, provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(_value, provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(_value, provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(_value, provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(_value, provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(_value, provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(_value, provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(_value, provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(_value, provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(_value, provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(_value, provider);
        string IConvertible.ToString(IFormatProvider provider) => Convert.ToString(_value, provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(_value, conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(_value, provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(_value, provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(_value, provider);

        /// <inheritdoc/>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <inheritdoc/>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var value = reader.MoveToAttribute("value") ? reader.Value : null;

            if (string.IsNullOrEmpty(value))
            {
                Unsafe.AsRef(this) = default;
            }
            else
            {
                Unsafe.AsRef(this) = new SmartFolder(value);
            }

            reader.Skip();
        }

        /// <inheritdoc/>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            if (!string.IsNullOrEmpty(_value)) writer.WriteAttributeString("value", _value);
        }

        /// <summary>
        /// Returns the path in the given format. Allowed formats are:
        /// <list type="bullet">
        /// <item>null: the default format, same as G</item>
        /// <item>"": the default format, same as G</item>
        /// <item>G: the default format, will return the path with all environment variables expanded, using the separators for the given OS</item>
        /// <item>O: Original format, no environment variables will be expanded</item>
        /// <item>E: Escaped format, the path will be escaped by double quotes</item>
        /// <item>S: Smart escaped format, the path will be escaped by double quotes only when necessary</item>
        /// <item>L: Linux format, the path will be separated by slashes</item>
        /// <item>W: Windows format, the path will be separated by backslashes</item>
        /// <item>R: Relative format, returns the relative path when possible, the absolute path otherwise</item>
        /// </list>
        /// </summary>
        /// <param name="format">the format to use</param>
        /// <param name="formatProvider">the format provider, will not be used.</param>
        /// <returns>The string representation for the given path</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(_value)) return _value;
            if (format == "O") return _value;

            var expanded = ExpandEnvironmentVariables(_value);
            if (!expanded.EndsWith(DirectorySeparatorChar))
            {
                expanded = expanded + DirectorySeparatorChar;
            }

            if (format == "R") return expanded;

            var full = new DirectoryInfo(expanded).FullName;

            switch (format)
            {
                case "S":
                    if (full.Contains(' ')) return ToString("E", formatProvider);
                    else return ToString("G", formatProvider);
                case "E":
                    return $"\"{full}\"";
                case "L":
                    return full.Replace(DirectorySeparatorChar, '/');
                case "W":
                    return full.Replace(DirectorySeparatorChar, '\\');
                case null:
                case "":
                case "G":
                    return full;
                default:
                    throw new FormatException();
            }
        }

        /// <summary>
        /// All environment variables are expanded into absolute paths. 
        /// </summary>
        public SmartFolder Expand()
        {
            return new SmartFolder(ExpandEnvironmentVariables(_value));
        }

        /// <summary>
        /// Create a relative paths from all the folders given
        /// </summary>
        /// <param name="paths">A list of paths</param>
        public static SmartFolder Relative(params string[] paths)
        {
            return new SmartFolder(string.Join(DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture), paths));
        }

        /// <summary>
        /// Returns the folder that contains the given file
        /// </summary>
        /// <param name="file">The file that specifies the current folder</param>
        public static SmartFolder FromFile(FileInfo file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            return new SmartFolder(file.Directory);
        }


        /// <summary>
        /// Create an absolute paths for the volume and all the folders given.
        /// </summary>
        /// <param name="volume">The root volume</param>
        /// <param name="paths">A list of paths</param>
        public static SmartFolder Absolute(string volume, params string[] paths)
        {
            return
                new SmartFolder(
                    volume + VolumeSeparatorChar + DirectorySeparatorChar +
                    string.Join(DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture), paths));
        }

        /// <summary>
        /// Returns the path in the given format. Allowed formats are:
        /// <list type="bullet">
        /// <item>null: the default format, same as G</item>
        /// <item>"": the default format, same as G</item>
        /// <item>G: the default format, will return the path with all environment variables expanded, using the separators for the given OS</item>
        /// <item>O: Original format, no environment variables will be expanded</item>
        /// <item>E: Escaped format, the path will be escaped by double quotes</item>
        /// <item>L: Linux format, the path will be separated by slashes</item>
        /// <item>W: Windows format, the path will be separated by backslashes</item>
        /// </list>
        /// </summary>
        /// <param name="format">the format to use</param>
        /// <returns>The string representation for the given path</returns>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

    }
}
