using System;
using System.Windows.Threading;
using System.Device.Location;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using AtmCommon.ViewModels;

namespace AtmCommon.ViewModels
{
    public class CustomBase :  TemplateViewModel
    {
#if NETCOREAPP3_0_OR_GREATER
        private DispatcherTimer? _timer;
#else
        private DispatcherTimer _timer;
#endif
        private bool _isTimer = true;
        private string strCurrentTime = "";

        public CustomBase() {
            if(_isTimer)
            {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(1);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                _timer.Tick += Timer_Tick;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                _timer.Start();
            }

            GetCurrentLocation();
        }

        public bool IsTimer
        { get { return _isTimer; } set => _isTimer = value; }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the current time property
            strCurrentTime = DateTime.UtcNow.ToString("yyyy-MM-dd H:m:s");
            OnPropertyChanged(nameof(CurrentTime));
        }

        public string CurrentTime
        { get { return strCurrentTime; } }

        private void GetCurrentLocation()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            if (watcher.Status == GeoPositionStatus.Ready)
            {
                watcher.PositionChanged += (sender, e) =>
                {
                    watcher.Stop();
                    Lat = e.Position.Location.Latitude.ToString("{0:0,0.0}");
                    Lon = e.Position.Location.Longitude.ToString("{0:0,0.0}");
                };
                watcher.Start();
                IsSupported = Visibility.Visible;
            }
            else
            {
                SupportGeolocation = "This device doesn`t support Geolocation";
                IsSupported = Visibility.Hidden;
            }
        }

        private Visibility isSupported;
        public Visibility IsSupported
        {
            get { return isSupported; } set { isSupported = value; OnPropertyChanged(nameof(IsSupported)); }
        }

        private string IsSupportGeolocation = "";
        public string SupportGeolocation
        {
            get => IsSupportGeolocation;
            set
            {
                IsSupportGeolocation = value;
                OnPropertyChanged(nameof(SupportGeolocation));
            }
        }
        private string lat="";
        public  string Lat
        { get => lat; set { lat = value; OnPropertyChanged(nameof(Lat)); } }

        private string lon="";
        public string Lon
        { get { return lon; } set { lon = value; OnPropertyChanged(nameof(Lon)); } }

    }
}
