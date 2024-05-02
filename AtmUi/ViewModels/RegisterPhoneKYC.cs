using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AtmCommon.ViewModels;

namespace AtmUi.ViewModels {
    public class RegisterPhoneKYC : Form {
        public RegisterPhoneKYC() { }

        private string _userGivenName = "";

        public string UserGivenName {
            get => _userGivenName;
            set {
                _userGivenName = value;
                OnPropertyChanged();
            }
        }

        private string _userSurname = "";

        public string UserSurname {
            get => _userSurname; 
            set { 
                _userSurname = value;
                OnPropertyChanged();
            }
        }
    }
}
