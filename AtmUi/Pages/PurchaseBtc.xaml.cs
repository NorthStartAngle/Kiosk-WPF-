using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
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
    /// Interaction logic for PurchaseBtc.xaml
    /// </summary>
    public partial class PurchaseBtc : Page, IAtmNavigation {
        public PurchaseBtc() {
            InitializeComponent();
        }

        public OptionScreen ViewModel {
            get => (OptionScreen)DataContext;
        }

        public static string Path { get; } = $"Pages/PurchaseBtc.xaml";

        public static string LinkName { get; } = "Purchase BTC";

        public static bool IsNavigationEnabled {
            get {
                // For now, this is good enough. Later, we may want logic here.
                return true;
            }
        }

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }
    }
}
