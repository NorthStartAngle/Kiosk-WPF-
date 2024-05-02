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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AtmCommon.Controls {
    /// <summary>
    /// Interaction logic for Dialog_OkCancel.xaml
    /// </summary>
    [ContentProperty("InnerContent")]
    public partial class Dialog_OkCancel : UserControl {
        public Dialog_OkCancel() {
            InitializeComponent();
        }

        public object InnerContent {
            get { return (object)GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }

        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register("InnerContent", typeof(object), typeof(Dialog_OkCancel),
              new PropertyMetadata(defaultValue: string.Empty));

        public static RoutedCommand OkButton_Command = new RoutedCommand();
        public static RoutedCommand CancelButton_Command = new RoutedCommand();

        private void OkButton_Executed(object sender, ExecutedRoutedEventArgs e) {

        }

        private void CancelButton_Executed(object sender, ExecutedRoutedEventArgs e) {

        }

        private void OkButton_CanExecute(object sender, CanExecuteRoutedEventArgs e) {

        }

        private void CancelButton_CanExecute(object sender, CanExecuteRoutedEventArgs e) {

        }
    }
}
