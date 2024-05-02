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

namespace AtmLoader.Pages {
    /// <summary>
    /// Interaction logic for KeyboardDemo.xaml
    /// </summary>
    public partial class KeyboardDemo : Page {
        public KeyboardDemo() {
            InitializeComponent();
        }

        public static string Path { get; } = "Pages/KeyboardDemo.xaml";
        public static string LinkName { get; } = "Keyboard Demo";
        public static bool IsNavigationEnabled { get; } = true;

        private List<Control> controls = new List<Control>();

        private void RegisterKeyboardControl(object sender, RoutedEventArgs e) {
            if (sender is Control control) {
                TouchKeyboard.RegisterControl(control);
                controls.Add(control);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
        }

        private void TouchKeyboard_Loaded(object sender, RoutedEventArgs e) {
            controls.First().Focus();
        }
    }
}
