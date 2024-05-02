using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace AtmCommon.ViewModels {
    public class Base : TemplateViewModel {
        private string _pageTitle = "";
        public string PageTitle {
            get => _pageTitle;
            set { _pageTitle = value; OnPropertyChanged(); }
        }

        private FontFamily _label_FontFamily = new FontFamily("Segoe UI");

        public FontFamily Label_FontFamily {
            get => _label_FontFamily;
            set { _label_FontFamily = value; OnPropertyChanged(); }
        }

        private double _label_FontSize = 36;

        public double Label_FontSize {
            get => _label_FontSize;
            set { _label_FontSize = value; OnPropertyChanged(); }
        }

        private string _description = "";
        public string Description {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private double _description_FontSize = 28;

        public double Description_FontSize {
            get => _description_FontSize;
            set { _description_FontSize = value; OnPropertyChanged(); }
        }


        private string _backButton_Content = "Back";
        public string BackButton_Content {
            get => _backButton_Content;
            set { _backButton_Content = value; OnPropertyChanged(); }
        }

        private bool _backButton_IsEnabled = true;
        public bool BackButton_IsEnabled {
            get => _backButton_IsEnabled;
            set { _backButton_IsEnabled = value; OnPropertyChanged(); }
        }

        private Visibility _backButton_Visibility = Visibility.Visible;

        public Visibility BackButton_Visibility {
            get { return _backButton_Visibility; }
            set {
                if (_backButton_Visibility != value) {
                    _backButton_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _backButton_FontSize = 30;
        public double BackButton_FontSize {
            get => _backButton_FontSize;
            set { _backButton_FontSize = value; OnPropertyChanged(); }
        }

        private FontFamily _backButton_FontFamily = new FontFamily("Segoe UI");
        public FontFamily BackButton_FontFamily {
            get => _backButton_FontFamily;
            set {
                _backButton_FontFamily = value;
                OnPropertyChanged();
            }
        }

        private static readonly DependencyProperty backButton_CommandProperty = DependencyProperty.Register("BackButton_Command", typeof(ICommand), typeof(Base));
        public static DependencyProperty BackButton_CommandProperty => backButton_CommandProperty;

        public ICommand BackButton_Command {
            get => (ICommand)GetValue(BackButton_CommandProperty);
            set => SetValue(BackButton_CommandProperty, value);
        }

        private string _systemButton_Content = "System Page";
        public string SystemButton_Content {
            get => _systemButton_Content;
            set { _systemButton_Content = value; OnPropertyChanged(); }
        }

        private bool _systemButton_IsEnabled = true;
        public bool SystemButton_IsEnabled {
            get => _systemButton_IsEnabled;
            set { _systemButton_IsEnabled = value; OnPropertyChanged(); }
        }

        private Visibility _systemButton_Visibility = Visibility.Visible;

        public Visibility SystemButton_Visibility {
            get { return _systemButton_Visibility; }
            set {
                if (_systemButton_Visibility != value) {
                    _systemButton_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _systemButton_FontSize = 30;
        public double SystemButton_FontSize {
            get => _systemButton_FontSize;
            set { _systemButton_FontSize = value; OnPropertyChanged(); }
        }

        private FontFamily _systemButton_FontFamily = new FontFamily("Segoe UI");
        public FontFamily SystemButton_FontFamily {
            get => _systemButton_FontFamily;
            set {
                _systemButton_FontFamily = value;
                OnPropertyChanged();
            }
        }

        private static readonly DependencyProperty systemButton_CommandProperty = DependencyProperty.Register("SystemButton_Command", typeof(ICommand), typeof(Base));
        public static DependencyProperty SystemButton_CommandProperty => systemButton_CommandProperty;

        public ICommand SystemButton_Command {
            get => (ICommand)GetValue(SystemButton_CommandProperty);
            set => SetValue(SystemButton_CommandProperty, value);
        }

        private string _systemButton_Link = string.Empty;

        public string SystemButton_Link {
            get => _systemButton_Link;
            set { _systemButton_Link = value; OnPropertyChanged(); }
        }

        private static readonly DependencyProperty _dialog_okButton_CommandProperty = DependencyProperty.Register("Dialog_OkButton_Command", typeof(ICommand), typeof(Base));
        public static DependencyProperty Dialog_OkButton_CommandProperty => _dialog_okButton_CommandProperty;

        private static readonly DependencyProperty _dialog_cancelButton_CommandProperty = DependencyProperty.Register("Dialog_CancelButton_Command", typeof(ICommand), typeof(Base));
        public static DependencyProperty Dialog_CancelButton_CommandProperty => _dialog_cancelButton_CommandProperty;

        private double _dialogText_FontSize = 30;

        public double DialogText_FontSize {
            get => _dialogText_FontSize;
            set { _dialogText_FontSize = value; OnPropertyChanged(); }
        }

        private string _okButton_Content = "";
        public string Dialog_OkButton_Content {
            get => _okButton_Content;
            set { _okButton_Content = value; OnPropertyChanged(); }
        }

        private string _cancelButton_Content = "";
        public string Dialog_CancelButton_Content {
            get => _cancelButton_Content;
            set { _cancelButton_Content = value; OnPropertyChanged(); }
        }


        private double _okButton_FontSize = 30;
        public double Dialog_OkButton_FontSize {
            get => _okButton_FontSize;
            set { _okButton_FontSize = value; OnPropertyChanged(); }
        }

        private double _cancelButton_FontSize = 30;
        public double Dialog_CancelButton_FontSize {
            get => _cancelButton_FontSize;
            set { _cancelButton_FontSize = value; OnPropertyChanged(); }
        }


        private FontFamily _okButton_FontFamily = new FontFamily("Segoe UI");
        public FontFamily Dialog_OkButton_FontFamily {
            get => _okButton_FontFamily;
            set {
                _okButton_FontFamily = value;
                OnPropertyChanged();
            }
        }

        private FontFamily _cancelButton_FontFamily = new FontFamily("Segoe UI");
        public FontFamily Dialog_CancelButton_FontFamily {
            get => _cancelButton_FontFamily;
            set {
                _cancelButton_FontFamily = value;
                OnPropertyChanged();
            }
        }


        private string _dialogText = "";

        public string DialogText {
            get => _dialogText;
            set { _dialogText = value; OnPropertyChanged(); }
        }

        public ICommand Dialog_OkButton_Command {
            get => (ICommand)GetValue(Dialog_OkButton_CommandProperty);
            set => SetValue(Dialog_OkButton_CommandProperty, value);
        }
        public ICommand Dialog_CancelButton_Command {
            get => (ICommand)GetValue(Dialog_CancelButton_CommandProperty);
            set => SetValue(Dialog_CancelButton_CommandProperty, value);
        }
    }
}
