using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.DirectoryServices.AccountManagement;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Security.Principal;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.Windows.Threading;
using System.IO;
using System.Windows.Resources;
using System.Security.Policy;
using System.Xml.Linq;
using System.Windows.Media;
using System.Threading;
using System.ServiceProcess;
using System.DirectoryServices;
using System.Reflection;
using AtmCommon.ViewModels;
using AtmCommon;
using System.Windows.Forms;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.DirectoryServices.ActiveDirectory;
using System.Data;
using System.Windows.Media.Media3D;
using AtmLib;
using System.Diagnostics;
using AtmLib.Tracing;
using AtmLib.Monitor;

namespace AtmLoader.ViewModels
{
    public delegate void MyEventHandler(object source, MyEventArgs e);

    public class MyEventArgs : EventArgs
    {
        private string EventInfo;
        public MyEventArgs(string Text)
        {
            EventInfo = Text;
        }
        public string GetInfo()
        {
            return EventInfo;
        }
    }

    public class LockDown : Blank
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        private const uint EWX_LOGOFF = 0x00000000;
        private const uint EWX_FORCE = 0x00000004;

        public delegate bool delegateOfResultProcess(PSObject pObj);

        private Runspace runspace;
        private ServiceInfo _selectedSettingInfo;
        public ServiceInfo SelectedSetting{get { return _selectedSettingInfo; }
            set { _selectedSettingInfo = value; OnPropertyChanged(); } 
        }
        public event MyEventHandler OnUpgradeItems;

        public Logger lockdown_log;
        public LockDown()
        {
            lockdown_log = new Logger(TraceLevel.Info, "RunPowerShellCommand");
            
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            Task task1 = Task.Run(() =>
            {
                createSettingInfos();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    lockdown_log.WriteLine(TraceLevel.Info, "Settings was successfully loaded.");
                    OnUpgradeItems?.Invoke(this, new MyEventArgs("Settings"));
                }), DispatcherPriority.Background);
            });

            Task task2 = Task.Run(() =>
            {
                GetAllServices();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    lockdown_log.WriteLine(TraceLevel.Info, "Services are successfully getted.");
                    OnUpgradeItems?.Invoke(this, new MyEventArgs("Services"));
                }), DispatcherPriority.Background);
            });
            Task task3 = Task.Run(() =>
            {
                GetUsers();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    lockdown_log.WriteLine(TraceLevel.Info, "Registered Users are successfully getted.");
                    OnUpgradeItems?.Invoke(this, new MyEventArgs("Users"));
                }), DispatcherPriority.Background);
            });
            Task task4 = Task.Run(() =>
            {
                string[] path_items = identity.Name.Split('\\');
                _installapps = new List<AppInfo>();
                if (path_items.Length == 2)
                {
                    if(RunPowerShellCommand("Get-AppxPackage -user " + path_items[1], ResultProcessForPowerShell,true))
                    {
                        lockdown_log.WriteLine(TraceLevel.Info, "rocessing are successfully getted.");
                    }
                    else
                    {
                        /* TODO: This should be changed. The logic should only throw the exception. */
                        /* throw new RuntimeException($"Failed running Powershell script for processing :Get-AppxPackage -user{path_items[1]}"); */
                        Logger.WriteException(new RuntimeException($"Failed running Powershell script for processing :Get-AppxPackage -user{path_items[1]}"));
                    }
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    OnUpgradeItems?.Invoke(this, new MyEventArgs("Installedapp"));
                }), DispatcherPriority.Background);

            });
        }

        private List<SettingBlock> items;
        public List<SettingBlock> SettingsInfos => items;

        private List<ServiceInfo> serviceInfos;
        public List<ServiceInfo> ServiceInfos => serviceInfos;

        private List<SystemUser> _sysusers;
        public List<SystemUser> Sysusers => _sysusers;

        private List<AppInfo> _installapps;
        public List<AppInfo> InstallApps => _installapps;

        private string _runScriptResults;
        public string RunScriptResults
        {
            get { return _runScriptResults; }
            set { _runScriptResults = value; OnPropertyChanged(); }
        }

        public void RunScriptsByUpgrade()
        {
            RunScriptResults = "";
            IList<SettingBlock> found = (IList<SettingBlock>)SettingsInfos.FindAll(item => item.IsSelected == true);
            Task task1 = Task.Run(async () =>
            {
                foreach (SettingBlock item in found)
                {
                    RunScriptResults += item.Script + "\n>result ";

                    bool ret = RunPowerShellCommand(item.Script, ResultProcessForPowerShell);
                    if (ret)
                    {
                        RunScriptResults += "Ok done!\n>\n";
                        item.RunResult = "Ok";
                        item.refreshItem();
                    }
                    else
                    {
                        RunScriptResults += "failed!\n>\n";
                        item.RunResult = "failed";
                    }

                    await Task.Delay(500);
                }

                _ = Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await Task.Delay(3000);
                    this.UpgradingShow = Visibility.Hidden;
                }), DispatcherPriority.Background);
            });

            lockdown_log.WriteLine(TraceLevel.Info, "Upgrading was processed.");

            //task1.Wait();
            //SelectedSetting = null;
        }

        public void selectAll(bool force = false, bool setting = false)
        {
            if (items.Count == 0) return;
            if (force)
            {
                for (int index = 0; index < items.Count; index++)
                {
                    items[index].IsSelected = setting;
                }
            }
            else
            {
                bool checkedStatus = items[0].IsSelected;
                int prepos = 0;
                for (int index = 0; index < items.Count; index++)
                {
                    if (items[index].IsSelected == checkedStatus)
                    {
                        items[index].IsSelected = !checkedStatus;
                    }
                    else
                    {
                        for (int reindex = prepos; reindex <= index; reindex++)
                        {
                            items[reindex].IsSelected = true;
                        }
                        prepos = index;
                        checkedStatus = false;
                    }
                }
            }
        }

        private void GetAllServices()
        {
            serviceInfos = new List<ServiceInfo>();
            ServiceInfo record;
            foreach (ServiceController service in ServiceController.GetServices())
            {
                record = new ServiceInfo();
                record.Name = service.ServiceName;
                record.DisplayName = service.DisplayName;
                record.Types = service.ServiceType.ToString();
                record.Status = service.Status.ToString();

                serviceInfos.Add(record);
            }
        }

        private void createSettingInfos()
        {
            items = new List<SettingBlock>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream resourceStream = assembly.GetManifestResourceStream("AtmLoader.res.settingsinfo.txt");

            try
            {
                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
						SettingBlock sb =SettingBlock.Parse(line);
						if(sb != null ) items.Add(sb);			
                    }
                }
            }
            catch (IOException e)
            {
                Logger.WriteException(e);                
            }
        }

        private bool ResultProcessForPowerShell(PSObject pObj)
        {
            AppInfo record = new AppInfo();
            foreach (var member in pObj.Members)
            {
                if (member.Name == "Name")
                {
                    record.Name = member.Value.ToString();
                }
                else if (member.Name == "PublisherId")
                {
                    record.PublishID = member.Value.ToString();
                }
                else if (member.Name == "Architecture")
                {
                    record.Arch = member.Value.ToString();
                }
                else if (member.Name == "Version")
                {
                    record.Version = member.Value.ToString();
                }
                else if (member.Name == "PackageFullName")
                {
                    record.Package = member.Value.ToString();
                }
                else if (member.Name == "InstallLocation")
                {
                    record.Location = member.Value.ToString();
                }
                else if (member.Name == "Dependencies")
                {
                    record.Dependencies = member.Value.ToString();
                }
                else if (member.Name == "NonRemovable")
                {
                    record.Removable = (bool)member.Value;
                }
                else if (member.Name == "Status")
                {
                    record.Status = member.Value.ToString();
                }

            }

            _installapps.Add(record);
            return true;
        }

        public bool RunPowerShellCommand(string script, delegateOfResultProcess f = null, bool isfirst = false) //, IDictionary param = new Dictionary<string, string>()
        {
            bool ret = true;

            this.runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
            this.runspace.Open();

            PowerShell ps = PowerShell.Create();
            try
            {
                if(isfirst)
                {
                    ps.AddCommand("Set-ExecutionPolicy").AddParameter("ExecutionPolicy", "Unrestricted ").AddParameter("Scope", "CurrentUser").Invoke();
                }

                ps.AddScript(script);

                Collection<PSObject> results = ps.Invoke();
                
                if (ps.HadErrors)
                {
                    /*foreach (ErrorRecord error in ps.Streams.Error.ReadAll())
                    {
                        Console.WriteLine(error.ToString());
                    }*/

                    /* TODO: Change this to only throw the exception. */
                    /* throw new Exception($"error while running powershell script:{script}"); */
                    Logger.WriteException(new Exception($"error while running powershell script:{script}"));

                    ret = false;
                }
                else
                {
                    foreach (PSObject result in results)
                    {
                        if (f == null || !f(result))
                        {
                            break;
                        }
                    }
                }                
            }
            catch (RuntimeException ex)
            {
                ret = false;
                Logger.WriteException(ex);
            }

            this.runspace.Close();
            return ret;
        }

        public void GetUsers()
        {
            _sysusers = new List<SystemUser>();
            SystemUser record;

            using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
            {
                // create a user principal object to search for all users
                using (UserPrincipal userPrincipal = new UserPrincipal(context))
                {
                    userPrincipal.Enabled = true;
                    // create a principal searcher to search for all users
                    using (PrincipalSearcher searcher = new PrincipalSearcher(userPrincipal))
                    {
                        // loop through all users and print their names
                        foreach (Principal user in searcher.FindAll())
                        {

                            var groups = user.GetGroups();

                            // create a string to hold the user's permissions
                            string permissions = "";

                            // loop through the user's groups and add them to the permissions string
                            foreach (var group in groups)
                            {
                                permissions += group.Name + ", ";
                            }

                            // remove the trailing comma and space from the permissions string
                            if (permissions.Length > 2)
                            {
                                permissions = permissions.Substring(0, permissions.Length - 2);
                            }

                            record = new SystemUser();
                            record.Name = user.Name;

                            bool isAdmin = false;
                            using (var group = GroupPrincipal.FindByIdentity(context, "Administrators"))
                            {
                                if (group != null)
                                {
                                    isAdmin = user.IsMemberOf(group);
                                }
                            }

                            if (user.Sid == WindowsIdentity.GetCurrent().User)
                            {
                                record.Islogined = true;
                            }
                            else
                            {
                                record.Islogined = false;
                            }

                            record.Authority = isAdmin ? "admin" : "";
                            record.Permission = permissions;
                            _sysusers.Add(record);
                        }
                    }
                }
            }
        }

        private double _progressValue = 0;
        public double ProgressValue { 
            get => _progressValue; 
            set { _progressValue = value; OnPropertyChanged() ; } 
        }

        private Visibility _upgradingShow = Visibility.Hidden;
        public Visibility UpgradingShow { get => _upgradingShow; set { _upgradingShow = value; OnPropertyChanged() ; } }
    }

    public enum POLICY_TYPE
    {
        NUM = 0,
        STRING
    };

    public class AppInfo : TemplateViewModel
    {
        private string _name;
        private string _archtecture;
        private string _version;
        private string _package;
        private string _location;
        private string _publishid;
        private string _dependencies;
        private bool _removeable;
        private string _status;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string Arch
        {
            get { return _archtecture; }
            set { _archtecture = value; OnPropertyChanged(); }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; OnPropertyChanged(); }
        }
        public bool Removable
        {
            get { return _removeable; }
            set { _removeable = value; OnPropertyChanged(); }
        }
        public string Package
        {
            get { return _package; }
            set { _package = value; OnPropertyChanged(); }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; OnPropertyChanged(); }
        }

        public string PublishID
        {
            get { return _publishid; }
            set { _publishid = value; OnPropertyChanged(); }
        }

        public string Dependencies
        {
            get { return _dependencies; }
            set { _dependencies = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }
    }

    public class SystemUser : TemplateViewModel
    {
        private string _name;
        private string _authoity;
        private string _permission;

        public bool Islogined { get; set; }

        public string Name
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

        public string Authority
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

        public string Permission
        {
            get => _permission;
            set
            {
                if (_permission != value)
                {
                    this._permission = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }

    public class ServiceInfo : TemplateViewModel
    {
        private string _name;
        private string _displayname;
        private string _types;
        private string _status;

        public string Name { get { return _name; } set { _name = value; this.OnPropertyChanged(); } }
        public string DisplayName { get { return _displayname; } set { _displayname = value; this.OnPropertyChanged(); } }
        public string Types { get { return _types; } set { _types = value; this.OnPropertyChanged(); } }
        public string Status { get { return _status; } set { _status = value; this.OnPropertyChanged(); } }
    }

    public class RegistryItem : TemplateViewModel
    {
        private string _path;
        private string _valuename;
        private POLICY_TYPE _valuetype;
        private object _value;
        private IDictionary<string, object> _valuesformat;
        private object _suggestedvalue;

        public string Title
        {
            get
            {
                return $"•      Key : {_path}\n       Value : {getValueFormat()}({_valuetype}) ";
            }
            set
            {
                OnPropertyChanged();
            }
        }

        public string Path { get { return _path; } set { _path = value;  } }
        public object Value { get { return _value; } set { _value = value; Title="";  } }
        public object SuggestedValue { get { return _suggestedvalue; } set { _suggestedvalue = value; OnPropertyChanged(); } }
        public string Valuename { get { return _valuename; } set { _valuename = value; } }

        public POLICY_TYPE ValueTypes
        {
            get { return _valuetype; }
            set { _valuetype = value;}
        }

        public string getValueFormat()
        {
            string ret = "Null";
            if (_value == null) return ret;
            if(_valuetype == POLICY_TYPE.NUM)
            {
                foreach (KeyValuePair<string, object> vf in _valuesformat)
                {
                    if (vf.Value.ToString() == _value.ToString())
                    {
                        ret = vf.Key; break;
                    }
                }
            }
            else if(_valuetype == POLICY_TYPE.STRING)
            {
                foreach (KeyValuePair<string, object> vf in _valuesformat)
                {
                    if (vf.Value.ToString() == "~" &&  _value.ToString().Length >0)
                    {
                        ret = vf.Key; break;
                    }
                    if (vf.Value.ToString() == _value.ToString())
                    {
                        ret = vf.Key; break;
                    }
                }
            }
            
            return ret;
        }

		public string SuggestedValueToString()
		{
            if (_valuetype == POLICY_TYPE.NUM)
            {
                return _suggestedvalue== null?"":_suggestedvalue.ToString();
            }
            else if (_valuetype == POLICY_TYPE.STRING)
            {
                return _suggestedvalue == null ? "\"\"" : "\"" + _suggestedvalue.ToString() + "\"";
            }
            else
                return "";
		}
		
        public void setValue()
        {
            Value = readFromRegedit(Path, Valuename);
        }
        
        public static RegistryItem Parse(string strContent)
        {
			if(strContent =="") return null;
			
			string[] parts = strContent.Split('|');
			
			if (parts.Length == 5)
			{
                RegistryItem item = new RegistryItem();
                item.Path = parts[1].Trim();
                item._valuetype = (POLICY_TYPE)int.Parse(parts[2].Trim());
                item._valuename = parts[3].Trim();
                
                string[] formats =parts[4].Trim().Split('-');
                if(formats.Length == 2)
                {
                    item._valuesformat = new Dictionary<string, object>();
                    item._valuesformat.Add("Enabled", formats[0]);
                    item._valuesformat.Add("Disabled", formats[1]);
                }

                item.setValue();

                return item;
            }
            else
            {
                return null;
            }            
        }

        private bool writeToRegedit(string keypath, string key, object value)
        {
            RegistryKey sk = null;
            string _tmp;
            string[] path_items = keypath.Split('\\');
            _tmp = string.Join("\\", path_items, 1, path_items.Length - 1);
            _tmp = _tmp.Substring(1, _tmp.Length - 1);

            if (path_items[0] == "HKEY_CURRENT_USER")
            {
                
                sk = Registry.CurrentUser.CreateSubKey(_tmp);
            }
            else if (path_items[0] == "HKEY_LOCAL_MACHINE")
            {
                sk = Registry.LocalMachine.CreateSubKey(_tmp);
            }

            try
            {
                sk.SetValue(key, value);
                sk.Close();
                return true;
            }
            catch (Exception e)
            {
                sk.Close();
                Utility.LogException(e);
                return false;
            }
        }

        private object readFromRegedit(string keyPath, string key)
        {
            RegistryKey sk = null;
            string _tmp;
            string[] path_items = keyPath.Split('\\', (char)StringSplitOptions.RemoveEmptyEntries);

            _tmp = string.Join("\\", path_items, 1, path_items.Length - 1);

            if (path_items[0] == "HKEY_CURRENT_USER")
            {              
                sk = Registry.CurrentUser.OpenSubKey(_tmp);
            }
            else if (path_items[0] == "HKEY_LOCAL_MACHINE")
            {
                sk = Registry.LocalMachine.OpenSubKey(_tmp);
            }

            if (sk == null)
            {
                return null;
            }
            else
            {
                try
                {
                    return sk.GetValue(key);
                }
                catch (Exception e)
                {
                    Utility.LogException(e);
                    return null;
                }
            }
        }
    }

    public class SettingBlock : TemplateViewModel
    {
        private bool _isSelected;
        private string _name;
        private string _script;
        private IList<RegistryItem> _regItems;
        private string _runResult;
        private string _status;

        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; OnPropertyChanged(); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }        
        public string Script { get { return _script; } set { _script = value; OnPropertyChanged(); } }
        public IList<RegistryItem> RegItems { get { return _regItems; }}
        public string RunResult { get { return _runResult; } set { _runResult = value;OnPropertyChanged(); } }
        public string Status { get { return _status; } set { _status = value; OnPropertyChanged(); } }

        private void createStatus()
        {
            string ret = "";
            foreach(RegistryItem item in _regItems)
            {
                ret += item.getValueFormat() + ",";
            }
            if(ret.Length>0) ret = ret.Substring(0, ret.Length - 1);
            Status = ret;
        }

        public static SettingBlock Parse(string strContent)
        {
			if(strContent =="") return null;
			string[] parts = strContent.Split('=');
			if (parts.Length == 2)
			{
				SettingBlock sb =new SettingBlock();
				sb._isSelected = false;
				sb._name = parts[0].Trim();
                sb.RunResult = "NoAction";
				if(parts[1] != "")
				{
					sb._regItems = new List<RegistryItem>();
					string[] strRegItems = parts[1].Split(',');
					for(int index =0 ;index < strRegItems.Length ;index++)
					{
                        string strRegItem = strRegItems[index].Trim();

                        if (strRegItem != "")
                        {
							var _item =RegistryItem.Parse(strRegItem);
							if(_item != null)
							{
                                _item.PropertyChanged += sb._item_PropertyChanged;
                                sb._regItems.Add( _item );
                                sb.createScript(_item);
							}
                        }

                    }
				}
                sb.createStatus();
                return sb;
			}else{
				return null;
			}
        }
        
        private void _item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "SuggestedValue")
            {
                createScript((RegistryItem)sender);
            }else if(e.PropertyName == "Title")
            {
                createScript((RegistryItem)sender);
                createStatus();
            }
        }

        public void refreshItem()
        {
            foreach (RegistryItem item in _regItems)
            {
                item.setValue();
            }
        }

        private void createScript(RegistryItem _item)
        {
            if (_regItems == null && _regItems.Count == 0) return;

			string psPaths ="";
			string psValuenames ="";
			string psValues ="";
			
            foreach(RegistryItem item in _regItems)
            {			
				string[] path_items = item.Path.Split('\\');			
				if (path_items[0] == "HKEY_CURRENT_USER")
				{
					path_items[0] = "HKCU:";
				}
				else if (path_items[0] == "HKEY_LOCAL_MACHINE")
				{
					path_items[0] = "HKLM:";
				}
				psPaths +=  "\""+ string.Join("\\", path_items, 0, path_items.Length) + "\"" + ",";
				psValuenames += "\""+ item.Valuename + "\"" + ",";
				psValues += item.SuggestedValueToString() + ",";
            }
            psPaths =psPaths.Substring(0, psPaths.Length - 1);
            psValuenames = psValuenames.Substring(0, psValuenames.Length - 1);
            psValues = psValues.Substring(0, psValues.Length - 1);

            string scriptor = $"$paths = @({psPaths})\n";
            scriptor += $"$names = @({psValuenames})\n";
            scriptor += $"$values = @({psValues})\n";
            scriptor += "for ($i = 0; $i -lt $paths.Length; $i++)\n{\n";
            scriptor += "   $path = $paths[$i]\r\n   $name = $names[$i]\r\n   $value = $values[$i]\r\n";
            scriptor += "   If(-NOT(Test-Path -Path $path))\n";
            scriptor += "   {\n";
            scriptor += "            New-Item -Path $path -Force | Out-Null\n";
            scriptor += "   }\n";
            scriptor += "   New-ItemProperty -Path $path -Name $name -Value $value -Force\n\r";
            scriptor += "}\n";

            Script = scriptor;
        }
    }
}