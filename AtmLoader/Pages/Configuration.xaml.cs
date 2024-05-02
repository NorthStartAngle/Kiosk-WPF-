using AtmLib.CheckList;
using AtmLib.Tracing;
using AtmLoader.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Packaging;
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
using System.Windows.Threading;

namespace AtmLoader.Pages {
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Page {
        public Configuration() {
            using (var log = new Logger(TraceLevel.Verbose, "Configuration.Configuration"))
            {
                InitializeComponent();

                /* Show current .Net and OS Versions */
                ShowNetPackages();
                ShowAppModules();

                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                using (var log = new Logger(TraceLevel.Verbose, "Configuration.Page_Loaded",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
                {
                    CheckAppInfos();
                }
            }), DispatcherPriority.Background);
        }

        public void CheckAppInfos()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Configuration.CheckAppInfos"))
            {
                CheckApp appChecker = new CheckApp();

                List<CheckAppItem> appItems = new List<CheckAppItem>();
                appItems.Add(appChecker.Check("AtmLoader"));
                appItems.Add(appChecker.Check("KioskWatcher"));
                appItems.Add(appChecker.Check("AtmUi"));

                ViewModel.AppInfos = appItems;

                log.WriteLine(TraceLevel.Verbose, $"AtmLoader: {appItems[0].Result}");
                log.WriteLine(TraceLevel.Verbose, $"KioskWatcher: {appItems[1].Result}");
                log.WriteLine(TraceLevel.Verbose, $"AtmUi: {appItems[2].Result}");
            }
        }

        private void ShowNetPackages()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Configuration.ShowNetPackages"))
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
                if (!string.IsNullOrEmpty(output))
                {
                    List<DotNetPackage> packages = new List<DotNetPackage>();
                    var strs = output.Split('\n');
                    foreach (var str in strs)
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

                    log.WriteLine(TraceLevel.Verbose, $"{packages.Count} packages found.");
                }
                else
                    log.WriteLine(TraceLevel.Warning, $"No packages found.");
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
                    MemorySize = $"{ Requirement.SizeToString( module.ModuleMemorySize )}",
                    Site = ( module.Site != null ) ? module.Site.Name : ""
                });
            }

            modules.Sort();
            ViewModel.AppModules = modules;
        }

            /*
        private void ShowOSVersion()
        {
            CheckResult checkResult = Requirement.CheckOS();
            ViewModel.OSMessage = checkResult.Description;
        }
            */

        private void OnAppListButton_Click(object sender, RoutedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "Configuration.OnAppListButton_Click",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {
                var btn = (Button)sender;
                var tag = btn.Tag.ToString();

                switch (tag)
                {
                    case "AtmLoader":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckDotNet.Path);
                        break;
                }

                log.WriteLine(TraceLevel.Verbose, $"Navigate to {tag}.");
            }
        }
    }
}
