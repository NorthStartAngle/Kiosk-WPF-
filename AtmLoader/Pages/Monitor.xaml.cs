using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using AtmLib.Tracing;

namespace AtmLoader.Pages {
    /// <summary>
    /// Interaction logic for Monitor.xaml
    /// </summary>
    public partial class Monitor : Page {
        DeviceManager DevMng { get; set; } = new DeviceManager();

        private System.Windows.Threading.DispatcherTimer CpuTimer;
        private TimeSpan LastProcessorTime;
        private DateTime LastProcTime;


        public Monitor() {
            using (var log = new Logger(TraceLevel.Verbose, "Monitor.Monitor"))
            {
                InitializeComponent();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                using (var log = new Logger(TraceLevel.Verbose, "Monitor.Page_Loaded",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
                {
                    DevMng.Changed += OnDevicesChangedAsync;
                    DevMng.Start();
                    // Start Mointor
                    OnStartMonitor(null, null);

                    log.WriteLine(TraceLevel.Verbose, "Device manager started monitoring devices.");

                    // Init Cpu Timer
                    CpuTimer = new System.Windows.Threading.DispatcherTimer();
                    CpuTimer.Tick += new EventHandler(OnCpuTimerEvent);
                    CpuTimer.Interval = TimeSpan.FromMilliseconds(500);
                    CpuTimer.Start();

                    LastProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
                    LastProcTime = DateTime.Now;

                    log.WriteLine(TraceLevel.Verbose, "Cpu timer initialized and started calculating the cpu speed.");
                }
            }));
        }

        void OnDevicesChangedAsync(object sender, EventArgs e) {
            Dispatcher.BeginInvoke(new Action(() => {
                using (var log = new Logger(TraceLevel.Verbose, "Monitor.OnDevicesChangedAsync",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
                {
                    DevMng.Devices.Sort();
                    FilterDeviceList();

                    log.WriteLine(TraceLevel.Verbose, "The device list has been changed and displayed on the page.");
                }
            }), DispatcherPriority.Background);

        }
        private void OnDeviceSelectionChanged(object sender, SelectionChangedEventArgs e) {
            using (var log = new Logger(TraceLevel.Verbose, "Monitor.OnDeviceSelectionChanged",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {

                if (ViewModel != null)
                {
                    Device dev = ViewModel.SelectedDevice;

                    if (dev == null)
                    {
                       log.WriteLine(TraceLevel.Warning, "The user selected device is null.");
                        return;
                    }

                    ViewModel.DevicePropList = dev.Props;
                    log.WriteLine(TraceLevel.Verbose, $"The props of the selected device [{dev.Name}] has been displayed in the list.");
                }
                else
                    log.WriteLine(TraceLevel.Warning, "ViewModel is null.");
            }
        }

        private void OnSearch(object sender, TextChangedEventArgs e) {
            using (var log = new Logger(TraceLevel.Verbose, "Monitor.OnSearch",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {
                ViewModel.SearchText = ((TextBox)sender).Text;
                FilterDeviceList();

                log.WriteLine(TraceLevel.Verbose, $"The device list has been filtered with '{ViewModel.SearchText}'.");
            }
        }

        public void FilterDeviceList() {
            ObservableCollection<Device> FilteredDevices = new ObservableCollection<Device>(DevMng.Devices);

            List<Device> TempFiltered;
            TempFiltered = DevMng.Devices.Where(device =>
                (ViewModel.SearchText == "" || device.Name.ToLower().Contains(ViewModel.SearchText.ToLower()/*, StringComparison.InvariantCultureIgnoreCase*/))).ToList();

            ViewModel.DeviceList = TempFiltered;
        }

        public void OnStartMonitor(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                using (var log = new Logger(TraceLevel.Verbose, "Monitor.OnStartMonitor",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
                {
                    int interval = int.Parse(ViewModel.Interval);
                    interval *= 1000;

                    DevMng.Start(interval);

                    log.WriteLine(TraceLevel.Verbose, $"Started monitoring the devices every {interval} ms.");

                    ViewModel.StartButton_Enabled = false;
                    ViewModel.StopButton_Enabled = true;
                }
            }), DispatcherPriority.Background);
            
        }

        public void OnStopMointor(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke(new Action(() => {
                using (var log = new Logger(TraceLevel.Verbose, "Monitor.OnStopMointor",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
                {
                    DevMng.Stop();

                    ViewModel.StartButton_Enabled = true;
                    ViewModel.StopButton_Enabled = false;

                    log.WriteLine(TraceLevel.Verbose, $"Stopped monitoring the devices.");
                }
            }), DispatcherPriority.Background);
        }

        public void OnCpuTimerEvent(object source, EventArgs e) {
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
            using (var log = new Logger(TraceLevel.Verbose, "Monitor.DeviceListBox_KeyDown",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {

                if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.C)
                {
                    ListBox list = (ListBox)sender;
                    string str = "";
                    if (list != null)
                    {
                        foreach (var item in list.SelectedItems)
                        {
                            Device dev = item as Device;

                            if (dev != null)
                            {
                                str += dev.Name + "\n";
                            }
                        }
                    }

                    Clipboard.SetText(str);
                    log.WriteLine(TraceLevel.Verbose, $"{str} has been copied from device list to clipboard.");
                }
            }
        }

        private void DevicePropListBox_KeyDown(object sender, KeyEventArgs e) {
            using (var log = new Logger(TraceLevel.Verbose, "Monitor.DevicePropListBox_KeyDown",
                new string[] { nameof(sender), nameof(e) },
                new object[] { sender, e }))
            {
                if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.C)
                {
                    ListBox list = (ListBox)sender;
                    string str = "";
                    if (list != null)
                    {
                        foreach (var item in list.SelectedItems)
                        {
                            KeyValuePair<string, object>? p = item as KeyValuePair<string, object>?;
                            if (p != null)
                            {
                                KeyValuePair<string, object> pair = (KeyValuePair<string, object>)p;
                                str += $"{pair.Key}:{pair.Value}\n";
                            }
                        }
                    }

                    Clipboard.SetText(str);
                    log.WriteLine(TraceLevel.Verbose, $"{str} has been copied from device prop list to clipboard.");
                }
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
