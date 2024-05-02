using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace AtmLib.Tracing {
    public class Logger : IDisposable {
        private static TraceSwitch traceConfig = new TraceSwitch("traceSwitch", "Switch in config file");

        public Logger(TraceLevel traceLevel, string scopeName) {
            _scopeName = scopeName;
            _traceLevel = traceLevel;
            Trace.Flush();

            if (_traceLevel <= traceConfig.Level) {
                Trace.WriteLine(new TraceItem {
                    Message = $"{_scopeName} () {{ ",
                    Level = _traceLevel
                });
            }

            Indent();
        }

        public Logger(TraceLevel traceLevel, string scopeName, string[] scopeValueNames, object[] scopeValues) {
            _scopeName = scopeName;
            _scopeValueNames = scopeValueNames;
            _scopeValues = scopeValues;
            _traceLevel = traceLevel;
            Trace.Flush();

            if (_traceLevel <= traceConfig.Level) {
                Trace.WriteLine(new TraceItem {
                    Message = $"{_scopeName} ({GetScopeValues()}) {{ ",
                    Level = _traceLevel
                });
            }

            Indent();
        }

        private readonly string _scopeName;
        private readonly string[] _scopeValueNames = null;
        private readonly object[] _scopeValues = null;

        private TraceLevel _traceLevel;

        //
        // Summary:
        //     Increases the current System.Diagnostics.Trace.IndentLevel by one.
        [Conditional("TRACE")]
        private static void Indent() => Trace.Indent();

        //
        // Summary:
        //     Decreases the current System.Diagnostics.Trace.IndentLevel by one.
        [Conditional("TRACE")]
        private static void Unindent() => Trace.Unindent();

        //
        // Summary:
        //     Gets or sets the indent level.
        //
        // Returns:
        //     The indent level. The default is zero.
        private static int IndentLevel {
            get => Trace.IndentLevel;
            set => Trace.IndentLevel = value;
        }

        //
        // Summary:
        //     Gets or sets the number of spaces in an indent.
        //
        // Returns:
        //     The number of spaces in an indent. The default is four.
        private static int IndentSize { 
            get => Trace.IndentSize; 
            set => Trace.IndentSize = value;  
        }

        private string GetScopeValues() {
            if ( _scopeValues == null ) {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _scopeValueNames.Length; i++) {
                sb.Append($"{_scopeValueNames[i]}: {_scopeValues[i]?.ToString() ?? "null"}, ");
            }
            
            return sb.ToString().TrimEnd(',', ' ');
        }

        public static void WriteException(Exception ex) {
            Trace.WriteLine(new TraceItem { 
                Message = $"{ex.GetType().ToString()} {ex.Message} {ex.StackTrace}"
            });
        }

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        public static void LogEntry(TraceLevel traceLevel, string name) {
            Trace.WriteLine(new TraceItem {
                Message = $"+++ {name?.ToString()} Normal Application Start +++",
                Level = traceLevel
            });
        }

        public static void LogExit(TraceLevel traceLevel, string name) {
            Trace.WriteLine(new TraceItem {
                Message = $"--- {name?.ToString()} Normal Application Exit ---",
                Level = traceLevel
            });
        }

        public static void LogAbnormalExit(TraceLevel traceLevel, string name) {
            Trace.WriteLine(new TraceItem {
                Message = $"!!! {name?.ToString()} Abnormal Application Exit !!!",
                Level = traceLevel
            });
        }

        public static void Write(Exception ex) => WriteException(ex);

        //
        // Summary:
        //     Writes a message to the trace listeners in the System.Diagnostics.Trace.Listeners
        //     collection.
        //
        // Parameters:
        //   message:
        //     A message to write.
        [Conditional("TRACE")]
        public void WriteLine(TraceLevel traceLevel, string message) {
            if (traceLevel > traceConfig.Level) {
                return;
            }

            Trace.WriteLine(new TraceItem { 
                Message = message,
                Level = traceLevel
            });
        }

        //
        // Summary:
        //     Writes the value of the object's System.Object.ToString method to the trace listeners
        //     in the System.Diagnostics.Trace.Listeners collection.
        //
        // Parameters:
        //   value:
        //     An System.Object whose name is sent to the System.Diagnostics.Trace.Listeners.
        [Conditional("TRACE")]
        public void WriteLine(TraceLevel traceLevel, object value) => WriteLine(traceLevel, value.ToString());

        //
        // Summary:
        //     Writes a message to the trace listeners in the System.Diagnostics.Trace.Listeners
        //     collection.
        //
        // Parameters:
        //   message:
        //     A message to write.
        [Conditional("TRACE")]
        public void Write(TraceLevel traceLevel, string message) {
            if (traceLevel > traceConfig.Level) {
                return;
            }

            Trace.Write(new TraceItem { 
                Message = message,
                Level = traceLevel
            });
        }

        //
        // Summary:
        //     Writes the value of the object's System.Object.ToString method to the trace listeners
        //     in the System.Diagnostics.Trace.Listeners collection.
        //
        // Parameters:
        //   value:
        //     An System.Object whose name is sent to the System.Diagnostics.Trace.Listeners.
        [Conditional("TRACE")]
        public void Write(TraceLevel traceLevel, object value) => Write(traceLevel, value.ToString());

        //
        // Summary:
        //     Writes a message to the trace listeners in the System.Diagnostics.Trace.Listeners
        //     collection if a condition is true.
        //
        // Parameters:
        //   condition:
        //     true to cause a message to be written; otherwise, false.
        //
        //   message:
        //     A message to write.
        [Conditional("TRACE")]
        public void WriteLineIf(TraceLevel traceLevel, bool condition, string message) {
            if (traceLevel > traceConfig.Level) {
                return;
            }

            if (condition) {
                WriteLine(traceLevel, message);
            }
        }

        //
        // Summary:
        //     Writes the value of the object's System.Object.ToString method to the trace listeners
        //     in the System.Diagnostics.Trace.Listeners collection if a condition is true.
        //
        // Parameters:
        //   condition:
        //     true to cause a message to be written; otherwise, false.
        //
        //   value:
        //     An System.Object whose name is sent to the System.Diagnostics.Trace.Listeners.
        [Conditional("TRACE")]
        public void WriteLineIf(TraceLevel traceLevel, bool condition, object value) => WriteLineIf(traceLevel, condition, value.ToString());

        //
        // Summary:
        //     Writes a message to the trace listeners in the System.Diagnostics.Trace.Listeners
        //     collection if a condition is true.
        //
        // Parameters:
        //   condition:
        //     true to cause a message to be written; otherwise, false.
        //
        //   message:
        //     A message to write.
        [Conditional("TRACE")]
        public void WriteIf(TraceLevel traceLevel, bool condition, string message) {
            if (traceLevel > traceConfig.Level) {
                return;
            }

            if (condition) {
                Write(traceLevel, message);
            }
        }

        //
        // Summary:
        //     Writes the value of the object's System.Object.ToString method to the trace listeners
        //     in the System.Diagnostics.Trace.Listeners collection if a condition is true.
        //
        // Parameters:
        //   condition:
        //     true to cause a message to be written; otherwise, false.
        //
        //   value:
        //     An System.Object whose name is sent to the System.Diagnostics.Trace.Listeners.
        [Conditional("TRACE")]
        public void WriteIf(TraceLevel traceLevel, bool condition, object value) => WriteIf(traceLevel, condition, value.ToString());

        //
        // Summary:
        //     Flushes the output buffer, and causes buffered data to be written to the System.Diagnostics.Trace.Listeners.
        [Conditional("TRACE")]
        public void Flush() => Trace.Flush();

        public void Dispose() {
            Unindent();
            if (_traceLevel <= traceConfig.Level) {  
                Trace.WriteLine(new TraceItem {
                    Message = $"}} // {_scopeName}",
                    Level = _traceLevel
                });
            }
        }
    }
}
