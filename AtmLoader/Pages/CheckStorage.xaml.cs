using AtmLib.CheckList;
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
    public partial class CheckStorage: Page {
        public CheckStorage() {
            using (var log = new Logger(TraceLevel.Verbose, "CheckStorage.CheckStorage"))
            {
                InitializeComponent();
                Refresh();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        public ViewModels.CheckStorage ViewModel {
            get => (ViewModels.CheckStorage) DataContext;
        }

        public static string Path { get; } = $"Pages/CheckStorage.xaml";
        public static string LinkName { get; } = "CheckStorage";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        private void Refresh()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckStorage.Refresh"))
            {
                var checkResult = Requirement.CheckStorage();

                ViewModel.Message = checkResult.Description;
                ViewModel.Message_Foreground = checkResult.IsChecked ? "white" : "red";

                ViewModel.InstallButton_Visibility = checkResult.IsChecked ? Visibility.Hidden : Visibility.Visible;
                List<CheckStorageItem> driveItems = new List<CheckStorageItem>();

                foreach (DriveInfo info in (DriveInfo[])checkResult.Details)
                {
                    driveItems.Add(new CheckStorageItem
                    {
                        Name = info.Name,
                        Label = info.VolumeLabel,
                        Type = (info.DriveType == DriveType.Fixed) ? "Local Disk" : info.DriveType.ToString(),
                        Format = info.DriveFormat.ToString(),
                        TotalSize = $"Total Size: {Requirement.SizeToString(info.TotalSize)}",
                        FreeSpace = $"Free Space: {Requirement.SizeToString(info.TotalFreeSpace)}"
                    });
                }

                ViewModel.Details = driveItems;

                log.WriteLine(TraceLevel.Verbose, checkResult.Description );
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
