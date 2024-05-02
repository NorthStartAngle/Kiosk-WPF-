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
using AtmCommon.ViewModels;

namespace AtmUi.Pages {
    /// <summary>
    /// Interaction logic for SystemPage.xaml
    /// </summary>
    public partial class SystemPage : Page, IAtmNavigation {
        public SystemPage() {
            InitializeComponent();
        }

        public static string Path { get; } = "Pages/SystemPage.xaml";
        public static string LinkName { get; } = "System";
        public static bool IsNavigationEnabled { get; } = true;

        private void OptionScreen_Loaded(object sender, RoutedEventArgs e) {

        }

        public OptionScreen ViewModel {
            get => (OptionScreen)DataContext;
        }

        public static RoutedCommand Exit_Command = new RoutedCommand();

        private void Exit_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            ViewModel.DialogVisibility = Visibility.Visible;
        }

        private void Exit_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        public static RoutedCommand OkButton_Command = new RoutedCommand();

        public void OkButton_Executed(object sender, ExecutedRoutedEventArgs e) {
            ViewModel.DialogVisibility = Visibility.Hidden;
            Application.Current.Shutdown();
        }

        public void OkButton_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        public static RoutedCommand CancelButton_Command = new RoutedCommand();

        public void CancelButton_Executed(object sender, ExecutedRoutedEventArgs e) {
            ViewModel.DialogVisibility = Visibility.Hidden;
        }

        public void CancelButton_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

    }
}
