using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DotNet20BridgeNet4x
{
    /// <summary>
    /// Represents the .NET Framework for the target assembly.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum TargetPlatformVersion
    {
        /// <summary>
        /// NET Framework v1
        /// </summary>
        v1,

        /// <summary>
        /// NET Framework v1.1
        /// </summary>
        v11,

        /// <summary>
        /// NET Framework v2
        /// </summary>
        v2,

        /// <summary>
        /// NET Framework v4
        /// </summary>
        v4
    }
}
