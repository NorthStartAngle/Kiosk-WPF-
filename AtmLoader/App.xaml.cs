using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
// using System.Windows.Shapes;

using AtmCommon;

using AtmLib.Tracing;
using AtmLoader.Core;

namespace AtmLoader {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application {
        public App() {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            var exception = e.ExceptionObject as Exception;
            if (exception != null) {
                Trace.WriteLine($"Unhandled exception\n{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}");
                Logger.LogAbnormalExit(TraceLevel.Off, "AtmLoader");
                AtmLib.Tracing.FileListener.StopMonitoring();
                Environment.Exit( 1 );
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        protected override void OnStartup(StartupEventArgs e) {
            Trace.IndentSize = 2;
            var listener = new AtmLib.Tracing.FileListener();
            Trace.Listeners.Add(listener);

            string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Merapi", "loader");

            if (!Directory.Exists(logPath)) {
                Directory.CreateDirectory(logPath);
            }

            AtmLib.Tracing.FileListener.TraceFileDir = logPath;
            AtmLib.Tracing.FileListener.TraceFileName = "AtmLoader.log";

            AtmLib.Tracing.FileListener.StartMonitoring();
            Logger.LogEntry(TraceLevel.Off, "AtmLoader");

            // Get the directory where the DLL is located
            string dllDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Set the DLL directory as the search path
            SetDllDirectory(dllDirectory);

            // Init camera capturing
            Camera.Init();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e) {
            // AtmApi.Disconnect();
            Logger.LogExit(TraceLevel.Off, "AtmLoader");
            AtmLib.Tracing.FileListener.StopMonitoring();

            // Finish capturing camera images
            Camera.Destroy();
        }

        private void Application_Startup(object sender, StartupEventArgs e) {
            using (var log = new Logger(TraceLevel.Verbose, "App.Application_Startup")) {
                Window window;

                if (AtmLoader.Properties.Settings.Default.IsDevTest) {
                    log.WriteLine(TraceLevel.Info, "Using TestWindow");
                    window = new TestWindow();
                }
                else {
                    log.WriteLine(TraceLevel.Info, "Using KioskWindow");
                    window = new KioskWindow();
                }

                window.Show();
            }
        }
    }
}
