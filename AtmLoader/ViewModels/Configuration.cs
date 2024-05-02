using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AtmCommon.ViewModels;
using AtmLib.CheckList;

namespace AtmLoader.ViewModels {
    public class Configuration : Blank
    {
        private string _dotNetVersion = "not found";
        public string DotNetVersion
        {
            get => _dotNetVersion;
            set { _dotNetVersion = value; OnPropertyChanged(); }
        }

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

        private string _osMessage = "not found";
        public string OSMessage
        {
            get => _osMessage;
            set { _osMessage = value; OnPropertyChanged(); }
        }

        private List<CheckAppItem> _appInfos = new List<CheckAppItem>();
        public List<CheckAppItem> AppInfos
        {
            get => _appInfos;
            set { _appInfos = value; OnPropertyChanged(); }
        }

        private List<DotNetPackage> _netPackages = new List<DotNetPackage>();
        public List<DotNetPackage> NetPackages
        {
            get => _netPackages;
            set { _netPackages = value; OnPropertyChanged(); }
        }

        private List<AppModule> _appModules = new List<AppModule>();
        public List<AppModule> AppModules
        {
            get => _appModules;
            set { _appModules = value; OnPropertyChanged(); }
        }
    }

}
