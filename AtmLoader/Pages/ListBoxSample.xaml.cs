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
    /// Interaction logic for ListBoxSample.xaml
    /// </summary>
    public partial class ListBoxSample : Page {
        public ListBoxSample() {
            InitializeComponent();
        }

        public static string Path { get; } = "Pages/ListBoxSample.xaml";

        public static string LinkName { get; } = "ListBox Sample";

        public static bool IsNavigationEnabled {
            get {
                return true;
            }
        }

        public ViewModels.DataGridSample ViewModel {
            get => (ViewModels.DataGridSample)DataContext;
        }

        private void DevicePropListBox_KeyDown(object sender, KeyEventArgs e) {
            //if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.C) {
            //    ListBox list = (ListBox)sender;
            //    string str = "";
            //    if (list != null) {
            //        foreach (var item in list.SelectedItems) {
            //            KeyValuePair<string, object>? p = item as KeyValuePair<string, object>?;
            //            if (p != null) {
            //                KeyValuePair<string, object> pair = (KeyValuePair<string, object>)p;
            //                str += $"{pair.Key}:{pair.Value}\n";
            //            }
            //        }
            //    }

            //    Clipboard.SetText(str);
            //}
        }

    }
}
