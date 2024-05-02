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
using AtmCommon.Controls;
using AtmCommon.ViewModels;

namespace AtmLoader.Pages {
    /// <summary>
    /// Interaction logic for ProgressDialogSample.xaml
    /// </summary>
    public partial class ProgressDialogSample : Page {
        public ProgressDialogSample() {
            InitializeComponent();
        }

        private async void BasePage_Loaded(object sender, RoutedEventArgs e) {
            ViewModel.DialogVisibility = Visibility.Visible;

            await Task.Run(async () => {
                await Task.Delay(TimeSpan.FromSeconds(3));  // Wait for 3 seconds
                Application.Current.Dispatcher.Invoke(() => ViewModel.DialogVisibility = Visibility.Hidden);
            });
        }

        public static string Path { get; } = "Pages/ProgressDialogSample.xaml";

        public static string LinkName { get; } = "Progress Dialog Sample";

        public static bool IsNavigationEnabled {
            get {
                // For now, this is good enough. Later, we may want logic here.
                return true;
            }
        }

        public OneButton ViewModel {
            get => (OneButton)DataContext;
        }

        public static RoutedCommand ShowDialog_Command = new RoutedCommand();

        private async void ShowDialog_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            ViewModel.DialogVisibility = Visibility.Visible;

            await Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(3));  // Wait for 3 seconds
                Application.Current.Dispatcher.Invoke(() => ViewModel.DialogVisibility = Visibility.Hidden);
            });
        }

        private void ShowDialog_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            // If this function returns true, the command can be executed, otherwise it is 
            // unavailable. The associated control will change its appearance and availability 
            // as needed.
            e.CanExecute = true;
        }
    }
}
