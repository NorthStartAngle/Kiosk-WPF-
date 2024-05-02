using AtmUi.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AtmCommon;

namespace AtmUi.Pages {
    /// <summary>
    /// Interaction logic for ConfigurationPage.xaml
    /// </summary>
    public partial class Configuration : Page, IAtmNavigation {
        public Configuration() {
            InitializeComponent();

            /* Show current .Net and OS Versions */
            ShowNetPackages();
            ShowAppModules();
            ShowOSVersion();
        }

        public ViewModels.Configuration ViewModel {
            get => (ViewModels.Configuration)DataContext;
        }

        public static string Path { get; } = $"Pages/Configuration.xaml";
        public static string LinkName { get; } = "Configuration";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        private void ShowNetPackages()
        {
            // Current .net version
            ViewModel.DotNetVersion = Environment.Version.ToString();

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
                var strs = output.Split("\n");
                foreach ( var str in strs )
                {
                    DotNetPackage pckg = new DotNetPackage();
                    var pathStartIndex = str.IndexOf(" [");
                    
                    if (pathStartIndex < 0)
                        continue;

                    pckg.Name = str.Substring(0, pathStartIndex);
                    pckg.Path = str.Substring(pathStartIndex + 2, str.IndexOf("]") - pathStartIndex - 2);
                    packages.Add(pckg);
                }

                ViewModel.NetPackages = packages;
            }
        }

        private void ShowAppModules()
        {
            List<AppModule> modules = new List<AppModule>();
            foreach( ProcessModule module in Process.GetCurrentProcess().Modules )
            {
                modules.Add(new AppModule {
                    Name = module.ModuleName,
                    Path = module.FileName,
                    MemorySize = $"{module.ModuleMemorySize / 1024} KB",
                    Site = ( module.Site != null ) ? module.Site.Name! : ""
                });
            }

            modules.Sort();
            ViewModel.AppModules = modules;
        }

        private void ShowOSVersion()
        {
            ViewModel.OSVersion = Environment.OSVersion.ToString();
        }

        private void OnInstallDotNetClick(object sender, RoutedEventArgs e)
        {
            // ./dotnet-install.ps1 -Runtime dotnet -Version 6.0.0
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = $"'{ViewModel.InstallDotNetFile} -Runtime dotnet -Version {ViewModel.TargetDotNetVersion}'";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            if (output.Contains("StringToBeVerifiedInAUnitTest") )
            {
                ViewModel.InstallError = output;
                // ViewModel.InstallError_Visibility = Visibility.Visible;
            }
            else if ( string.IsNullOrEmpty( errors ) == false )
            {
                ViewModel.InstallError = errors;
                // ViewModel.InstallError_Visibility = Visibility.Visible;
            }
            else
            {
                ViewModel.InstallError = $".Net {ViewModel.TargetDotNetVersion} has been successfully installed on your device.";
                // ViewModel.InstallError_Visibility = Visibility.Visible;
            }
        }
    }
}
