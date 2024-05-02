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
    public partial class CheckDotNet : Page {
        public CheckDotNet() {
            using (var log = new Logger(TraceLevel.Verbose, "CheckDotNet.CheckDotNet"))
            {
                InitializeComponent();
                Refresh();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        public ViewModels.CheckDotNet ViewModel {
            get => (ViewModels.CheckDotNet)DataContext;
        }

        public static string Path { get; } = $"Pages/CheckDotNet.xaml";
        public static string LinkName { get; } = "CheckDotNet";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        private void Refresh()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckDotNet.Refresh"))
            {
                var checkResult = Requirement.CheckDotNet();

                ViewModel.Message = checkResult.Description;
                ViewModel.Message_Foreground = checkResult.IsChecked ? "white" : "red";

                ViewModel.InstallButton_Visibility = checkResult.IsChecked ? Visibility.Hidden : Visibility.Visible;

                /* Show current .Net and OS Versions */
                ShowNetPackages();

                log.WriteLine(TraceLevel.Verbose, checkResult.Description);
            }
        }

        private void ShowNetPackages()
        {
            // Available .net packages
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            // startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd";
            startInfo.Arguments = "/c dotnet --list-runtimes";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            if ( !string.IsNullOrEmpty(output ) )
            {
                List<DotNetPackage> packages = new List<DotNetPackage>();
                var strs = output.Split('\n');
                foreach ( var str in strs )
                {
                    DotNetPackage pckg = new DotNetPackage();
                    var pathStartIndex = str.IndexOf(" [");
                    
                    if (pathStartIndex < 0)
                        continue;

                    pckg.Name = str.Substring(0, pathStartIndex);
                    pckg.Path = str.Substring(pathStartIndex + 2, str.IndexOf("]") - pathStartIndex - 2);

                    var subStrs = pckg.Name.Split(' ');
                    pckg.Name = subStrs[0];
                    pckg.Ver = new Version(subStrs[1]);

                    packages.Add(pckg);
                }

                ViewModel.NetPackages = packages;
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckDotNet.InstallButton_Click",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {
                ViewModel.DialogVisibility = Visibility.Visible;
                ViewModel.DialogScript = $"{Requirement.InstallDotNetFile} -Runtime dotnet -Version {Requirement.TargetDotNetVersion}";
            }
            /*
            if ( Requirement.InstallDotNet() )
            {
                ViewModel.InstallError = ".Net installed successfully.";
            }
            else
            {
                ViewModel.InstallError = ".Net install failed.";
            }
            
            ViewModel.InstallError_Visibility = Visibility.Visible;
            Refresh();
            */
        }


        public static RoutedCommand OkButton_Command = new RoutedCommand();

        public void OkButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckDotNet.OkButton_Executed",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {
                if (Requirement.InstallDotNet())
                {
                    ViewModel.InstallError = ".Net installed successfully.";
                    log.WriteLine(TraceLevel.Verbose, ViewModel.InstallError);
                }
                else
                {
                    ViewModel.InstallError = ".Net install failed.";
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
