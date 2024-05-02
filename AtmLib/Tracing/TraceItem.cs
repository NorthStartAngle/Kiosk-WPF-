using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AtmLib.Tracing {
    internal class TraceItem {
        internal TraceItem() {
        }

        internal TraceItem(TraceLevel traceLevel, string message) {
            Message = message;
            Level = traceLevel;
        }

        internal DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        internal uint ThreadId { get; set; } = GetCurrentThreadId();
        internal string Message { get; set; } = string.Empty;
        internal TraceLevel Level { get; set; } = TraceLevel.Off;

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        public override string ToString() {
            return $"{TimeStamp.ToString("yyyy-MM-ddTHH:mm:ss.fff")} {Level.ToString().PadRight(7)} {ThreadId.ToString("X8")}: {Message}";
        }
    }
}
