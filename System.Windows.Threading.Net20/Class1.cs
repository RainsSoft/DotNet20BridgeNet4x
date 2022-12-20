using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace System.Windows.Threading.Net20
{
    public static class SafeAction
    {
        //private static readonly Logger Log = GlobalLogger.GetLogger("SafeAction");

        public static ThreadStart Wrap(ThreadStart action){//, [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) {
            return () => {
                try {
                    action();
                }
                catch (ThreadAbortException) {
                    // Ignore this exception
                }
                catch (Exception e) {
                    //Log.Fatal("Unexpected exception", e, CallerInfo.Get(sourceFilePath, memberName, sourceLineNumber));
                    throw;
                }
            };
        }

        public static ParameterizedThreadStart Wrap(ParameterizedThreadStart action){//, [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) {
            return obj => {
                try {
                    action(obj);
                }
                catch (ThreadAbortException) {
                    // Ignore this exception
                }
                catch (Exception e) {
                    //Log.Fatal("Unexpected exception", e, CallerInfo.Get(sourceFilePath, memberName, sourceLineNumber));
                    throw;
                }
            };
        }
    }
}
