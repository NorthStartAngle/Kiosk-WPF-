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
    public partial class CheckLibrary : Page {
        public CheckLibrary() {
            using (var log = new Logger(TraceLevel.Verbose, "CheckLibrary.CheckLibrary"))
            {
                InitializeComponent();
                Refresh();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        public ViewModels.CheckLibrary ViewModel {
            get => (ViewModels.CheckLibrary)DataContext;
        }

        public static string Path { get; } = $"Pages/CheckLibrary.xaml";
        public static string LinkName { get; } = "CheckLibrary";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        private void Refresh()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckLibrary.Refresh"))
            {
                var checkResult = Requirement.CheckLibrary();

                ViewModel.Message = checkResult.Description;
                ViewModel.Message_Foreground = checkResult.IsChecked ? "white" : "red";

                ViewModel.InstallButton_Visibility = Visibility.Hidden;
                ViewModel.Details = (List<AppModule>)checkResult.Details;

                log.WriteLine(TraceLevel.Verbose, checkResult.Description);
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckLibrary.InstallButton_Click",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {
                ViewModel.InstallError_Visibility = Visibility.Visible;
                Refresh();
            }
        }
    }
}
