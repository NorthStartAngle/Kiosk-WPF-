using kiosk_blue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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

namespace kiosk_deployer.Templates
{
    public partial class PageTemplate : UserControl
    {
        private PageTemplateData? _viewinfo;

        public PageTemplate()
        {
            InitializeComponent();
        }
                
        public PageTemplateData? VIEWINFO {
            get { return _viewinfo; }
            set { 
                _viewinfo = value;
            }
        }

        public object BodyContent
        {
            get { return (object)GetValue(BodyContentProperty); }
            set { SetValue(BodyContentProperty, value); }
        }

        public static readonly DependencyProperty BodyContentProperty =
           DependencyProperty.Register("PageTemplate.BodyContent", typeof(object), typeof(PageTemplate), new PropertyMetadata(null));

    }

    public class PageTemplateData : ViewDataObj
    {
        private Visibility _footerVisible = Visibility.Visible;
        private Visibility _headerVisible = Visibility.Visible;

        public Visibility FooterVisibility
        {
            get { return _footerVisible; }
            set
            {
                if (_footerVisible != value)
                {
                    _footerVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public Visibility HeaderVisibility
        {
            get { return _headerVisible; }
            set
            {
                if (_headerVisible != value)
                {
                    _headerVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public PageTemplateData()
        {
            FooterVisibility = Visibility.Visible;
            HeaderVisibility = Visibility.Visible;
        }

        public PageTemplateData(PageTemplateData other)
        {
            FooterVisibility = other.FooterVisibility;
            HeaderVisibility = other.HeaderVisibility;
        }

        public string toString()
        {
            return "footer :"+FooterVisibility.ToString() +",Header:" +HeaderVisibility.ToString();
        } 
    }
}
