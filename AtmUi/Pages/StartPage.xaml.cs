using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
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
using System.IO;
using static AtmUi.AtmApi;
using AtmCommon;
using AtmCommon.ViewModels;

namespace AtmUi.Pages {
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page, IAtmNavigation {
        public StartPage() {
            InitializeComponent();
        }

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        public static string Path { get; } = "Pages/StartPage.xaml";

        public static string LinkName { get; } = "Start Page";

        public static bool IsNavigationEnabled {
            get {
                // For now, this is good enough. Later, we may want logic here.
                return true;
            }
        }

        public OneButton ViewModel {
            get => (OneButton)DataContext;
        }

        public static RoutedCommand Start_Command = new RoutedCommand();

        private async void Start_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            ViewModel.DialogVisibility = Visibility.Visible;

            bool isStarted = await AtmApi.StartTransaction();

            if (isStarted) {
                Navigator.Navigate<Home>(this);
            }
            else {
                /* TODO: Load text from a localized resource */
                /* TODO: Make the error message user-friendly and not techie */
                /* TODO: Log the error */
                ErrorMessage.Error<StartPage>(this, "Transaction did not start successfully.");
            }
        }

        private void Start_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            // If this function returns true, the command can be executed, otherwise it is 
            // unavailable. The associated control will change its appearance and availability 
            // as needed.
            e.CanExecute = true;
        }


    }
}
