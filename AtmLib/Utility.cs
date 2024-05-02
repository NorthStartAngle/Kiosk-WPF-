using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AtmLib {
    public class Utility {
        public static void LogException(Exception ex) {
            WriteToLog(ex);
        }

        public static void LogExceptionAndRethrow(Exception ex) {
            WriteToLog(ex);
            throw ex;
        }

        private static void WriteToLog(Exception ex) {
            Trace.TraceError($"\n{ex.GetType().ToString()}\n{ex.Message}\n{ex.StackTrace}");
        }
    }
}
