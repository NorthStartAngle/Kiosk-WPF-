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
using AtmCommon.ViewModels;

namespace AtmLoader.Pages {
    /// <summary>
    /// Interaction logic for DataGridSample.xaml
    /// </summary>
    public partial class DataGridSample : Page {
        public DataGridSample() {
            InitializeComponent();
        }

        public static string Path { get; } = "Pages/DataGridSample.xaml";

        public static string LinkName { get; } = "DataGrid Sample";

        public static bool IsNavigationEnabled {
            get {
                return true;
            }
        }

        public ViewModels.DataGridSample ViewModel {
            get => (ViewModels.DataGridSample)DataContext;
        }

        private void RegisterKeyboardControl(object sender, RoutedEventArgs e) {
            if (sender is Control control) {
                TouchKeyboard.RegisterControl(control);
                control.Focus();
            }
        }
    }
}
