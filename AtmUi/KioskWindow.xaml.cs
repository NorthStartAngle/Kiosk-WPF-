using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AtmUi {
    /// <summary>
    /// Interaction logic for KioskWindow.xaml
    /// </summary>
    public partial class KioskWindow : Window {
        public KioskWindow() {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //writeToRegedit("Shell", "explorer", @"Microsoft\Windows NT\CurrentVersion\Winlogon");// HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon
        }
        private void writeToRegedit(string key, string value, string keyPath = @"SOFTWARE\kiosk\Settings", bool overwrite = true)
        {
            // saved value at HKEY_LOCAL_MACHINE\SOFTWARE\kiosk\Settings

            // Open the key if it exists, or create it if it doesn't
            RegistryKey? node;
            if (overwrite)
            {
                node = Registry.LocalMachine.OpenSubKey(keyPath, true) ?? Registry.LocalMachine.CreateSubKey(keyPath);
            }
            else
            {
                node = Registry.LocalMachine.OpenSubKey(keyPath, true);
            }


            // Set a value in the key
            node?.SetValue(key, value);

            // Close the key
            node?.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            if (!Topmost) {
                var result = MessageBox.Show(
                    "Warning: The application window is not top-most. This is a security risk. To test the application in a non-topmost window, set the IsDevTest setting in App.config to 'True'.\nDo you want to continue running the application?",
                    "Warning",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Cancel) {
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
