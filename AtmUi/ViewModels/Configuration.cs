using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using AtmCommon.ViewModels;

namespace AtmUi.ViewModels {
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

        public readonly string InstallDotNetFile = "./dotnet-install.ps1";

        private string _targetDotNetVersion = "7.0.5";
        public string TargetDotNetVersion
        {
            get => _targetDotNetVersion;
            set { _targetDotNetVersion = value; OnPropertyChanged(); }
        }

        private string _osVersion = "not found";
        public string OSVersion
        {
            get => _osVersion;
            set { _osVersion = value; OnPropertyChanged(); }
        }

        private string _targetOSVersion = "Windows 7.0";
        public string TargetOSVersion
        {
            get => _targetOSVersion;
            set { _targetOSVersion = value; OnPropertyChanged(); }
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

    public class DotNetPackage
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
    }

    public class AppModule : IComparable
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public string MemorySize { get; set; } = "";
        public string Site{ get; set; } = "";

        public int CompareTo(object? obj)
        {
            if (obj == null) return -1;
            return Name.CompareTo( ( (AppModule) obj ).Name  );
        }
    }
}
