using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AtmCommon.ViewModels;
using AtmLib.CheckList;

namespace AtmLoader.ViewModels {
    public class CheckInternet : Blank
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

        private List<CheckInternetInterfaceItem> _networkInterfaces = null;
        public List<CheckInternetInterfaceItem> NetworkInterfaces
        {
            get => _networkInterfaces;
            set { _networkInterfaces = value; OnPropertyChanged(); }
        }

        private IPAddress[] _ipAddresses = null;
        public IPAddress[] IPAddresses
        {
            get => _ipAddresses;
            set { _ipAddresses = value; OnPropertyChanged(); }
        }

        private PingReply _serverPingReply = null;
        public PingReply ServerPingReply
        {
            get => _serverPingReply;
            set { _serverPingReply = value; OnPropertyChanged(); }
        }

        private string _serverName = "";
        public string ServerName
        {
            get => _serverName;
            set { _serverName = value; OnPropertyChanged(); }
        }

        private string _serverStatus = "";
        public string ServerStatus
        {
            get => _serverStatus;
            set { _serverStatus = value; OnPropertyChanged(); }
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
