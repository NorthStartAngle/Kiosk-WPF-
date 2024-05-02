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

namespace AtmCommon.Templates {
    /// <summary>
    /// Interaction logic for Base.xaml
    /// </summary>
    [ContentProperty("Content")]
    public partial class Base : UserControl {
        public Base() {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets additional content for the UserControl
        /// </summary>
        new public object Content {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        new public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(Base),
              new PropertyMetadata(defaultValue: null));
    }
}
