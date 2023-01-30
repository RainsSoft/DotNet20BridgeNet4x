using System;
using System.Collections.Generic;
using System.Text;
using DirectoryPath = System.IO.DirectoryInfo;
namespace DotNet20BridgeNet4x
{
    /// <summary>
    /// Represents a target platform.
    /// </summary>
    public sealed class TargetPlatform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TargetPlatform"/> class.
        /// </summary>
        /// <param name="platform">The .NET framework target version.</param>
        public TargetPlatform(TargetPlatformVersion platform) {
            Platform = platform;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetPlatform"/> class.
        /// </summary>
        /// <param name="platform">The .NET framework target version.</param>
        /// <param name="path">The directory where <c>mscorlib.dll</c> can be found.</param>
        public TargetPlatform(TargetPlatformVersion platform, DirectoryPath path) {
            if (path == null) {
                throw new ArgumentNullException(nameof(path));
            }
            Platform = platform;
            Path = path;
        }

        /// <summary>
        /// Gets the .NET framework target version.
        /// </summary>
        public TargetPlatformVersion Platform { get; }

        /// <summary>
        /// Gets the directory where <c>mscorlib.dll</c> can be found.
        /// </summary>
        public DirectoryPath Path { get; }
    }
}
