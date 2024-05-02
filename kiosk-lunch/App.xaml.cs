using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace kiosk_lunch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { 
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);

        protected override void OnStartup(StartupEventArgs e)
        {
            // Get the directory where the DLL is located
            string? dllDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Set the DLL directory as the search path
            SetDllDirectory(dllDirectory!);

            base.OnStartup(e);
        }

        private void AppStartup(object sender, StartupEventArgs e)
        {

        }
    }
}
