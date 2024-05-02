using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for SampleExplorer.xaml
    /// </summary>
    public partial class SampleExplorer : Page, IAtmNavigation {
        public SampleExplorer() {
            InitializeComponent();
            Dialog.Visibility = Visibility.Hidden;
        }

        public static string Path { get => "Pages/SampleExplorer.xaml"; }

        public static string LinkName { get; } = "Sample Explorer";

        public static bool IsNavigationEnabled {
            get {
                // For now, this is good enough. Later, we may want logic here.
                return true;
            }
        }

        public ViewModels.SampleExplorer ViewModel {
            get => (ViewModels.SampleExplorer)DataContext;
        }

        private void OptionScreen_Loaded(object sender, RoutedEventArgs e) {

        }

        public static RoutedCommand ShowDialog_Command = new RoutedCommand();

        private void ShowDialog_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            ViewModel.DialogVisibility = Visibility.Visible;
            Dialog.Visibility = Visibility.Visible;
        }

        private void ShowDialog_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        public static RoutedCommand OkButton_Command = new RoutedCommand();

        public void OkButton_Executed(object sender, ExecutedRoutedEventArgs e) {
            ViewModel.DialogVisibility = Visibility.Hidden;
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
