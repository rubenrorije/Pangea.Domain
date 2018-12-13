using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Pangea.Domain
{
    /// <summary>
    /// Represent a number of bytes in a human readable format
    /// </summary>
    [Serializable]
    public struct FileSize
        : IEquatable<FileSize>
        , IComparable<FileSize>
        , IComparable<long>
        , IFormattable
        , IComparable
        , IXmlSerializable
    {
        /// <summary>
        /// The total number of bytes that are stored within the FileSize
        /// </summary>
        public long TotalBytes { get; }
        /// <summary>
        /// The calculated number of KiloBytes that are stored within the FileSize.
        /// This is equal to TotalBytes / 1024
        /// </summary>
        public double TotalKiloBytes => TotalBytes / 1024.0;
        /// <summary>
        /// The calculated number of MegaBytes that are stored within the FileSize.
        /// This is equal to TotalBytes / (1024 * 1024) 
        /// </summary>
        public double TotalMegaBytes => TotalBytes / (1024.0 * 1024.0);
        /// <summary>
        /// The calculated number of GigaBytes that are stored within the FileSize.
        /// This is equal to TotalBytes / (1024 * 1024 * 1024) 
        /// </summary>
        public double TotalGigaBytes => TotalBytes / (1024.0 * 1024.0 * 1024.0);
        /// <summary>
        /// The calculated number of TeraBytes that are stored within the FileSize.
        /// This is equal to TotalBytes / (1024 * 1024 * 1024) 
        /// </summary>
        public double TotalTeraBytes => TotalBytes / (1024.0 * 1024.0 * 1024.0 * 1024.0);

        /// <summary>
        /// Create a FileSize based on the number of bytes that are passed. To create a FileSize 
        /// based on Kilo/Mega/TeraBytes use the FromXXX static methods.
        /// </summary>
        /// <param name="bytes">The number of bytes.</param>
        public FileSize(long bytes)
        {
            TotalBytes = bytes;
        }

        /// <summary>
        /// Create a FileSize based on the stream that is used. The FileSize will hold the number of bytes in the stream
        /// when null is passed, creating a FileSize will not throw an exception, the FileSize will be 0B
        /// </summary>
        /// <param name="stream">The stream that contains the file</param>
        public FileSize(Stream stream)
        {
            if (stream == null) TotalBytes = 0;
            else TotalBytes = stream.Length;
        }

        /// <summary>
        /// Create a FileSize based on the FileInfo that is used. The FileSize will hold the number of bytes in the file.
        /// When <paramref name="file"/> is null, the FileSize will not throw an exception and the FileSize will be 0B
        /// </summary>
        /// <param name="file">The file for which the FileSize must be calculated</param>
        public FileSize(FileInfo file)
        {
            if (file == null) TotalBytes = 0;
            else TotalBytes = file.Length;
        }

        /// <summary>
        /// Returns the representation of the FileSize by showing this in the most appropriate manner.
        /// When the FileSize is more than 1GB, the FileSize will be shown in GB, likewise for MegaBytes and KiloBytes.
        /// </summary>
        /// <returns>The string representation of the FileSize</returns>
        public override string ToString()
        {
            return ToString("G", null);
        }

        /// <summary>
        /// Returns the representation of the FileSize by showing this in the most appropriate manner.
        /// When the FileSize is more than 1GB, the FileSize will be shown in GB, likewise for MegaBytes and KiloBytes.
        /// The given format will be used to format the numbers. 
        /// </summary>
        /// <param name="format">The format of the numbers in the string representation</param>
        /// <returns>The string representation of the FileSize</returns>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Returns the representation of the FileSize by showing this in the most appropriate manner.
        /// When the FileSize is more than 1GB, the FileSize will be shown in GB, likewise for MegaBytes and KiloBytes.
        /// The given format will be used to format the numbers. 
        /// </summary>
        /// <param name="format">The format of the numbers in the string representation</param>
        ///<param name="formatProvider">The format provider</param>
        /// <returns>The string representation of the FileSize</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var safeFormat = format ?? "G";
            var safeFormatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            if (TotalTeraBytes >= 1) return TotalTeraBytes.ToString(safeFormat, safeFormatProvider) + "TB";
            else if (TotalGigaBytes >= 1) return TotalGigaBytes.ToString(safeFormat, safeFormatProvider) + "GB";
            else if (TotalMegaBytes >= 1) return TotalMegaBytes.ToString(safeFormat, safeFormatProvider) + "MB";
            else if (TotalKiloBytes >= 1) return TotalKiloBytes.ToString(safeFormat, safeFormatProvider) + "KB";
            else return TotalBytes.ToString(safeFormat, safeFormatProvider) + "B";
        }


        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is FileSize && Equals((FileSize)obj);
        }

        /// <summary>
        /// Check equality for File sizes.
        /// </summary>
        /// <param name="other">The FileSize to Compare to</param>
        /// <returns>returns whether the FileSizes are equal</returns>
        public bool Equals(FileSize other)
        {
            return TotalBytes.Equals(other.TotalBytes);
        }

        /// <summary>
        /// the hashcode of the FileSize, is equal to the hashcode of TotalBytes
        /// </summary>
        /// <returns>the hashcode of this FileSize</returns>
        public override int GetHashCode()
        {
            return TotalBytes.GetHashCode();
        }

        /// <inheritdoc/>
        public int CompareTo(FileSize other)
        {
            return TotalBytes.CompareTo(other.TotalBytes);
        }

        /// <inheritdoc/>
        public int CompareTo(long other)
        {
            return TotalBytes.CompareTo(other);
        }

        /// <inheritdoc/>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc/>
        public void ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var value = reader.MoveToAttribute("value") ? reader.Value : null;

            if (string.IsNullOrEmpty(value))
            {
                Unsafe.AsRef(this) = default;
            }
            else
            {
                Unsafe.AsRef(this) = new FileSize(long.Parse(value, CultureInfo.InvariantCulture));
            }

            reader.Skip();
        }

        /// <inheritdoc/>
        public void WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (TotalBytes > 0) writer.WriteAttributeString("value", TotalBytes.ToString(null, CultureInfo.InvariantCulture));
        }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (obj is long) return CompareTo((long)obj);
            if (obj is FileSize) return CompareTo((FileSize)obj);
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public static bool operator ==(FileSize lhs, FileSize rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <inheritdoc/>
        public static bool operator !=(FileSize lhs, FileSize rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <inheritdoc/>
        public static bool operator >(FileSize lhs, FileSize rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        /// <inheritdoc/>
        public static bool operator <(FileSize lhs, FileSize rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        /// <inheritdoc/>
        public static bool operator >=(FileSize lhs, FileSize rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        /// <inheritdoc/>
        public static bool operator <=(FileSize lhs, FileSize rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        /// <inheritdoc/>
        public static bool operator >(FileSize lhs, long rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        /// <inheritdoc/>
        public static bool operator <(FileSize lhs, long rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        /// <inheritdoc/>
        public static bool operator >=(FileSize lhs, long rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        /// <inheritdoc/>
        public static bool operator <=(FileSize lhs, long rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        /// <inheritdoc/>
        public static FileSize operator +(FileSize lhs, FileSize rhs)
        {
            return lhs.Add(rhs);
        }

        /// <summary>
        /// Add the given file size to the current file size
        /// </summary>
        /// <param name="other">The file size to add</param>
        /// <returns>The added file sizes</returns>
        public FileSize Add(FileSize other)
        {
            return new FileSize(TotalBytes + other.TotalBytes);
        }

        /// <inheritdoc/>
        public static FileSize operator -(FileSize lhs, FileSize rhs)
        {
            return lhs.Subtract(rhs);
        }

        /// <summary>
        /// Subtract the given file size from this instance and return a new instance.
        /// </summary>
        public FileSize Subtract(FileSize other)
        {
            return new FileSize(TotalBytes - other.TotalBytes);
        }

        /// <inheritdoc/>
        public static FileSize operator *(FileSize lhs, long rhs)
        {
            return lhs.Multiply(rhs);
        }

        /// <summary>
        /// Multiply the file size by the given multiplier
        /// </summary>
        /// <param name="multiplier">The multiplier</param>
        /// <returns>The result</returns>
        public FileSize Multiply(long multiplier)
        {
            return new FileSize(TotalBytes * multiplier);
        }

        /// <inheritdoc/>
        public static FileSize operator /(FileSize lhs, long rhs)
        {
            return lhs.Divide(rhs);
        }

        /// <summary>
        /// Divide the File size by the given divisor
        /// </summary>
        /// <param name="divisor">The divisor</param>
        /// <returns>The result</returns>
        public FileSize Divide(long divisor)
        {
            return new FileSize(TotalBytes / divisor);
        }
    }
}
