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
    /// Interaction logic for AtmStart.xaml
    /// </summary>
    public partial class AtmStart : Page, IAtmNavigation {
        public AtmStart() {
            InitializeComponent();
        }

        public static string Path { get; } = "Pages/AtmStart.xaml";

        public static string LinkName { get; } = "Start Screen";

        public static bool IsNavigationEnabled {
            get {
                // For now, this is good enough. Later, we may want logic here.
                return true;
            }
        }

        public InProgress ViewModel {
            get => (InProgress)DataContext;
        }


        private async void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
            bool isStarted = await AtmApi.Connect();

            if (isStarted) {
                Navigator.Navigate<StartPage>(this);
            }
            else {
                /* TODO: Load text from a localized resource */
                /* TODO: Make the error message user-friendly and not techie */
                /* TODO: Log the error */
                /* TODO: This is not going to the right place in the event of an error. */
                ErrorMessage.Error<StartPage>(this, "Application did not start successfully.");
            }
        }
    }
}

