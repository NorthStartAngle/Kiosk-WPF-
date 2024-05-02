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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AtmCommon.Controls {
    /// <summary>
    /// Interaction logic for FixedNavigation.xaml
    /// </summary>
    [ContentProperty("FixedNavigationContent")]
    public partial class FixedNavigation : UserControl {
        public FixedNavigation() {
            InitializeComponent();
        }

        public ViewModels.Base ViewModel {
            get => (ViewModels.Base)DataContext;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            if (ViewModel != null) {
                if (ViewModel.BackButton_Command is null) {
                    ViewModel.BackButton_Command = BackButton_Command;
                }

                if (ViewModel.SystemButton_Command is null) {
                    ViewModel.SystemButton_Command = SystemButton_Command;
                }
            }
        }

        /// <summary>
        /// Gets or sets additional content for the UserControl
        /// </summary>
        public object FixedNavigationContent {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(FixedNavigationContentProperty, value); }
        }

        public static readonly DependencyProperty FixedNavigationContentProperty =
            DependencyProperty.Register("FixedNavigationContent", typeof(object), typeof(FixedNavigation),
              new PropertyMetadata(null));


        public static RoutedCommand BackButton_Command = new RoutedCommand();

        private void BackButton_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            Navigator.GoBack(this);
        }

        private void BackButton_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        public static RoutedCommand SystemButton_Command = new RoutedCommand();

        private void SystemButton_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (ViewModel.SystemButton_Link != null) {
                Navigator.Navigate(this, ViewModel.SystemButton_Link);
            }

        }

        private void SystemButton_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }
    }
}
