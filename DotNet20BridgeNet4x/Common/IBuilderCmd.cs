using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNet20BridgeNet4x
{
    public interface IBuilderCmd
    {
       
        /// <summary>
        /// 构建命令
        /// </summary>
        /// <returns></returns>
        string BuilderCmd(FileInfo outputAssemblyPath, FileInfo primaryAssemblyPath,
            IEnumerable<FileInfo> assemblyPaths);

    }
}
