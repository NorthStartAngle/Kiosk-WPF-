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
    /// Interaction logic for DebitCardToPaperWallet.xaml
    /// </summary>
    public partial class DebitCardToPaperWallet : Page, IAtmNavigation {
        public DebitCardToPaperWallet() {
            InitializeComponent();
        }

        public static string Path { get; } = "Pages/DebitCardToPaperWallet.xaml";

        public static string LinkName { get; } = "Debit Card to Paper Wallet";

        public static bool IsNavigationEnabled {
            get {
                // For now, this is good enough. Later, we may want logic here.
                return true;
            }
        }

        private void OptionScreen_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        /* This probably needs a view model of its own. Borrowing OptionScreen for now. */
        public OptionScreen ViewModel {
            get => (OptionScreen)DataContext;
        }
    }
}
