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

namespace AtmLoader {
    /// <summary>
    /// Interaction logic for KioskWindow.xaml
    /// </summary>
    public partial class KioskWindow : Window {
        public KioskWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            if (!Topmost) {
                var result = MessageBox.Show(
                    "Warning: The application window is not top-most. This is a security risk. To test the application in a non-topmost window, set the IsDevTest setting in App.config to 'True'.\nDo you want to continue running the application?", 
                    "Warning", 
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Cancel) {
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
