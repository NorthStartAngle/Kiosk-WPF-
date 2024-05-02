using kiosk_lunch;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Management;
using System;
using System.Windows;
using System.Security.Principal;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.Win32;
using System.DirectoryServices.AccountManagement;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;

namespace kiosk_lunch.Pages
{
    /// <summary>
    /// Interaction logic for startPage.xaml
    /// </summary>
    public partial class startPage : Page
    {
        

        private StartPageData? _viewinfo;
        // private string filepath = "";

        public startPage()
        {
            InitializeComponent();
        }

        public StartPageData VIEWINFOS
        {
            get => (StartPageData)DataContext;
        }



        private void GetLocalUserAccounts()
        {
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd.exe";
            p.Start();
            // Read the output stream first and then wait.
            p.StandardInput.WriteLine("net user");
            p.StandardInput.WriteLine("exit");
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            MatchCollection _output = new Regex(@"(((?! )(?<= {2,}).)((?![" + "\"" + @"&/\[\]:\|\<\>\+=;,\?\*%@]| {2,}).)+(?= {2,}))|((?<! {2,})((?![" + "\"" + @"&/\[\]:\|\<\>\+=;,\?\*%@]| {2,}).)+(?= {2,}))", RegexOptions.Compiled).Matches(output);
            for (int i = 0; i < _output.Count; i++)
            {
                //Do summin with the selected username string
                MessageBox.Show(_output[i].Value);
            }
        }

        private void GetUsers()
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
            {
                // create a user principal object to search for all users
                using (UserPrincipal userPrincipal = new UserPrincipal(context))
                {
                    userPrincipal.Enabled = true; // only enabled users

                    // create a principal searcher to search for all users
                    using (PrincipalSearcher searcher = new PrincipalSearcher(userPrincipal))
                    {
                        // loop through all users and print their names
                        foreach (Principal result in searcher.FindAll())
                        {
                            _viewinfo?.addUser(new User() { name = result.Name });
                        }
                    }
                }
            }
        }
        private void LoadCompleted(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewinfo = VIEWINFOS;
            _viewinfo.Title = "Registering account for kiosk";

            GetUsers();
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        private const uint EWX_LOGOFF = 0x00000000;
        private const uint EWX_FORCE = 0x00000004;

        private void btnUserSelected(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            dlg.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            dlg.FilterIndex = 2;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                if (dlg.FileName != null && _viewinfo != null && !string.IsNullOrEmpty(_viewinfo.SelectedUser))
                {
                    // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon
                    if (writeToRegedit(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "Shell",dlg.FileName))
                    {
                        if (writeToRegedit(Registry.CurrentUser, @"Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "Explorer", 0))
                        {
                            if (writeToRegedit(Registry.CurrentUser, @"Software\Microsoft\Windows NT\kiosk", "user", _viewinfo.SelectedUser))
                            {
                                MessageBox.Show("Switch to the kiosk User :" + _viewinfo?.SelectedUser);
                                ExitWindowsEx(EWX_LOGOFF, 0);

                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Specify the correct user and a exe file.");
                }
            }


            //filepath = File.ReadAllText(openFileDialog.FileName);
            //MessageBox.Show(_viewinfo?.SelectedUser);
            //MessageBox.Show(openFileDialog.FileName);
        }

        private bool writeToRegedit(RegistryKey rk, string keypath, string key, object value)
        {
            try
            {
                RegistryKey sk = rk.CreateSubKey(keypath);
                sk.SetValue(key, value);
                rk.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("error while Writing value in registry \n" + e.Message);
                rk.Close();
                return false;
            }
        }

        private string? readFromRegedit(string key, string keyPath = @"SOFTWARE\kiosk\Settings")
        {
            // saved value at HKEY_LOCAL_MACHINE\SOFTWARE\kiosk\Settings

            RegistryKey rk = Registry.LocalMachine;
            RegistryKey? sk1 = rk.OpenSubKey(keyPath);

            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    return sk1?.GetValue(key)?.ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("Reading registry " + key.ToUpper());
                    return null;
                }
            }
        }
    }

    public class User : INotifyPropertyChanged
    {
        private string? _name;

        public string? name
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

        public override string? ToString()
        {
            return name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class StartPageData : ViewDataObj
    {
        private string? _title;

        private string? _selectedUser;

        private ObservableCollection<User> _users = new ObservableCollection<User>();

        public StartPageData()
        {
        }

        public string? SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); }
        }


        public string? Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public void addUser(User _user)
        {
            _users.Add(_user);
        }

        public void clear()
        {
            _users.Clear();
        }


    }
}
