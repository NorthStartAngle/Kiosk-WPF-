using AtmLib.CheckList;
using AtmLib.Tracing;
using AtmLoader.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class CheckOS : Page {
        public CheckOS() {
            using (var log = new Logger(TraceLevel.Verbose, "CheckOS.CheckOS"))
            {
                InitializeComponent();
                Refresh();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        public ViewModels.CheckOS ViewModel {
            get => (ViewModels.CheckOS)DataContext;
        }

        public static string Path { get; } = $"Pages/CheckOS.xaml";
        public static string LinkName { get; } = "CheckOS";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        private void Refresh()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckOS.Refresh"))
            {
                var checkResult = Requirement.CheckOS();

                ViewModel.Message = checkResult.Description;
                ViewModel.Message_Foreground = checkResult.IsChecked ? "white" : "red";

                ViewModel.InstallButton_Visibility = Visibility.Visible; // checkResult.IsChecked ? Visibility.Hidden : Visibility.Visible;
                ViewModel.Details = (List<KeyValuePair<string, object>>)checkResult.Details;

                log.WriteLine(TraceLevel.Verbose, checkResult.Description);
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DialogScript = Requirement.UpdateOSPS;
            ViewModel.DialogVisibility = Visibility.Visible;
        }

        public static RoutedCommand OkButton_Command = new RoutedCommand();

        public void OkButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckOS.OkButton_Executed",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {
                if (Requirement.UpdateOS())
                {
                    ViewModel.InstallError = "Updated successfully.";
                    log.WriteLine(TraceLevel.Verbose, ViewModel.InstallError);
                }
                else
                {
                    ViewModel.InstallError = "Update failed.";
                    log.WriteLine(TraceLevel.Error, ViewModel.InstallError);
                }

                ViewModel.InstallError_Visibility = Visibility.Visible;
                Refresh();

                ViewModel.DialogVisibility = Visibility.Hidden;
            }
        }

        public void OkButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static RoutedCommand CancelButton_Command = new RoutedCommand();

        public void CancelButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.DialogVisibility = Visibility.Hidden;
        }

        public void CancelButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
