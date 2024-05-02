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
    /// Interaction logic for RegisterPhoneKYC.xaml
    /// </summary>
    public partial class RegisterPhoneKYC : Page, IAtmNavigation {
        public RegisterPhoneKYC() {
            InitializeComponent();
        }

        public static string Path { get; } = "Pages/RegisterPhoneKYC.xaml";

        public static string LinkName { get; } = "Register (Phone KYC)";

        public static bool IsNavigationEnabled {
            get {
                // For now, this is good enough. Later, we may want logic here.
                return true;
            }
        }

        public ViewModels.RegisterPhoneKYC ViewModel {
            get => (ViewModels.RegisterPhoneKYC)DataContext;
        }

        public static RoutedCommand Submit_Command = new RoutedCommand();

        private async void Submit_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            // This function executes when the command is activated by an associated control 
            // on the template. This example navigates to another page in the application.
            ViewModel.DialogVisibility = Visibility.Visible;
            Dialog.Visibility = Visibility.Visible;

            AtmApi.RegistrationData registrationData = new AtmApi.RegistrationData { userName = ViewModel.UserGivenName };
            var success = await AtmApi.RegisterPhoneKYC(registrationData);
            ViewModel.DialogVisibility = Visibility.Hidden;

            if (success) {
                Navigator.Navigate<Home>(this);
            }
            else {
                return;
            }
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
