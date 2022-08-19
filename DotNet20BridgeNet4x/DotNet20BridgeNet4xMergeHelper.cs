using System;
using System.Collections.Generic;
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

        public static void MergeDll(string[] dlls) {
/*
cd ILRepack\bin\Release\netcoreapp3.1\
dotnet ILRepack.dll /log /wildcards /internalize /out:..\..\netcoreapp3.1\ILRepack.dll /target:library ILRepack.dll BamlParser.dll Fasterflect.dll Mono.Cecil.dll Mono.Cecil.Mdb.dll Mono.Cecil.Pdb.dll Mono.Cecil.Rocks.dll runtimes\win-x64\lib\netstandard2.0\Mono.Posix.NETStandard.dll
cd ..\net461
ILRepack.exe /log /wildcards /internalize /out:..\..\net461\ILRepack.dll /target:library ILRepack.exe BamlParser.dll Fasterflect.dll Mono.Cecil.dll Mono.Cecil.Mdb.dll Mono.Cecil.Pdb.dll Mono.Cecil.Rocks.dll Mono.Posix.dll 
 */ 

     }

    }
}
