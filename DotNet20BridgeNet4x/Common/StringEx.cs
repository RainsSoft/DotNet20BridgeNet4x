using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet20BridgeNet4x
{
    public static class StringEx
    {
        /// <summary>
        /// 字符串前后加双引号
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Quote(this string s) {
            return "\"" + s+"\"";
        }
   
    }
}
