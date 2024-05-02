using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using AtmLib.Monitor;
using AtmCommon;

namespace AtmUi.Pages {
    /// <summary>
    /// Interaction logic for Monitor.xaml
    /// </summary>
    public partial class Monitor : Page, IAtmNavigation {
        DeviceManager DevMng { get; set; } = new DeviceManager();

        private System.Windows.Threading.DispatcherTimer CpuTimer;
        private TimeSpan LastProcessorTime;
        private DateTime LastProcTime;


        public Monitor() {
            InitializeComponent();

            DevMng.Changed += OnDevicesChangedAsync;
            DevMng.Start();

            // Start Mointor
            OnStartMonitor(null, null);

            // Init Cpu Timer
            CpuTimer = new System.Windows.Threading.DispatcherTimer();
            CpuTimer.Tick += new EventHandler(OnCpuTimerEvent);
            CpuTimer.Interval = TimeSpan.FromMilliseconds(500);
            CpuTimer.Start();

            LastProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
            LastProcTime = DateTime.Now;
        }


        void OnDevicesChangedAsync(object? sender, EventArgs args) {
            Dispatcher.BeginInvoke(new Action(() => {
                DevMng.Devices.Sort();
                FilterDeviceList();
            }), DispatcherPriority.Background);

        }
        private void OnDeviceSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ViewModel != null) {
                Device? dev = ViewModel.SelectedDevice;

                if (dev == null) {
                    return;
                }

                ViewModel.DevicePropList = dev.Props;
            }
        }

        private void OnSearch(object sender, TextChangedEventArgs e) {
            ViewModel.SearchText = ((TextBox)sender).Text;
            FilterDeviceList();
        }

        public void FilterDeviceList() {
            ObservableCollection<Device> FilteredDevices = new ObservableCollection<Device>(DevMng.Devices);

            List<Device> TempFiltered;
            TempFiltered = DevMng.Devices.Where(device =>
                (ViewModel.SearchText == "" || device.Name.Contains(ViewModel.SearchText, StringComparison.InvariantCultureIgnoreCase))).ToList();

            foreach (Device Device in TempFiltered)
            {

            }

            ViewModel.DeviceList = TempFiltered;
        }

        public void OnStartMonitor(object? sender, RoutedEventArgs? e) {
            Dispatcher.BeginInvoke(new Action(() => {
                int interval = int.Parse(ViewModel.Interval);
                interval *= 1000;

                DevMng.Start(interval);

                ViewModel.StartButton_Enabled = false;
                ViewModel.StopButton_Enabled = true;
            }), DispatcherPriority.Background);
        }

        public void OnStopMointor(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke(new Action(() => {
                DevMng.Stop();

                ViewModel.StartButton_Enabled = true;
                ViewModel.StopButton_Enabled = false;
            }), DispatcherPriority.Background);
        }

        public void OnCpuTimerEvent(object? source, EventArgs e) {
            Process process = Process.GetCurrentProcess();
            TimeSpan procTime = process.TotalProcessorTime;
            var cpuUsage = (procTime - LastProcessorTime).TotalMilliseconds;
            var timeDiff = (DateTime.Now - LastProcTime).TotalMilliseconds;

            var cpuPercentage = (timeDiff > 0) ? cpuUsage / (timeDiff * Environment.ProcessorCount) * 100 : 0;

            process.Refresh();
            var ram = (float)process.WorkingSet64 / 1024 / 1024; // RAM Usage in Mega Bytes

            string cpuRamTxt = $"CPU: {cpuPercentage.ToString("F")}% RAM: {ram.ToString("F")} MB";

            ViewModel.CpuRamUsage = cpuRamTxt;

            LastProcessorTime = procTime;
            LastProcTime = DateTime.Now;
        }

        public ViewModels.Monitor ViewModel {
            get => (ViewModels.Monitor)DataContext;
        }

        public static string Path { get; } = $"Pages/Monitor.xaml";
        public static string LinkName { get; } = "Monitor";
        public static bool IsNavigationEnabled { get; } = true;

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        private void DeviceListBox_KeyDown(object sender, KeyEventArgs e) {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.C) {
                ListBox list = (ListBox)sender;
                string str = "";
                if (list != null) {
                    foreach (var item in list.SelectedItems) {
                        Device? dev = item as Device;

                        if (dev != null) {
                            str += dev.Name + "\n";
                        }
                    }
                }

                Clipboard.SetText(str);
            }
        }

        private void DevicePropListBox_KeyDown(object sender, KeyEventArgs e) {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.C) {
                ListBox list = (ListBox)sender;
                string str = "";
                if (list != null) {
                    foreach (var item in list.SelectedItems) {
                        KeyValuePair<string, object>? p = item as KeyValuePair<string, object>?;
                        if (p != null) {
                            KeyValuePair<string, object> pair = (KeyValuePair<string, object>)p;
                            str += $"{pair.Key}:{pair.Value}\n";
                        }
                    }
                }

                Clipboard.SetText(str);
            }
        }

        private void Blank_GotFocus(object sender, RoutedEventArgs e) {
            TouchKeyboard.FocusedControl = null;
            TouchKeyboard.Visibility = Visibility.Hidden;
        }

        private void RegisterKeyboardControl(object sender, RoutedEventArgs e) {
            if (sender is Control control) {
                TouchKeyboard.RegisterControl(control);
            }
        }
    }
}
