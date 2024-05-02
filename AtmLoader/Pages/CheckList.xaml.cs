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
using System.Management;
using Microsoft.Win32;
using AtmLib.CheckList;
using System.Management.Automation.Language;
using System.Management.Automation;
using System.Windows.Media.Media3D;
using System.ComponentModel;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using AtmLib.Tracing;

namespace AtmLoader.Pages
{
    /// <summary>
    /// Interaction logic for CheckListPage.xaml
    /// </summary>
    public partial class CheckList : Page
    {
        public CheckList()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckList"))
            {
                InitializeComponent();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized.");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke(new Action(() => {
                using (var log = new Logger(TraceLevel.Verbose, "CheckList.Page_Loaded",
                    new string[] { nameof(sender), nameof(e) },
                    new object[] { sender, e }))
                {
                    ResetCheckList();
                    log.WriteLine(TraceLevel.Verbose, "Page loaded with checklist.");
                }
            }), DispatcherPriority.Background);
        }

        public ViewModels.CheckList ViewModel
        {
            get => (ViewModels.CheckList)DataContext;
        }

        public void ResetCheckList()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.ResetCheckList"))
            {
                ObservableCollection<CheckItem> items = new ObservableCollection<CheckItem>();

                // .Net
                items.Add(CheckDotNet());

                // OS
                items.Add(CheckOS());

                // Storage
                items.Add(CheckStorage());

                // Memory
                items.Add(CheckMemory());

                // Libraries
                items.Add(CheckLibrary());

                // Resources
                items.Add(CheckResource());

                // Files
                items.Add(CheckFile());

                // Permission
                items.Add(CheckPermission());

                // Internet Connection
                items.Add(CheckInternet());

                // Devices
                items.Add(CheckDevice());

                // Updates
                items.Add(CheckUpdate());

                ViewModel.CheckItems = items;

                log.WriteLine(TraceLevel.Verbose, "Reset check list items.");
            }
        }

        private CheckItem CheckDotNet()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckDotNet"))
            {
                CheckResult checkResult = Requirement.CheckDotNet();

                CheckItem item = new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Button_Visibility = Visibility.Visible,
                    Tag = "DotNet"
                };

                log.WriteLine(TraceLevel.Verbose, "Checking dot net version done.");
                return item;
            }
        }

        private CheckItem CheckOS()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckOS"))
            {
                CheckResult checkResult = Requirement.CheckOS();

                CheckItem item = new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Button_Visibility = Visibility.Visible,
                    Tag = "OS"
                };

                log.WriteLine(TraceLevel.Verbose, "Checking OS done.");
                return item;
            }
        }

        private CheckItem CheckStorage()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckStorage"))
            {
                CheckResult checkResult = Requirement.CheckStorage();
                
                log.WriteLine(TraceLevel.Verbose, "Checking persistent storage done.");

                return new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Tag = "Storage",
                    Button_Visibility = Visibility.Visible
                };
            }
        }

        private CheckItem CheckMemory()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckMemory"))
            {
                CheckResult checkResult = Requirement.CheckMemory();
                log.WriteLine(TraceLevel.Verbose, "Checking memory done.");
                return new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Tag = "Memory",
                    Button_Visibility = Visibility.Visible
                };
            }
        }

        private CheckItem CheckLibrary()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckLibrary"))
            {
                CheckResult checkResult = Requirement.CheckLibrary();
                log.WriteLine(TraceLevel.Verbose, "Checking library done.");
                return new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Tag = "Library",
                    Button_Visibility = Visibility.Visible
                };
            }
        }

        private CheckItem CheckResource()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckResource"))
            {
                CheckResult checkResult = Requirement.CheckResource();
                log.WriteLine(TraceLevel.Verbose, "Checking resource done.");
                return new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Tag = "Resource",
                    Button_Visibility = Visibility.Visible
                };
            }
        }

        private CheckItem CheckFile()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckFile"))
            {
                CheckResult checkResult = Requirement.CheckFile();
                log.WriteLine(TraceLevel.Verbose, "Checking file done.");
                return new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Tag = "File",
                    Button_Visibility = Visibility.Visible
                };
            }
        }

        private CheckItem CheckPermission()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckPermission"))
            {
                CheckResult checkResult = Requirement.CheckPermission();
                log.WriteLine(TraceLevel.Verbose, "Checking permission done.");
                return new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Tag = "Permission",
                    Button_Visibility = Visibility.Visible
                };
            }
        }

        BackgroundWorker _internetChecker = null;

        private CheckItem CheckInternet()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckInternet"))
            {
                _internetChecker = new BackgroundWorker();
                _internetChecker.DoWork += InternetChecker_DoWork;
                _internetChecker.RunWorkerCompleted += InternetChecker_DoneWork;

                _internetChecker.RunWorkerAsync();
                log.WriteLine(TraceLevel.Verbose, "Checking internet done.");

                return new CheckItem
                {
                    Title = "Internet",
                    Description = "Checking...",
                    IconPath = "/res/uncheck.jpg",
                    Tag = "Internet",
                    Button_Visibility = Visibility.Visible
                };
            }
        }

        private void InternetChecker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.InternetChecker_DoWork",
                    new string[] { nameof(sender), nameof(e) },
                    new object[] { sender, e }))
            {
                CheckResult checkResult = Requirement.CheckInternet();
                e.Result = checkResult;
                log.WriteLine(TraceLevel.Verbose, "Internet checker is doing work.");
            }
        }

        private void InternetChecker_DoneWork(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.InternetChecker_DoneWork"))
            {
                CheckResult checkResult = (CheckResult)e.Result;
                ViewModel.UpdateCheckList(checkResult.Title, new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Tag = "Internet",
                    Button_Visibility = Visibility.Visible
                });
                log.WriteLine(TraceLevel.Verbose, "Internet checker has done working.");
            }
        }

        private CheckItem CheckUpdate()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckUpdate"))
            {
                CheckResult checkResult = Requirement.CheckUpdate();
                log.WriteLine(TraceLevel.Verbose, "Checking updates done.");
                return new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Tag = "Update",
                    Button_Visibility = Visibility.Visible
                };
            }
        }

        private CheckItem CheckDevice()
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.CheckDevice"))
            {
                CheckResult checkResult = Requirement.CheckDevice();
                log.WriteLine(TraceLevel.Verbose, "Checking devices done.");
                return new CheckItem
                {
                    Title = checkResult.Title,
                    Description = checkResult.Description,
                    IconPath = checkResult.IsChecked ? "/res/check.png" : "/res/uncheck.jpg",
                    Tag = "Device",
                    Button_Visibility = Visibility.Visible
                };
            }
        }

        private void OnListButton_Click(object sender, RoutedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.OnListButton_Click",
                    new string[] { nameof(sender), nameof(e) },
                    new object[] { sender, e }))
            {
                var btn = (Button)sender;
                var tag = btn.Tag.ToString();

                switch (tag)
                {
                    case "DotNet":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckDotNet.Path);
                        break;

                    case "OS":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckOS.Path);
                        break;

                    case "Storage":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckStorage.Path);
                        break;

                    case "Memory":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckMemory.Path);
                        break;

                    case "Library":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckLibrary.Path);
                        break;

                    case "Resource":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckResource.Path);
                        break;

                    case "File":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckFile.Path);
                        break;

                    case "Permission":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckPermission.Path);
                        break;

                    case "Internet":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckInternet.Path);
                        break;

                    case "Update":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckUpdate.Path);
                        break;

                    case "Device":
                        AtmCommon.Navigator.Navigate(this, Pages.CheckDevice.Path);
                        break;
                }
                log.WriteLine(TraceLevel.Verbose, $"Navigate to {tag} page.");
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.TextBox_GotFocus",
                    new string[] { nameof(sender), nameof(e) },
                    new object[] { sender, e }))
            {
                TouchKeyboard.FocusedControl = sender as Control;
                TouchKeyboard.Visibility = Visibility.Visible;
                e.Handled = true;
            }
        }

        private void Blank_GotFocus(object sender, RoutedEventArgs e)
        {
            using (var log = new Logger(TraceLevel.Verbose, "CheckList.Blank_GotFocus",
                    new string[] { nameof(sender), nameof(e) },
                    new object[] { sender, e }))
            {
                TouchKeyboard.FocusedControl = null;
                TouchKeyboard.Visibility = Visibility.Hidden;
            }
        }

        public static string Path { get; } = $"Pages/CheckList.xaml";
        public static string LinkName { get; } = "CheckList";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e)
        {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }
    }
}
