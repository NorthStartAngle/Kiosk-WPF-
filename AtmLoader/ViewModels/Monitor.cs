using AtmLib.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtmCommon.ViewModels;

namespace AtmLoader.ViewModels {
    public class Monitor : Blank {
        private string _interval = "10";
        public string Interval {
            get => _interval;
            set { _interval = value; OnPropertyChanged(); }
        }

        private string _searchText = "";
        public string SearchText {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        private Boolean _startButtonEnabled = true;
        public Boolean StartButton_Enabled {
            get => _startButtonEnabled;
            set { _startButtonEnabled = value; OnPropertyChanged(); }
        }

        private Boolean _stopButtonEnabled = true;
        public Boolean StopButton_Enabled {
            get => _stopButtonEnabled;
            set { _stopButtonEnabled = value; OnPropertyChanged(); }
        }


        private string _cpuRamUsage = "";
        public string CpuRamUsage {
            get => _cpuRamUsage;
            set { _cpuRamUsage = value; OnPropertyChanged(); }
        }

        private List<Device> _deviceList = new List<Device>();
        public List<Device> DeviceList {
            get => _deviceList;
            set { _deviceList = value; OnPropertyChanged(); }
        }

        private Device _selectedDevice = null;
        public Device SelectedDevice {
            get => _selectedDevice;
            set { _selectedDevice = value; OnPropertyChanged(); }
        }

        private List<KeyValuePair<string, object>> _devicePropList = new List<KeyValuePair<string, object>>();
        public List<KeyValuePair<string, object>> DevicePropList {
            get => _devicePropList;
            set { _devicePropList = value; OnPropertyChanged(); }
        }

    }
}
