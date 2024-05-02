using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace AtmCommon.ViewModels {
    public class OneButton : Base {

        private string _button1Content = string.Empty;

        public string Button1_Content {
            get => _button1Content;
            set { _button1Content = value; OnPropertyChanged(); }
        }

        private string _button1_Link = string.Empty;

        public string Button1_Link {
            get => _button1_Link;
            set { _button1_Link = value; OnPropertyChanged(); }
        }

        private bool _button1_IsEnabled = false;

        public bool Button1_IsEnabled {
            get => _button1_IsEnabled;
            set { _button1_IsEnabled = value; OnPropertyChanged(); }
        }


        private Visibility _button1_Visibility;

        public Visibility Button1_Visibility {
            get { return _button1_Visibility; }
            set {
                if (_button1_Visibility != value) {
                    _button1_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }


        private double _button1_FontSize = 30;

        public double Button1_FontSize { 
            get => _button1_FontSize; 
            set { _button1_FontSize = value; OnPropertyChanged(); } 
        }

        private FontFamily _button1_FontFamily = new FontFamily("Segoe UI");

        public FontFamily Button1_FontFamily {
            get => _button1_FontFamily;
            set { _button1_FontFamily = value; OnPropertyChanged(); }
        }


        private static readonly DependencyProperty button1_CommandProperty = 
            DependencyProperty.Register("Button1_Command", typeof(ICommand), typeof(OneButton));


        public static DependencyProperty Button1_CommandProperty => button1_CommandProperty;


        public ICommand Button1_Command {
            get => (ICommand)GetValue(Button1_CommandProperty);
            set => SetValue(Button1_CommandProperty, value);
        }
    }
}
