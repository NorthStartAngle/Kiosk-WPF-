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
using AtmCommon.ViewModels;
using AtmLib.Tracing;

namespace AtmLoader.Pages {
    /// <summary>
    /// Interaction logic for DemoPage.xaml
    /// </summary>
    public partial class DemoPage : Page {
        public DemoPage() {
            using (var log = new Logger(TraceLevel.Verbose, "DemoPage.DemoPage")) {
                InitializeComponent();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        public static string Path { get; } = "Pages/DemoPage.xaml";
        public static string LinkName { get; } = "ATM UI Demos";
        public static bool IsNavigationEnabled { get; } = true;

        private void OptionScreen_Loaded(object sender, RoutedEventArgs e) {
            // This will log the method name, parameter names, and parameter values
            using (var log = new Logger(TraceLevel.Verbose, "DemoPage.OptionScreen_Loaded",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e })) 
            {

                // Anything that needs to be done, put it here.
                log.WriteLine(TraceLevel.Verbose, "This will be written under the context of the current scope");


                log.Write(TraceLevel.Verbose, "This line ");
                log.Write(TraceLevel.Verbose, "will be built ");
                log.WriteLine(TraceLevel.Verbose, "bit by bit.");

                try {
                    throw new NotImplementedException("oops!");
                }
                catch (Exception ex) {
                    Logger.WriteException(ex);
                }
            }

            // Because Log is wrapped in a "using" statement, the exit point will also be logged, 
            // no matter how the scope is exited (return, throw, whatever).
        }

        public OptionScreen ViewModel {
            get => (OptionScreen)DataContext;
        }

    }
}
