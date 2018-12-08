using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Versioning;
using System.Text;

namespace Pangea.Domain
{
    /// <summary>
    /// Type safe class to inspect all assembly information
    /// </summary>
    public class AssemblyInformation
    {

        /// <summary>
        /// Get the full name of the assembly
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets the hash algorithm of an assembly manifest's contents, specified in <code>AssemblyAlgorithmIdAttribute</code>
        /// </summary>
        public AssemblyHashAlgorithm AlgorithmId { get; }
        /// <summary>
        /// Get the company of the assembly, specified in <code>AssemblyCompanyAttribute</code>
        /// </summary>
        public string Company { get; }
        /// <summary>
        /// Get the configuration of the assembly, specified in <code>AssemblyConfigurationAttribute</code>
        /// </summary>
        public string Configuration { get; }
        /// <summary>
        /// Get the copyright of the assembly, specified in <code>AssemblyCopyrightAttribute</code>
        /// </summary>
        public string Copyright { get; }
        /// <summary>
        /// Get the culture of the assembly, specified in <code>AssemblyCultureAttribute</code>
        /// </summary>
        public string Culture { get; }
        /// <summary>
        /// Get the default alias of the assembly, specified in <code>AssemblyDefaultAliasAttribute</code>
        /// </summary>
        public string DefaultAlias { get; }
        /// <summary>
        /// Get Delay Sign value of the assembly, specified in <code>AssemblyDelaySignAttribute</code>
        /// </summary>
        public bool DelaySign { get; }

        /// <summary>
        /// Get the description of the assembly, specified in <code>AssemblyDescriptionAttribute</code>
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Get the FileVersion of the assembly, specified in <code>AssemblyFileVersionAttribute</code>
        /// </summary>
        public string FileVersion { get; }

        /// <summary>
        /// Get the FileVersion of the assembly, specified in <code>AssemblyFileVersionAttribute</code>
        /// </summary>
        public Version FileVersionInfo => new Version(FileVersion);

        /// <summary>
        /// Provides information about the assembly, specified in <code>AssemblyFlagsAttribute</code>
        /// </summary>
        public AssemblyNameFlags Flags { get; }


        /// <summary>
        /// Specifies that just-in-time(JIT) compiler optimization is disabled for the assembly.
        /// This is the EXACT OPPOSITE of the meaning that is suggested by the member name.
        /// </summary>
        public bool EnableJITcompileOptimizer => Flags.HasFlag(AssemblyNameFlags.EnableJITcompileOptimizer);

        ///<summary>
        ///Specifies that just-in-time(JIT) compiler tracking is enabled for the assembly.
        ///</summary>
        public bool EnableJITcompileTracking => Flags.HasFlag(AssemblyNameFlags.EnableJITcompileTracking);

        /// <summary>
        /// Specifies that a public key is formed from the full public key rather than the public key token.
        /// </summary>
        public bool HasPublicKey => Flags.HasFlag(AssemblyNameFlags.PublicKey);

        /// <summary>
        /// Specifies that the assembly can be retargeted at runtime to an assembly from a different publisher. 
        /// This value supports the .NET Framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public bool Retargetable => Flags.HasFlag(AssemblyNameFlags.Retargetable);

        /// <summary>
        /// Defines additional version information for an assembly manifest, specified in <code>AssemblyInformationalVersionAttribute</code>
        /// </summary>
        public string InformationalVersion { get; }
        /// <summary>
        /// Gets the name of the file containing the key pair used to generate a strong name
        /// for the attributed assembly, specified in <code>AssemblyKeyFileAttribute</code>
        /// </summary>
        public string KeyFile { get; }
        /// <summary>
        /// Specifies the name of a key container within the CSP containing the key pair
        /// used to generate a strong name, specified in <code>AssemblyKeyNameAttribute</code>
        /// </summary>
        public string KeyName { get; }

        private List<KeyValuePair<string, string>> _metaData;

        /// <summary>
        /// All meta data specified by zero or more <c>AssemblyMetadataAttribute</c>s
        /// </summary>
        public IReadOnlyCollection<KeyValuePair<string, string>> MetaData => _metaData;

        /// <summary>
        ///  Defines a product name custom attribute for an assembly manifest, specified in specified in <c>AssemblyProductAttribute</c>
        /// </summary>
        public string Product { get; }

        /// <summary>
        /// Provides migration from an older, simpler strong name key to a larger key with a stronger hashing algorithm, specified in the <c>AssemblySignatureKeyAttribute</c>
        /// </summary>
        public string PublicKey { get; }
        /// <summary>
        /// Provides migration from an older, simpler strong name key to a larger key with a stronger hashing algorithm, specified in the <c>AssemblySignatureKeyAttribute</c>
        /// </summary>
        public string Countersignature { get; }
        /// <summary>
        /// Gets assembly title information, specified in <c>AssemblyTitleAttribute</c>
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// Defines a trademark custom attribute for an assembly manifest, specified in <c>AssemblyTrademarkAttribute</c>
        /// </summary>
        public string Trademark { get; }
        /// <summary>
        /// Get the assembly version, specified in <c>AssemblyVersionAttribute</c>
        /// </summary>
        public Version AssemblyVersionInfo { get; }
        /// <summary>
        /// Gets the name of the .NET Framework version against which a particular assembly was compiled, specified in <c>TargetFrameworkAttribute</c>
        /// </summary>
        public string TargetFrameworkName { get; }
        /// <summary>
        /// Gets the display name of the .NET Framework version against which an assembly was built, specified in <c>TargetFrameworkAttribute</c>
        /// </summary>
        public string TargetFrameworkDisplayName { get; }
        /// <summary>
        /// Get the neutral language culture name, specified in <c>NeutralResourcesLanguageAttribute</c>
        /// </summary>
        public string NeutralLanguageCultureName { get; }

        /// <summary>
        /// The compile time as specified in the header of the assemlby. Note that because of 'deterministic' builds this may no longer work.
        /// See http://blog.paranoidcoding.com/2016/04/05/deterministic-builds-in-roslyn.html for more information about 'deterministic' builds.
        /// </summary>
        public DateTime? LinkerTime { get; }

        /// <summary>
        /// The name of the assembly
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Get the fall back location for the resource, specified in <c>NeutralResourcesLanguageAttribute</c>
        /// </summary>
        public UltimateResourceFallbackLocation NeutralLanguageLocationName { get; }

        /// <summary>
        /// Get the creation time in UTC for the assembly file. For generated assemblies the creation time will be null
        /// </summary>
        public DateTime? FileCreationTimeUtc { get; }

        /// <summary>
        /// Get the last write time in UTC for the assembly file. For generated assemblies the creation time will be null
        /// </summary>
        public DateTime? FileLastWriteTimeUtc { get; }

        /// <summary>
        /// Create an easy way to get all information about the given assembly
        /// </summary>
        /// <param name="assembly">The assembly to extract the information from</param>
        public AssemblyInformation(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            var assemblyName = assembly.GetName();

            FullName = assembly.FullName;
            AlgorithmId =
                (AssemblyHashAlgorithm?)assembly.GetCustomAttribute<AssemblyAlgorithmIdAttribute>()?.AlgorithmId ??
                AssemblyHashAlgorithm.None;
            Company = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
            Configuration = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration;
            Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
            Culture = assembly.GetCustomAttribute<AssemblyCultureAttribute>()?.Culture;
            DefaultAlias = assembly.GetCustomAttribute<AssemblyDefaultAliasAttribute>()?.DefaultAlias;
            DelaySign = assembly.GetCustomAttribute<AssemblyDelaySignAttribute>()?.DelaySign ?? false;
            Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
            FileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
            Flags =
                ((AssemblyNameFlags?)assembly.GetCustomAttribute<AssemblyFlagsAttribute>()?.AssemblyFlags) ??
                AssemblyNameFlags.None;
            InformationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            KeyFile = assembly.GetCustomAttribute<AssemblyKeyFileAttribute>()?.KeyFile;
            KeyName = assembly.GetCustomAttribute<AssemblyKeyNameAttribute>()?.KeyName;
            _metaData = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Select(att => new KeyValuePair<string, string>(att.Key, att.Value)).ToList();
            Product = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
            PublicKey = assembly.GetCustomAttribute<AssemblySignatureKeyAttribute>()?.PublicKey;
            Countersignature = assembly.GetCustomAttribute<AssemblySignatureKeyAttribute>()?.Countersignature;
            Title = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
            Trademark = assembly.GetCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark;
            AssemblyVersionInfo = assemblyName.Version;

            TargetFrameworkName = assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            TargetFrameworkDisplayName = assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName;
            NeutralLanguageCultureName = assembly.GetCustomAttribute<NeutralResourcesLanguageAttribute>()?.CultureName;
            NeutralLanguageLocationName = assembly.GetCustomAttribute<NeutralResourcesLanguageAttribute>()?.Location ?? UltimateResourceFallbackLocation.MainAssembly;
            LinkerTime = GetLinkerTime(assembly);
            Name = assemblyName.Name;
            
            if (!assembly.IsDynamic)
            {
                FileCreationTimeUtc = File.GetCreationTimeUtc(new Uri(assembly.CodeBase).LocalPath);
                FileLastWriteTimeUtc = File.GetLastWriteTimeUtc(new Uri(assembly.CodeBase).LocalPath);
            }
        }

        private static DateTime? GetLinkerTime(Assembly assembly)
        {
            if (assembly.IsDynamic) return null;

            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.Read(buffer, 0, 2048);
            }

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            return linkTimeUtc;
        }

        /// <summary>
        /// Return the meta data value associated with this key. 
        /// a <c>KeyNotFoundException</c> when there is no <c>AssemblyMetadataAttribute</c> with this key.
        /// The key is case sensitive
        /// </summary>
        /// <param name="key">the case sensitive key of the <c>AssemblyMetadataAttribute</c></param>
        /// <returns>the value</returns>
        public string this[string key]
        {
            get
            {
                var items = _metaData.Where(kvp => kvp.Key == key).ToList();
                if (items.Count < 1) throw new KeyNotFoundException();
                return items.First().Value;
            }
        }

        /// <summary>
        /// Create the assembly information for the calling assembly. That is, the assembly invoking this method.
        /// This is the same as using <code>new AssemblyInformation(Assembly.GetCallingAssembly())</code>
        /// </summary>
        /// <returns>The assembly information</returns>
        public static AssemblyInformation ForCallingAssembly()
        {
            return new AssemblyInformation(Assembly.GetCallingAssembly());
        }
        /// <summary>
        /// Create the assembly information for the Entry assembly. That is, the process executable in the default application domain)
        /// This is the same as using <code>new AssemblyInformation(Assembly.GetEntryAssembly())</code>
        /// </summary>
        /// <returns>The assembly information</returns>
        public static AssemblyInformation ForEntryAssembly()
        {
            return new AssemblyInformation(Assembly.GetEntryAssembly());
        }
        /// <summary>
        /// Create the assembly information for the Entry assembly. That is, the process executable in the default application domain)
        /// This is the same as using <code>new AssemblyInformation(Assembly.GetEntryAssembly())</code>
        /// </summary>
        /// <returns>The assembly information</returns>
        public static AssemblyInformation ForExecutingAssembly()
        {
            return new AssemblyInformation(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Create the assembly information for the assembly in which <code>type</code> is defined
        /// </summary>
        /// <param name="type">the type that specifies the assembly</param>
        /// <returns>the assembly information</returns>
        public static AssemblyInformation ForAssemblyOf(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return new AssemblyInformation(type.Assembly);
        }

        /// <summary>
        /// Create the assembly information for the assembly in which <code>type</code> is defined
        /// </summary>
        /// <typeparam name="T">The type that specifies the assembly</typeparam>
        public static AssemblyInformation ForAssemblyOf<T>()
        {
            return ForAssemblyOf(typeof(T));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return FullName;
        }
    }
}
