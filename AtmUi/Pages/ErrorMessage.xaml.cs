using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using AtmCommon.ViewModels;

namespace AtmUi.Pages
{
    /// <summary>
    /// Interaction logic for ErrorMessage.xaml
    /// </summary>
    public partial class ErrorMessage : Page, IAtmNavigation {
        public ErrorMessage() {
            InitializeComponent();
        }

        public OneButton ViewModel {
            get => (OneButton)DataContext;
        }

        public static string Path { get; } = $"Pages/ErrorMessage.xaml";

        public static string LinkName { get; } = "Error Message";

        public static bool IsNavigationEnabled {
            get {
                // For now, this is good enough. Later, we may want logic here.
                return true;
            }
        }

        public static void Error<T>(DependencyObject source, string message, string? buttonText = null) where T : IAtmNavigation {
            var paramList = new Dictionary<string, string> {
                    { "message", message },
                    { "continue", T.Path }
                };

            if (buttonText is not null) { 
                buttonText = buttonText.Trim();
                paramList["buttonText"] = buttonText;
            }

            Navigator.Navigate<T>(source, paramList);
        }

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
            string uriString = NavigationService.CurrentSource.OriginalString;

            if (uriString.Contains('?')) {
                string queryString = NavigationService.CurrentSource.OriginalString.Split('?')[1];
                var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

                string? message = queryDictionary["message"];

                if (message != null) {
                    ViewModel.Description = message;
                }

                string? continuePage = queryDictionary["continue"];

                if (continuePage is not null) {
                    ContinuePage = continuePage;
                }

                string? buttonText = queryDictionary["buttonText"];

                if (buttonText is not null) {
                    ViewModel.Button1_Content = buttonText;
                }
            }
        }

        private string? _continuePage;
        public string? ContinuePage {
            get => _continuePage;
            set {
                if (_continuePage != value) {
                    _continuePage = value;
                    OnPropertyChanged(nameof(ContinuePage));
                }
            }
        }

        public static RoutedCommand Button1_Command = new RoutedCommand();

        private void Button1_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            // This function executes when the command is activated by an associated control 
            // on the template. This example navigates to another page in the application.
            NavigationService.GetNavigationService(this).Navigate(new Uri(ContinuePage!, UriKind.RelativeOrAbsolute));
        }

        private void Button1_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            // If this function returns true, the command can be executed, otherwise it is 
            // unavailable. The associated control will change its appearance and availability 
            // as needed.
            e.CanExecute = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
