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
    /// Interaction logic for Dialog_InProgress.xaml
    /// </summary>
    [ContentProperty("InnerContent")]
    public partial class Dialog_InProgress : UserControl {
        public Dialog_InProgress() {
            InitializeComponent();
        }

        public object InnerContent {
            get { return (object)GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }

        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register("InnerContent", typeof(object), typeof(Dialog_InProgress),
              new PropertyMetadata(defaultValue: string.Empty));
    }
}
