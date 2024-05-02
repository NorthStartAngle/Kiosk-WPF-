using AtmLib.CheckList;
using AtmLib.Monitor;
using AtmLib.Tracing;
using AtmLoader.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AtmLoader.Pages {
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class CheckDevice : Page {
        public CheckDevice() {
            using (var log = new Logger(TraceLevel.Verbose, "CheckDevice.CheckDevice"))
            {
                InitializeComponent();
                Refresh();
            }
        }

        public ViewModels.CheckDevice ViewModel {
            get => (ViewModels.CheckDevice)DataContext;
        }

        public static string Path { get; } = $"Pages/CheckDevice.xaml";
        public static string LinkName { get; } = "CheckDevice";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        private void Refresh()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckDevice.Refresh"))
            {
                var checkResult = Requirement.CheckDevice();

                ViewModel.Message = checkResult.Description;
                ViewModel.Message_Foreground = checkResult.IsChecked ? "white" : "red";

                ViewModel.InstallButton_Visibility = checkResult.IsChecked ? Visibility.Hidden : Visibility.Visible;
                ViewModel.Details = (List<Device>)checkResult.Details;

                log.WriteLine( TraceLevel.Verbose, checkResult.Description);
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckDevice.InstallButton_Click",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {
                if (Requirement.InstallDotNet())
                {
                    ViewModel.InstallError = "Installed successfully.";
                }
                else
                {
                    ViewModel.InstallError = "Install failed.";
                }

                ViewModel.InstallError_Visibility = Visibility.Visible;
                Refresh();
            }
        }
    }
}
