using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page, IAtmNavigation {

        public Home() {
            InitializeComponent();
        }

        public static string Path { get; } = $"Pages/Home.xaml";

        public static string LinkName { get; } = "Home";

        public static bool IsNavigationEnabled {
            get {
                // For now, this is good enough. Later, we may want logic here.
                return true;
            }
        }

        public static readonly DependencyProperty PathProperty =
            DependencyProperty.RegisterAttached(
                "Path",
                typeof(string),
                typeof(PageProperties),
                new PropertyMetadata(null)
                );

        public OptionScreen ViewModel {
            get => (OptionScreen)DataContext;
        }

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
            ViewModel.PageTitle = $"Welcome, {AtmApi.Transaction.UserName}";
        }

        /* Here are the commands that are linked to the template's buttons */
        public static RoutedCommand Cancel_Command = new RoutedCommand();

        private void Cancel_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            ViewModel.DialogVisibility = Visibility.Visible;
        }

        private void Cancel_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            // If this function returns true, the command can be executed, otherwise it is 
            // unavailable. The associated control will change its appearance and availability 
            // as needed.
            e.CanExecute = true;
        }

        public static RoutedCommand OkButton_Command = new RoutedCommand();

        public void OkButton_Executed(object sender, ExecutedRoutedEventArgs e) {
            ViewModel.DialogVisibility = Visibility.Hidden;
            Navigator.Navigate<EndTransaction>(this);
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
