using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNet20BridgeNet4x.ILMerge
{
    public class ILMergeCmdBuilder
    {
        public ILMergeCmdBuilder(ILMergeConfig config) {
            this.Config = config;
        }
        public ILMergeConfig Config {
            get;
            set;
        }
        public string BuilderCmd(FileInfo outputAssemblyPath, FileInfo primaryAssemblyPath,
            IEnumerable<FileInfo> assemblyPaths) {
            if (Config == null) {
                return "";
            }
            if (Config.ILMergeExeFile == null) {
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
            builder.AppendLine("cd " + Config.ILMergeExeFile.DirectoryName);
            int index = Config.ILMergeExeFile.DirectoryName.IndexOf(':');
            builder.AppendLine(Config.ILMergeExeFile.DirectoryName.Substring(0, index + 1));
            builder.AppendLine("echo %cd%");
            builder.AppendLine("echo ------------------------------------");
            builder.Append(Config.ILMergeExeFile.Name);
            //                
            builder.Append(" /log:" + Path.Combine(outPath, "ILMerge_MergeDLL.log").Quote());
            if (Config.TargetKind != TargetKind.Default) {
                builder.Append(GetTargetKindParameter(Config));
            }
            if (Config.Internalize) {
                builder.Append(" /internalize");
            }

            builder.Append(" /ndebug");

            builder.Append(GetOutputParameter(outputAssemblyPath));
            if (Config.TargetPlatform != null) {
                builder.Append(GetTargetPlatformParameter(Config));
            }
            // Add primary assembly.
            builder.Append(primaryAssemblyPath.FullName.Quote());

            foreach (var file in assemblyPaths) {
                builder.Append(" "+file.FullName.Quote());
            }

            return builder.ToString();

        }

        private string GetOutputParameter(FileInfo outputAssemblyPath) {
            var path = outputAssemblyPath;
            return string.Concat(" /out:", path.FullName.Quote());
        }

        private static string GetTargetKindParameter(ILMergeConfig settings) {
            return string.Concat(" /target:", GetTargetKindName(settings.TargetKind).Quote());
        }

        private static string GetTargetPlatformParameter(ILMergeConfig settings) {
            var result = new List<string>();
            result.Add(GetTargetPlatformString(settings.TargetPlatform.Platform));
            if (settings.TargetPlatform.Path != null) {
                result.Add(settings.TargetPlatform.Path.FullName.Quote());
            }
            return string.Concat(" /targetPlatform:", string.Join(",", result.ToArray()));
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
                    throw new NotSupportedException("The provided ILMerge target platform is not valid.");
            }
        }

        private static string GetTargetKindName(TargetKind kind) {
            switch (kind) {
                case TargetKind.Dll:
                    return "dll";
                case TargetKind.Exe:
                    return "exe";
                case TargetKind.WinExe:
                    return "winexe";
                default:
                    throw new NotSupportedException("The provided ILMerge target kind is not valid.");
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
