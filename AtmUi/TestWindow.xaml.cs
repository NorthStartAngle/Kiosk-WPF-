using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
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

namespace AtmUi {
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window {
        public TestWindow() {
            InitializeComponent();
            // string source = ConfigurationManager.AppSettings["StartPage"]; 
            string source = Properties.Settings.Default.StartPage;
        }

        public Uri Source {
            get { return new Uri(ConfigurationManager.AppSettings["StartPage"]!, UriKind.Relative); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            AdjustWindowSize();
        }

        private void AdjustWindowSize() {
            // Get the screen where the window is located
            var screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);

            // Get the current system DPI and calculate the ratio
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
                float dpiRatio = 96 / g.DpiY;

                // Set the window's height to match the screen's working area height, adjusting for DPI
                this.Height = screen.WorkingArea.Height * dpiRatio;
            }
        }
    }
}
