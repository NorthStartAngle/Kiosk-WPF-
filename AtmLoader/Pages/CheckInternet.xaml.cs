using AtmLib.CheckList;
using AtmLib.Tracing;
using AtmLoader.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class CheckInternet : Page
    {

        BackgroundWorker _internetChecker = null;
        public CheckInternet() {
            using (var log = new Logger(TraceLevel.Verbose, "CheckInternet.CheckInternet"))
            {
                InitializeComponent();
                Refresh();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        public ViewModels.CheckInternet ViewModel {
            get => (ViewModels.CheckInternet)DataContext;
        }

        public static string Path { get; } = $"Pages/CheckInternet.xaml";
        public static string LinkName { get; } = "CheckInternet";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        private void Refresh()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckInternet.Refresh"))
            {
                _internetChecker = new BackgroundWorker();
                _internetChecker.DoWork += InternetChecker_DoWork;
                _internetChecker.RunWorkerCompleted += InternetChecker_DoneWork;
                _internetChecker.RunWorkerAsync();

                ViewModel.Message = "Checking...";
                ViewModel.Message_Foreground = "white";
                ViewModel.InstallButton_Visibility = Visibility.Hidden;

                log.WriteLine(TraceLevel.Verbose, "Checking started.");
            }
        }


        private void InternetChecker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckInternet.InternetChecker_DoWork",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {
                CheckResult checkResult = Requirement.CheckInternet();
                e.Result = checkResult;
            }
        }

        private void InternetChecker_DoneWork(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckInternet.InternetChecker_DoneWork",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {
                CheckResult checkResult = (CheckResult)e.Result;

                ViewModel.Message = checkResult.Description;
                ViewModel.Message_Foreground = checkResult.IsChecked ? "white" : "red";

                ViewModel.InstallButton_Visibility = Visibility.Hidden;

                CheckInternetResult result = (CheckInternetResult)checkResult.Details;
                ViewModel.NetworkInterfaces = result.NetworkInterfaces;
                ViewModel.IPAddresses = result.IPAddresses;
                ViewModel.ServerName = result.ServerHostName;
                ViewModel.ServerStatus = result.ServerStatus;

                log.WriteLine(TraceLevel.Verbose, checkResult.Description);
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
