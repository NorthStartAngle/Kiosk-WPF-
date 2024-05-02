using System;
using System.Collections.Generic;
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

using AtmCommon;

namespace AtmUi.Pages {
    /// <summary>
    /// Interaction logic for RegisterEnhancedKYC.xaml
    /// </summary>
    public partial class RegisterEnhancedKYC : Page, IAtmNavigation {
        public RegisterEnhancedKYC() {
            InitializeComponent();
        }

        public static string Path { get; } = "Pages/RegisterEnhancedKYC.xaml";
        public static string LinkName { get; } = "Register (Enhanced KYC)";
        public static bool IsNavigationEnabled { get; } = true;

        public ViewModels.RegisterEnhancedKYC ViewModel {
            get => (ViewModels.RegisterEnhancedKYC)DataContext;
        }

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
        }

        public static RoutedCommand Submit_Command = new RoutedCommand();

        private void Submit_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            // This function executes when the command is activated by an associated control 
            // on the template. This example navigates to another page in the application.
            Navigator.Navigate<Home>(this);
        }

        private void Submit_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            // If this function returns true, the command can be executed, otherwise it is 
            // unavailable. The associated control will change its appearance and availability 
            // as needed.
            e.CanExecute = true;
        }

        private void Form_Loaded(object sender, RoutedEventArgs e) {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            UserGivenName.Focus();
        }

        private void RegisterKeyboardControl(object sender, RoutedEventArgs e) {
            if (sender is Control control) {
                Keyboard.RegisterControl(control);
            }
        }
    }
}

