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

namespace AtmCommon.Templates {
    /// <summary>
    /// Interaction logic for Blank.xaml
    /// </summary>
    public partial class Blank : UserControl {
        public Blank() {
            InitializeComponent();
        }

        new public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(Blank), 
                new PropertyMetadata(defaultValue: null));

        new public object Content {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
    }
}
