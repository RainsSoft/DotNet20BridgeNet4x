using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNet20BridgeNet4x.ILRepack
{
    /// <summary>
    /// 根据配置构建ILRepack CMD命令
    /// </summary>
    public class ILRepackCmdBuilder : IBuilderCmd
    {
        //public ILRepackCmdBuilder() {
        //}
        public ILRepackCmdBuilder(ILRepackConfig config) {
            this.Config = config;
        }
        public ILRepackConfig Config {
            get;
            set;
        }
        public string BuilderCmd(FileInfo outputAssemblyPath, FileInfo primaryAssemblyPath,
            IEnumerable<FileInfo> assemblyPaths) {
            if (Config == null) {
                return "";
            }
            if (Config.ILRepackExeFile == null) {
                throw new Exception("Config.ILRepackExeFile 未赋值");
            }
            if (outputAssemblyPath == null) {
                throw new ArgumentNullException(nameof(outputAssemblyPath));
            }
            if (primaryAssemblyPath == null) {
                throw new ArgumentNullException(nameof(primaryAssemblyPath));
            }
            if (assemblyPaths == null) {
                throw new ArgumentNullException(nameof(assemblyPaths));
            }
           
            string outPath = outputAssemblyPath.DirectoryName;
            CheckDirectory(outPath);
            //ILRepack.exe / help
            //ILRepack.exe / log:merge2.log / ndebug / wildcards /out:..\..\Bin_DLL\DotNet20BridgeNet4x.Net20.dll / target:library DotNet20BridgeNet4x.dll System.Mono.Posix.Net20.dll System.Core.Net20.dll System.Threading.Net20.dll
            //pause
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("cd "+Config.ILRepackExeFile.DirectoryName);
            int index = Config.ILRepackExeFile.DirectoryName.IndexOf(':');
            builder.AppendLine(Config.ILRepackExeFile.DirectoryName.Substring(0,index+1));
            builder.AppendLine("echo %cd%");
            builder.AppendLine("echo ------------------------------------");
            builder.Append(Config.ILRepackExeFile.Name);
            //
            if (Config.Keyfile != null) {
                builder.Append(" /keyfile:" + Config.Keyfile.FullName.Quote());
            }

            if (!string.IsNullOrEmpty(Config.Log)) {
                builder.Append(" /log:" + Path.Combine(outPath,new FileInfo(Config.Log).Name).Quote());
            }

            if (Config.Version != null) {
                builder.Append(" /ver:" + Config.Version);
            }

            if (Config.Union) {
                builder.Append(" /union");
            }

            if (Config.NDebug) {
                builder.Append(" /ndebug");
            }

            if (Config.CopyAttrs) {
                builder.Append(" /copyattrs");
            }

            if (Config.Attr != null) {
                builder.Append(" /attr:" + Config.Attr.FullName.Quote());
            }

            if (Config.AllowMultiple) {
                builder.Append(" /allowmultiple");
            }

            if (Config.TargetKind != TargetKind.Default) {
                builder.Append(" /target:"+ GetTargetKindName(Config.TargetKind).Quote());
            }

            if (Config.TargetPlatform.HasValue) {                
                builder.Append(" /targetplatform:"+ GetTargetPlatformString(Config.TargetPlatform.Value));
            }

            if (Config.XmlDocs) {
                builder.Append(" /xmldocs");
            }

            if (Config.Libs != null && Config.Libs.Count > 0) {
                foreach (var lib in Config.Libs) {
                    builder.Append(" /lib:" + lib.FullName.Quote());
                }
            }

            if (Config.Internalize) {
                builder.Append(" /internalize");
            }

            if (Config.DelaySign) {
                builder.Append(" /delaysign");
            }

            if (!string.IsNullOrEmpty(Config.AllowDup)) {
                builder.Append(" /allowdup:" + Config.AllowDup);
            }

            if (Config.AllowDuplicateResources) {
                builder.Append(" /allowduplicateresources");
            }

            if (Config.ZeroPeKind) {
                builder.Append(" /zeropekind");
            }

            if (Config.Wildcards) {
                builder.Append(" /wildcards");
            }

            if (Config.Parallel) {
                builder.Append(" /parallel");
            }

            if (Config.Pause) {
                builder.Append(" /pause");
            }

            if (Config.Verbose) {
                builder.Append(" /verbose");
            }

            builder.Append(" /out:"+ outputAssemblyPath.FullName.Quote());
            
            // Add primary assembly.
            builder.Append(" "+primaryAssemblyPath.FullName.Quote());

            foreach (var file in assemblyPaths) {
                builder.Append(" "+file.FullName.Quote());
            }


            return builder.ToString();
        }


        private static string GetTargetPlatformString(TargetPlatformVersion version) {
            switch (version) {
                case TargetPlatformVersion.v1:
                    return "v1";
                case TargetPlatformVersion.v11:
                    return "v1.1";
                case TargetPlatformVersion.v2:
                    return "v2";
                case TargetPlatformVersion.v4:
                    return "v4";
                default:
                    throw new NotSupportedException("The provided ILRepack target platform is not valid.");
            }
        }

        private static string GetTargetKindName(TargetKind kind) {
            switch (kind) {
                case TargetKind.Dll:
                    return "library";
                case TargetKind.Exe:
                    return "exe";
                case TargetKind.WinExe:
                    return "winexe";
                default:
                    throw new NotSupportedException("The provided ILRepack target kind is not valid.");
            }
        }

        /// <summary>
        /// 检测path是否存在,如果不存在,会按序依次创建
        /// </summary>
        /// <param name="path">不带文件名的路径</param>
        static void CheckDirectory(string path) {
            //string dir = Path.GetDirectoryName(path);
            if (Directory.Exists(path) || string.IsNullOrEmpty(path) || path.EndsWith(":")) {
                return;
            }
            else {
                string dir = Path.GetDirectoryName(path);
                CheckDirectory(dir);
                Directory.CreateDirectory(path);
            }
        }
    }
}
