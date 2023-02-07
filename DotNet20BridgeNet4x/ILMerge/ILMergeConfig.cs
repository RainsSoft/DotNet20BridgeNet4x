using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNet20BridgeNet4x.ILMerge
{
    // ILMerge.exe就是用于将多个程序集合并的工具。
    // 官方下载地址: http://www.microsoft.com/en-us/download/details.aspx?id=17630
    //            安装后可在命令提示符中使用。
    // 命令内容：
    // Usage: ilmerge[/ lib:directory] * [/ log[:filename]][/ keyfile:filename[/ delaysign]][/ internalize[:filename]][/ t[arget]:(library | exe | winexe)][/ closed][/ ndebug][/ ver:version][/ copyattrs[/ allowMultiple][/ keepFirst]][/ xmldocs][/ attr:filename]
    // [/ targetplatform:< version >[,< platformdir >] | / v1 | / v1.1 | / v2 | / v4][/ useFullPublicKeyForReferences]
    // [/ wildcards][/ zeroPeKind][/ allowDup:type] * [/ union][/ align:n]
    // /out:filename < primary assembly > [< other assemblies > ...]
    // 其中这两个是必须的参数：/out:filename < primary assembly >
    // out:filename 表示输出的程序集名，< primary assembly > 表示输入的主程序集名。下面是一个实例：
    // ilmerge.exe / t:winexe / targetplatform:v2 /out:main.new.exe main.exe Interop.IWshRuntimeLibrary.dll
    // 表示将主程序集main.exe和另一个程序集Interop.IWshRuntimeLibrary.dll合并为main.new.exe，/ t:winexe表示生成目标是Windows应用程序，/ targetplatform:v2表示生成目标是.net 2.0程序集。

    //合并dll
    //ilmerge.exe /?
    //ilmerge.exe / log : merge1.log / t:library / targetplatform:v2 / ndebug /out:DotNet20BridgeNet4x.Net20.dll DotNet20BridgeNet4x.dll System.Mono.Posix.Net20.dll System.Core.Net20.dll System.Threading.Net20.dll
    //pause
    public class ILMergeConfig
    {

        /// <summary>
        /// ILRepack.exe (must exist)
        /// </summary>
        public FileInfo ILMergeExeFile { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether whether types in assemblies other
        /// than the primary assembly should have their visibility modified to internal.
        /// </summary>
        /// <value>
        /// <c>true</c> if types in assemblies other than the primary assembly should
        /// have their visibility modified to internal; otherwise, <c>false</c>.
        /// </value>
        public bool Internalize { get; set; }

        /// <summary>
        /// Gets or sets the target kind.
        /// </summary>
        /// <value>The target kind.</value>
        public TargetKind TargetKind { get; set; }

        /// <summary>
        /// Gets or sets the target platform.
        /// </summary>
        /// <value>The target platform.</value>
        public TargetPlatform TargetPlatform { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ILMergeSettings"/> class.
        /// </summary>
        public ILMergeConfig() {
            Internalize = false;
            TargetKind = TargetKind.Default;
        }
    }
}
