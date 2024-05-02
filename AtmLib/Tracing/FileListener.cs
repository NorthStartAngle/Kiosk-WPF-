using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace AtmLib.Tracing {
    public class FileListener : TraceListener {
        private static readonly object _lockObj = new object();

        public FileListener() {
        }

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        private static Queue<TraceItem> _traceItems = new Queue<TraceItem>();

        public override void Write(object o) {
            if (o is TraceItem t) {
                lock (_lockObj) {
                    _traceItems.Enqueue(t);
                    return;
                }
            }

            base.WriteLine(o);
        }

        public override void Write(string message) {
            lock (_lockObj) {
                WriteToFile(message);
            }
        }

        public override void WriteLine(object o) {
            if (o is TraceItem t) {
                lock (_lockObj) {
                    _traceItems.Enqueue(t);
                    Flush();
                    return;
                }
            }

            base.WriteLine(o);
        }

        public override void WriteLine(string message) {
            lock (_lockObj) {
                WriteToFile($"{message}{Environment.NewLine}");
            }
        }

        private void EmptyQueue() {
            lock (_lockObj) {
                if (_traceItems.Count > 0 ) {
                    var t = _traceItems.Dequeue();
                    WriteToFile($"{t.TimeStamp.ToString("yyyy-MM-ddTHH:mm:ss.fff")} {t.Level.ToString().PadRight(7)} {t.ThreadId.ToString("X8")}: {new string(' ', Trace.IndentSize * Trace.IndentLevel)}{t.Message}");

                    while (_traceItems.Count > 0) {
                        WriteToFile($"{_traceItems.Dequeue().Message}");
                    }

                    WriteToFile($"{Environment.NewLine}");
                }
            }
        }

        public override void Flush() {
            EmptyQueue();
            base.Flush();
        }

        private void WriteToFile(string message) {
            lock (_lockObj) {
                File.AppendAllText(_traceFilePath, message);
            }
        }

        private static long _maxFileSize = 1024 * 1024 * 5; // ~5 MB default
        
        public static long MaxFileSize { 
            get => _maxFileSize; 
            set { 
                lock (_lockObj) {
                    _maxFileSize = value;
                }
            }
        }

        private static string _traceFileName = "AtmTrace.log";

        public static string TraceFileName {
            get => _traceFileName;
            set {
                lock (_lockObj) {
                    _traceFileName = value;
                    _traceFilePath = Path.Combine(_traceFileDir, _traceFileName);
                }
            }
        }

        private static string _traceFileDir = $"{System.IO.Path.GetTempPath()}";

        public static string TraceFileDir {
            get => _traceFileDir;
            set {
                lock (_lockObj) { 
                    _traceFileDir = value;
                    _traceFilePath = Path.Combine(_traceFileDir, _traceFileName);
                }
            }
        }

        private static string _traceFilePath = Path.Combine(_traceFileDir, _traceFileName);

        private static FileSystemWatcher _fileWatcher;

        public static void StartMonitoring() {
            _fileWatcher = new FileSystemWatcher {
                Path = TraceFileDir,
                Filter = TraceFileName,
                NotifyFilter = NotifyFilters.Size
            };

            _fileWatcher.Changed += OnChanged;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object sender, FileSystemEventArgs e) {
            if (e.ChangeType == WatcherChangeTypes.Changed) {
                FileInfo fileInfo = new FileInfo(e.FullPath);

                if (fileInfo.Length > MaxFileSize) {
                    lock (_lockObj) {
                        var newName = $"{DateTime.UtcNow.ToString("s").Replace(':','-')}.{_traceFileName}";
                        var newPath = Path.Combine(_traceFileDir, newName);
                        File.Copy(fileInfo.FullName, newPath, true);
                    }
                }
            }
        }

        public static void StopMonitoring() {
            if (_fileWatcher != null) {
                _fileWatcher.EnableRaisingEvents = false;
                _fileWatcher.Dispose();
            }
        }
    }
}
