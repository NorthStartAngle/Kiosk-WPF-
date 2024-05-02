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

namespace AtmCommon.Templates {
    /// <summary>
    /// Interaction logic for OptionScreen.xaml
    /// </summary>
    public partial class OptionScreen : UserControl {
        public OptionScreen() {
            InitializeComponent();
        }

        public ViewModels.OptionScreen ViewModel {
            get => (ViewModels.OptionScreen)DataContext;
        }

        private void Base_Loaded(object sender, RoutedEventArgs e) {
            if (ViewModel != null) {
                if (ViewModel.Button1_Command is null) {
                    ViewModel.Button1_Command = Button1_Command;
                }

                if (ViewModel.Button2_Command is null) {
                    ViewModel.Button2_Command = Button2_Command;
                }

                if (ViewModel.Button3_Command is null) {
                    ViewModel.Button3_Command = Button3_Command;
                }

                if (ViewModel.Button4_Command is null) {
                    ViewModel.Button4_Command = Button4_Command;
                }
            }
        }

        private void FixedNavigation_Loaded(object sender, RoutedEventArgs e) {
            if (ViewModel != null) {
            }
        }

        /// <summary>
        /// Gets or sets additional content for the UserControl
        /// </summary>
        new public object Content {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        new public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(OptionScreen),
              new PropertyMetadata(defaultValue: null));

        public static RoutedCommand Button1_Command = new RoutedCommand();
        public static RoutedCommand Button2_Command = new RoutedCommand();
        public static RoutedCommand Button3_Command = new RoutedCommand();
        public static RoutedCommand Button4_Command = new RoutedCommand();

        private void Button1_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (ViewModel.Button1_Link != null) {
                Navigator.Navigate(this, ViewModel.Button1_Link);
            }
        }

        private void Button1_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ViewModel.Button1_Link != null;
        }

        private void Button2_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (ViewModel.Button2_Link != null) {
                Navigator.Navigate(this, ViewModel.Button2_Link);
            }
        }

        private void Button2_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ViewModel.Button2_Link != null;
        }

        private void Button3_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (ViewModel.Button3_Link != null) {
                Navigator.Navigate(this, ViewModel.Button3_Link);
            }
        }

        private void Button3_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ViewModel.Button3_Link != null;
        }

        private void Button4_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (ViewModel.Button4_Link != null) {
                Navigator.Navigate(this, ViewModel.Button4_Link);
            }
        }

        private void Button4_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = ViewModel.Button4_Link != null;
        }
    }
}
