using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace AtmCommon.ViewModels {
    public class OptionScreen : Base {

        private string _button1Content = "";
        public string Button1_Content {
            get => _button1Content;
            set { _button1Content = value; OnPropertyChanged(); }
        }

        private string _button2Content = "";
        public string Button2_Content {
            get => _button2Content;
            set { _button2Content = value; OnPropertyChanged(); }
        }

        private string _button3Content = "";
        public string Button3_Content {
            get => _button3Content;
            set { _button3Content = value; OnPropertyChanged(); }
        }

        private string _button4Content = "";
        public string Button4_Content {
            get => _button4Content;
            set { _button4Content = value; OnPropertyChanged(); }
        }

        private string _button5Content = "";
        public string Button5_Content {
            get => _button5Content;
            set { _button5Content = value; OnPropertyChanged(); }
        }

        private string _button6Content = "";
        public string Button6_Content {
            get => _button6Content;
            set { _button6Content = value; OnPropertyChanged(); }
        }

        private string _button7Content = "";
        public string Button7_Content {
            get => _button7Content;
            set { _button7Content = value; OnPropertyChanged(); }
        }

        private string _button8Content = "";
        public string Button8_Content {
            get => _button8Content;
            set { _button8Content = value; OnPropertyChanged(); }
        }


        private bool _button1_IsEnabled = false;

        public bool Button1_IsEnabled {
            get => _button1_IsEnabled;
            set { _button1_IsEnabled = value; OnPropertyChanged(); }
        }

        private bool _button2_IsEnabled = false;
        public bool Button2_IsEnabled {
            get => _button2_IsEnabled;
            set { _button2_IsEnabled = value; OnPropertyChanged(); }
        }

        private bool _button3_IsEnabled = false;
        public bool Button3_IsEnabled {
            get => _button3_IsEnabled;
            set { _button3_IsEnabled = value; OnPropertyChanged(); }
        }

        private bool _button4_IsEnabled = false;
        public bool Button4_IsEnabled {
            get => _button4_IsEnabled;
            set { _button4_IsEnabled = value; OnPropertyChanged(); }
        }

        private bool _button5_IsEnabled = false;
        public bool Button5_IsEnabled {
            get => _button5_IsEnabled;
            set { _button5_IsEnabled = value; OnPropertyChanged(); }
        }

        private bool _button6_IsEnabled = false;
        public bool Button6_IsEnabled {
            get => _button6_IsEnabled;
            set { _button6_IsEnabled = value; OnPropertyChanged(); }
        }

        private bool _button7_IsEnabled = false;
        public bool Button7_IsEnabled {
            get => _button7_IsEnabled;
            set { _button7_IsEnabled = value; OnPropertyChanged(); }
        }

        private bool _button8_IsEnabled = false;
        public bool Button8_IsEnabled {
            get => _button8_IsEnabled;
            set { _button8_IsEnabled = value; OnPropertyChanged(); }
        }


        private Visibility _button1_Visibility = Visibility.Visible;
        public Visibility Button1_Visibility {
            get { return _button1_Visibility; }
            set {
                if (_button1_Visibility != value) {
                    _button1_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _button2_Visibility = Visibility.Visible;
        public Visibility Button2_Visibility {
            get { return _button2_Visibility; }
            set {
                if (_button2_Visibility != value) {
                    _button2_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _button3_Visibility = Visibility.Visible;
        public Visibility Button3_Visibility {
            get { return _button3_Visibility; }
            set {
                if (_button3_Visibility != value) {
                    _button3_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _button4_Visibility = Visibility.Visible;
        public Visibility Button4_Visibility {
            get { return _button4_Visibility; }
            set {
                if (_button4_Visibility != value) {
                    _button4_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _button5_Visibility = Visibility.Visible;
        public Visibility Button5_Visibility {
            get { return _button5_Visibility; }
            set {
                if (_button5_Visibility != value) {
                    _button5_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _button6_Visibility = Visibility.Visible;
        public Visibility Button6_Visibility {
            get { return _button6_Visibility; }
            set {
                if (_button6_Visibility != value) {
                    _button6_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _button7_Visibility = Visibility.Visible;
        public Visibility Button7_Visibility {
            get { return _button7_Visibility; }
            set {
                if (_button7_Visibility != value) {
                    _button7_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _button8_Visibility = Visibility.Visible;
        public Visibility Button8_Visibility {
            get { return _button8_Visibility; }
            set {
                if (_button8_Visibility != value) {
                    _button8_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }


        private double _button1_FontSize = 30;
        public double Button1_FontSize {
            get => _button1_FontSize;
            set { _button1_FontSize = value; OnPropertyChanged(); }
        }

        private double _button2_FontSize = 30;
        public double Button2_FontSize {
            get => _button2_FontSize;
            set { _button2_FontSize = value; OnPropertyChanged(); }
        }

        private double _button3_FontSize = 30;
        public double Button3_FontSize {
            get => _button3_FontSize;
            set { _button3_FontSize = value; OnPropertyChanged(); }
        }

        private double _button4_FontSize = 30;
        public double Button4_FontSize {
            get => _button4_FontSize;
            set { _button4_FontSize = value; OnPropertyChanged(); }
        }

        private double _button5_FontSize = 30;
        public double Button5_FontSize {
            get => _button5_FontSize;
            set { _button5_FontSize = value; OnPropertyChanged(); }
        }

        private double _button6_FontSize = 30;
        public double Button6_FontSize {
            get => _button6_FontSize;
            set { _button6_FontSize = value; OnPropertyChanged(); }
        }

        private double _button7_FontSize = 30;
        public double Button7_FontSize {
            get => _button7_FontSize;
            set { _button7_FontSize = value; OnPropertyChanged(); }
        }

        private double _button8_FontSize = 30;
        public double Button8_FontSize {
            get => _button8_FontSize;
            set { _button8_FontSize = value; OnPropertyChanged(); }
        }


        private FontFamily _button1_FontFamily = new FontFamily("Segoe UI");
        public FontFamily Button1_FontFamily {
            get => _button1_FontFamily;
            set {
                _button1_FontFamily = value;
                OnPropertyChanged();
            }
        }

        private FontFamily _button2_FontFamily = new FontFamily("Segoe UI");
        public FontFamily Button2_FontFamily {
            get => _button2_FontFamily;
            set {
                _button2_FontFamily = value;
                OnPropertyChanged();
            }
        }

        private FontFamily _button3_FontFamily = new FontFamily("Segoe UI");
        public FontFamily Button3_FontFamily {
            get => _button3_FontFamily;
            set {
                _button3_FontFamily = value;
                OnPropertyChanged();
            }
        }

        private FontFamily _button4_FontFamily = new FontFamily("Segoe UI");
        public FontFamily Button4_FontFamily {
            get => _button4_FontFamily;
            set {
                _button4_FontFamily = value;
                OnPropertyChanged();
            }
        }

        private FontFamily _button5_FontFamily = new FontFamily("Segoe UI");
        public FontFamily Button5_FontFamily {
            get => _button5_FontFamily;
            set {
                _button5_FontFamily = value;
                OnPropertyChanged();
            }
        }

        private FontFamily _button6_FontFamily = new FontFamily("Segoe UI");
        public FontFamily Button6_FontFamily {
            get => _button6_FontFamily;
            set {
                _button6_FontFamily = value;
                OnPropertyChanged();
            }
        }

        private FontFamily _button7_FontFamily = new FontFamily("Segoe UI");
        public FontFamily Button7_FontFamily {
            get => _button7_FontFamily;
            set {
                _button7_FontFamily = value;
                OnPropertyChanged();
            }
        }

        private FontFamily _button8_FontFamily = new FontFamily("Segoe UI");
        public FontFamily Button8_FontFamily {
            get => _button8_FontFamily;
            set {
                _button8_FontFamily = value;
                OnPropertyChanged();
            }
        }


        private string _button1_Link = string.Empty;

        public string Button1_Link {
            get => _button1_Link;
            set { _button1_Link = value; OnPropertyChanged(); }
        }


        private string _button2_Link = string.Empty;

        public string Button2_Link {
            get => _button2_Link;
            set { _button2_Link = value; OnPropertyChanged(); }
        }

        private string _button3_Link = string.Empty;

        public string Button3_Link {
            get => _button3_Link;
            set { _button3_Link = value; OnPropertyChanged(); }
        }

        private string _button4_Link = string.Empty;

        public string Button4_Link {
            get => _button4_Link;
            set { _button4_Link = value; OnPropertyChanged(); }
        }

        private string _button5_Link = string.Empty;
        public string Button5_Link {
            get => _button5_Link;
            set { _button5_Link = value; OnPropertyChanged(); }
        }

        private string _button6_Link = string.Empty;
        public string Button6_Link {
            get => _button6_Link;
            set { _button6_Link = value; OnPropertyChanged(); }
        }

        private string _button7_Link = string.Empty;
        public string Button7_Link {
            get => _button7_Link;
            set { _button7_Link = value; OnPropertyChanged(); }
        }

        private string _button8_Link = string.Empty;
        public string Button8_Link {
            get => _button8_Link;
            set { _button8_Link = value; OnPropertyChanged(); }
        }


        private static readonly DependencyProperty button1_CommandProperty = DependencyProperty.Register("Button1_Command", typeof(ICommand), typeof(OptionScreen));
        public static DependencyProperty Button1_CommandProperty => button1_CommandProperty;

        private static readonly DependencyProperty button2_CommandProperty = DependencyProperty.Register("Button2_Command", typeof(ICommand), typeof(OptionScreen));
        public static DependencyProperty Button2_CommandProperty => button2_CommandProperty;

        private static readonly DependencyProperty button3_CommandProperty = DependencyProperty.Register("Button3_Command", typeof(ICommand), typeof(OptionScreen));
        public static DependencyProperty Button3_CommandProperty => button3_CommandProperty;

        private static readonly DependencyProperty button4_CommandProperty = DependencyProperty.Register("Button4_Command", typeof(ICommand), typeof(OptionScreen));
        public static DependencyProperty Button4_CommandProperty => button4_CommandProperty;

        private static readonly DependencyProperty button5_CommandProperty = DependencyProperty.Register("Button5_Command", typeof(ICommand), typeof(OptionScreen));
        public static DependencyProperty Button5_CommandProperty => button5_CommandProperty;

        private static readonly DependencyProperty button6_CommandProperty = DependencyProperty.Register("Button6_Command", typeof(ICommand), typeof(OptionScreen));
        public static DependencyProperty Button6_CommandProperty => button6_CommandProperty;

        private static readonly DependencyProperty button7_CommandProperty = DependencyProperty.Register("Button7_Command", typeof(ICommand), typeof(OptionScreen));
        public static DependencyProperty Button7_CommandProperty => button7_CommandProperty;

        private static readonly DependencyProperty button8_CommandProperty = DependencyProperty.Register("Button8_Command", typeof(ICommand), typeof(OptionScreen));
        public static DependencyProperty Button8_CommandProperty => button8_CommandProperty;


        public ICommand Button1_Command {
            get => (ICommand)GetValue(Button1_CommandProperty);
            set => SetValue(Button1_CommandProperty, value);
        }
        public ICommand Button2_Command {
            get => (ICommand)GetValue(Button2_CommandProperty);
            set => SetValue(Button2_CommandProperty, value);
        }
        public ICommand Button3_Command {
            get => (ICommand)GetValue(Button3_CommandProperty);
            set => SetValue(Button3_CommandProperty, value);
        }
        public ICommand Button4_Command {
            get => (ICommand)GetValue(Button4_CommandProperty);
            set => SetValue(Button4_CommandProperty, value);
        }

        public ICommand Button5_Command {
            get => (ICommand)GetValue(Button5_CommandProperty);
            set => SetValue(Button5_CommandProperty, value);
        }

        public ICommand Button6_Command {
            get => (ICommand)GetValue(Button6_CommandProperty);
            set => SetValue(Button6_CommandProperty, value);
        }

        public ICommand Button7_Command {
            get => (ICommand)GetValue(Button7_CommandProperty);
            set => SetValue(Button7_CommandProperty, value);
        }

        public ICommand Button8_Command {
            get => (ICommand)GetValue(Button8_CommandProperty);
            set => SetValue(Button8_CommandProperty, value);
        }
    }
}
