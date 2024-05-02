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

namespace AtmCommon.Templates
{
    /// <summary>
    /// Interaction logic for CustomBase.xaml
    /// </summary>
    public partial class CustomBase : UserControl
    {
        public CustomBase()
        {
            InitializeComponent();
        }

        public object HeaderContent
        {
            get { return (object)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public object BodyContent
        {
            get { return (object)GetValue(BodyContentProperty); }
            set { SetValue(BodyContentProperty, value); }
        }

        public object FooterContent
        {
            get { return (object)GetValue(FooterContentProperty); }
            set { SetValue(FooterContentProperty, value); }
        }

        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("CustomBase.HeaderContent", typeof(object), typeof(CustomBase),
              new PropertyMetadata(null));

        public static readonly DependencyProperty BodyContentProperty =
            DependencyProperty.Register("CustomBase.BodyContent", typeof(object), typeof(CustomBase),
              new PropertyMetadata(null));

        public static readonly DependencyProperty FooterContentProperty =
            DependencyProperty.Register("CustomBase.FooterContent", typeof(object), typeof(CustomBase),
              new PropertyMetadata(null));

        public object NavigationContent
        {
            get { return (object)GetValue(NavigationContentProperty); }
            set { SetValue(NavigationContentProperty, value); }
        }

        public static readonly DependencyProperty NavigationContentProperty =
            DependencyProperty.Register("CustomBase.NavigationContent", typeof(object), typeof(CustomBase),
              new PropertyMetadata(null));

        public static RoutedCommand Back_Command = new RoutedCommand();


        private void Back_CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Navigator.GoBack(this);
        }

        private void Back_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

    }
}
