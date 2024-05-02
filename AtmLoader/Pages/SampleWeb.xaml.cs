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

namespace AtmLoader.Pages {
    /// <summary>
    /// Interaction logic for SampleWeb.xaml
    /// </summary>
    public partial class SampleWeb : Page, IAtmNavigation {
        public SampleWeb() {
            InitializeComponent();
        }

        public static string Path { get; } = "Pages/SampleWeb.xaml";

        public static string LinkName { get; } = "Web Sample";

        public static bool IsNavigationEnabled {
            get {
                // For now, this is good enough. Later, we may want logic here.
                return true;
            }
        }
    }
}
