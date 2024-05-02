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
    public partial class CheckMemory : Page {
        public CheckMemory() {
            using (var log = new Logger(TraceLevel.Verbose, "CheckMemory.CheckMemory"))
            {
                InitializeComponent();
                Refresh();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        public ViewModels.CheckMemory ViewModel {
            get => (ViewModels.CheckMemory)DataContext;
        }

        public static string Path { get; } = $"Pages/CheckMemory.xaml";
        public static string LinkName { get; } = "CheckMemory";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        private void Refresh()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckMemory.Refresh"))
            {
                var checkResult = Requirement.CheckMemory();

                ViewModel.Message = checkResult.Description;
                ViewModel.Message_Foreground = checkResult.IsChecked ? "white" : "red";

                ViewModel.InstallButton_Visibility = checkResult.IsChecked ? Visibility.Hidden : Visibility.Visible;
                ViewModel.Details = (List<KeyValuePair<string, object>>)checkResult.Details;

                log.WriteLine(TraceLevel.Verbose, checkResult.Description);
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
