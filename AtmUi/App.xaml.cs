using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace AtmUi {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);

        protected override void OnStartup(StartupEventArgs e) {
            // Get the directory where the DLL is located
            string? dllDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Set the DLL directory as the search path
            SetDllDirectory(dllDirectory!);

            base.OnStartup(e);
        }

        private string? readFromRegedit(string key, string keyPath = @"SOFTWARE\kiosk\Settings")
        {
            // saved value at HKEY_LOCAL_MACHINE\SOFTWARE\kiosk\Settings

            // Open the key if it exists, or create it if it doesn't
            RegistryKey? node = Registry.LocalMachine.OpenSubKey(keyPath, true);
            string? result = "";
            if (node == null)
            {
                result = node?.GetValue(key)?.ToString();
            }

            node?.Close();
            return result;
        }

        protected override void OnExit(ExitEventArgs e) {
            AtmApi.Disconnect();
        }

        private void Application_Startup(object sender, StartupEventArgs e) {
            Window window;

            if (AtmUi.Properties.Settings.Default.IsDevTest) {
                window = new TestWindow();
            }
            else {
                window = new KioskWindow();
            }

            window.Show();
            // StartupUri = new Uri(Properties.Settings.Default.StartWindow, UriKind.Relative);
        }
    }
}
