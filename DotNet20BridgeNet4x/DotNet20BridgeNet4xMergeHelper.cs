using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNet20BridgeNet4x
{
    public class DotNet20BridgeNet4xMergeHelper
    {
        //  Syntax: ILRepack.exe[options] /out:<path> <path_to_primary> [<other_assemblies> ...]

        // - /help displays this usage
        // - /keyfile:<path>      specifies a keyfile to sign the output assembly
        // - /keycontainer:<name> specifies a key container to sign the output assembly(takes precedence over /keyfile)
        // - /log:<logfile>       enable logging(to a file, if given) (default is disabled)
        // - /ver:M.X.Y.Z target assembly version
        // - /union merges types with identical names into one
        // - /ndebug disables symbol file generation
        // - /copyattrs copy assembly attributes(by default only the primary assembly attributes are copied)
        // - /attr:<path>         take assembly attributes from the given assembly file
        // - /allowMultiple when copyattrs is specified, allows multiple attributes(if type allows)
        // - /target:kind specify target assembly kind(library, exe, winexe supported, default is same as first assembly)
        // - /targetplatform:P specify target platform(v1, v1.1, v2, v4 supported)
        // - /xmldocs merges XML documentation as well
        // - /lib:<path>          adds the path to the search directories for referenced assemblies(can be specified multiple times)
        // - /internalize sets all types but the ones from the first assembly 'internal'
        // - /renameInternalized rename all internalized types
        // - /delaysign sets the key, but don't sign the assembly
        // - /usefullpublickeyforreferences - NOT IMPLEMENTED
        // - /align               - NOT IMPLEMENTED
        // - /closed              - NOT IMPLEMENTED
        // - /allowdup:Type allows the specified type for being duplicated in input assemblies
        // - /allowduplicateresources allows to duplicate resources in output assembly(by default they're ignored)
        // - /zeropekind allows assemblies with Zero PeKind (but obviously only IL will get merged)
        // - /wildcards allows(and resolves) file wildcards(e.g. `*`.dll) in input assemblies
        // 允许（并解析）输入程序集中的文件通配符（例如，`*`.dll）
        // - /parallel use as many CPUs as possible to merge the assemblies
        // - /pause pause execution once completed(good for debugging)
        // - /verbose shows more logs
        // - /out:<path>          target assembly path, symbol/config/doc files will be written here as well
        // - <path_to_primary>    primary assembly, gives the name, version to the merged one
        // - <other_assemblies>   ...

        //Note: for compatibility purposes, all options can be specified using '/', '-' or '--' prefix.

        public static void CreateCmd_MergeDll_ILRepack(string[] dlls,FileInfo ILRepackExe, string outputAssemblyFullName) {
            /*
            cd ILRepack\bin\Release\netcoreapp3.1\
            dotnet ILRepack.dll /log /wildcards /internalize /out:..\..\netcoreapp3.1\ILRepack.dll /target:library ILRepack.dll BamlParser.dll Fasterflect.dll Mono.Cecil.dll Mono.Cecil.Mdb.dll Mono.Cecil.Pdb.dll Mono.Cecil.Rocks.dll runtimes\win-x64\lib\netstandard2.0\Mono.Posix.NETStandard.dll
            cd ..\net461
            ILRepack.exe /log /wildcards /internalize /out:..\..\net461\ILRepack.dll /target:library ILRepack.exe BamlParser.dll Fasterflect.dll Mono.Cecil.dll Mono.Cecil.Mdb.dll Mono.Cecil.Pdb.dll Mono.Cecil.Rocks.dll Mono.Posix.dll 
             */
            //合并dll
            //ILRepack.exe / help
            //ILRepack.exe / log:merge2.log / ndebug / wildcards /out:..\..\Bin_DLL\DotNet20BridgeNet4x.Net20.dll / target:library DotNet20BridgeNet4x.dll System.Mono.Posix.Net20.dll System.Core.Net20.dll System.Threading.Net20.dll
            //pause
            ILRepack.ILRepackConfig config = new ILRepack.ILRepackConfig() {
                ILRepackExeFile = ILRepackExe,
                 Log="ILRepack_MergeDLL.log",
                 NDebug=true,
                 Verbose=true,
                 XmlDocs=true,
            };
            config.Libs = new List<DirectoryInfo>();
            config.Libs.Add(new DirectoryInfo(new FileInfo( dlls[0]).DirectoryName));
            ILRepack.ILRepackCmdBuilder builder = new ILRepack.ILRepackCmdBuilder(config);
            List<FileInfo> refDlls = new List<FileInfo>();
            for (int i = 1; i < dlls.Length; i++) {
                refDlls.Add(new FileInfo(dlls[i]));
            }
            string cmdStr = builder.BuilderCmd(new FileInfo(outputAssemblyFullName), new FileInfo(dlls[0]), refDlls);
            File.WriteAllText("ILRepack_CMD_"+new FileInfo(dlls[0]).Name+".bat",cmdStr,Encoding.ASCII);
        }
        public static void MergeDLL_ILMerge(string[] dlls, FileInfo ILMergeExe, string outputAssemblyFullName) {
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
            ILMerge.ILMergeConfig config = new  ILMerge.ILMergeConfig() {
                 ILMergeExeFile = ILMergeExe,
                 Internalize=false
            };

            ILMerge.ILMergeCmdBuilder builder = new  ILMerge.ILMergeCmdBuilder(config);
            List<FileInfo> refDlls = new List<FileInfo>();
            for (int i = 1; i < dlls.Length; i++) {
                refDlls.Add(new FileInfo(dlls[i]));
            }
            string cmdStr = builder.BuilderCmd(new FileInfo(outputAssemblyFullName), new FileInfo(dlls[0]), refDlls);
            File.WriteAllText("ILMerge_CMD_" + new FileInfo(dlls[0]).Name + ".bat", cmdStr, Encoding.ASCII);
        }
    }
}
