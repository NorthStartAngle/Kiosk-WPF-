using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using static System.Formats.Asn1.AsnWriter;
using System.Diagnostics;
using AtmCommon.ViewModels;

namespace AtmUi.ViewModels
{
    public class MachineConfig : TemplateViewModel
    {
        private SystemUser? _currentSysUser;
        private List<SystemUser>? _sysusers;
        private List<AppUID>? _apps;
        private Runspace? runspace;

        public MachineConfig() {
        }

        public void GetUsers()
        {
            using (PrincipalContext? context = new PrincipalContext(ContextType.Machine))
            {
                // create a user principal object to search for all users
                using (UserPrincipal? userPrincipal = new UserPrincipal(context))
                {
                    userPrincipal.Enabled = true; // only enabled users

                    // create a principal searcher to search for all users
                    using (PrincipalSearcher? searcher = new PrincipalSearcher(userPrincipal))
                    {
                        // loop through all users and print their names
                        foreach (Principal result in searcher.FindAll())
                        {
                            var _user = new SystemUser();
                            _user.Name = result.Name;
                            _user.Authority = "admin";
                            SYSUSERS.Add(_user);
                        }
                    }
                }
            }
        }

        public SystemUser? CurrentSystemUser
        { get { return _currentSysUser; } set { _currentSysUser = value; this.OnPropertyChanged(); } }

        public List<SystemUser> SYSUSERS
        {
            get
            {
                if(_sysusers == null)
                {
                    _sysusers = new List<SystemUser>();
                }
                return _sysusers;
            }
        }

        public List<AppUID> APPS
        {
            get
            {
                if (_apps == null)
                {
                    _apps = new List<AppUID>();
                }
                return _apps;
            }
        }

        public bool RunPowerShellCommand(string script = "Get-StartApps") //, IDictionary param = new Dictionary<string, string>()
        {
            //Dictionary<string, string>
            //param = new Dictionary<String, String>();
            bool ret = true;

            this.runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
            this.runspace.Open();

            PowerShell? ps = PowerShell.Create();
            ps.AddCommand("Set-ExecutionPolicy").AddParameter("ExecutionPolicy", "RemoteSigned").AddParameter("Scope", "LocalMachine").Invoke();

            ps.AddCommand(script);
            try
            {
                Collection<PSObject>? results = ps.Invoke();

                // Display the results.
                foreach (PSObject result in results)
                {
                    var _app = new AppUID();
                    _app.Name = (string?)result.Members["Name"].Value;
                    _app.Id =   (string?)result.Members["AppID"].Value;
                    APPS.Add(_app);
                    OnPropertyChanged("");
                }
            }
            catch (RuntimeException)
            {
                ret = false;
            }

            this.runspace.Close();
            return ret;
        }

    }

    public class AppUID : INotifyPropertyChanged
    {
        private string? _name;
        private string? _id;

        public string? Name
        {
            get { return _name; }
            set
            {
                _name = value;OnPropertyChanged();
            }
        }

        public string? Id
        {
            get { return _id; }
            set
            {
                _id = value; OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class SystemUser : INotifyPropertyChanged
    {
        private string? _name;
        private string? _authoity;
        
        public string? Name
        {
            get { return _name; }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string? Authority
        {
            get => _authoity;
            set
            {
                if (_authoity != value)
                {
                    this._authoity = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public override string? ToString()
        {
            return _authoity + _name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
