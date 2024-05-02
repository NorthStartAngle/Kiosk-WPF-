using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AtmCommon.ViewModels;
using AtmLib.CheckList;

namespace AtmLoader.ViewModels {
    public class CheckUpdate : Blank
    {
        private string _installError = "";
        public string InstallError
        {
            get => _installError;
            set { _installError = value; OnPropertyChanged(); }
        }

        private Visibility _installError_Visibility = Visibility.Visible;
        public Visibility InstallError_Visibility
        {
            get => _installError_Visibility;
            set
            {
                if (_installError_Visibility != value)
                {
                    _installError_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _installButton_Visibility = Visibility.Visible;
        public Visibility InstallButton_Visibility
        {
            get => _installButton_Visibility;
            set
            {
                if (_installButton_Visibility != value)
                {
                    _installButton_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<CheckUpdateItem> _details = null;
        public List<CheckUpdateItem> Details
        {
            get => _details;
            set { _details = value; OnPropertyChanged(); }
        }

        private string _message = "";
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private string _message_Foreground = "white";
        public string Message_Foreground
        {
            get => _message_Foreground;
            set { _message_Foreground = value; OnPropertyChanged(); }
        }
    }

}
