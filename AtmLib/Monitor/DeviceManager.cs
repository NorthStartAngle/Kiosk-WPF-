using AtmLib.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;


namespace AtmLib.Monitor
{
    public delegate void Eventhandler(object sender, EventArgs args);

    public class DeviceManager
    {
        public event EventHandler Changed;    // the Event

        private DeviceEnumeration DevEnum = new DeviceEnumeration();

        // Device Watcher
        public List<Device> Devices = new List<Device>();
        public static bool IsEnumCompleted = false;
        public Log DevLog = new Log();

        public System.Timers.Timer LogTimer;

        ApiConnector apiConnector = new ApiConnector();

        public DeviceManager()
        {
            using (var log = new Logger(TraceLevel.Verbose, "DeviceManager.DeviceManager"))
            {
                // Start watching devices
                LogTimer = new System.Timers.Timer();
                LogTimer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
                LogTimer.Interval = 10000;
                LogTimer.Enabled = false;

                apiConnector.RegisterAtm();
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
            }
        }

        public void OnTimerEvent(object source, ElapsedEventArgs e)
        {
            CheckDevices();
        }

        // Start watching device changes
        public void Start( int logInterval = 10000 )
        {
            using (var log = new Logger(TraceLevel.Verbose, "DeviceManager.Start"))
            {
                LogTimer.Interval = logInterval;
                LogTimer.Enabled = true;

                CheckDevices();

                log.WriteLine(TraceLevel.Verbose, $"Start monitoring every {logInterval} ms.");
            }
        }

        public void CheckDevices()
        {
            using (var log = new Logger(TraceLevel.Verbose, "DeviceManager.CheckDevices"))
            {
                List<Device> devs = DevEnum.EnumerateDevices();

                foreach (Device dev in devs)
                {
                    Device device = Devices.Find((dv) => dv.Id == dev.Id);
                    if (device == null)   // New device
                    {
                        DevLog.NewDevices.Add(dev);
                    }
                    else if (!device.Equals(dev)) // Update
                    {
                        DevLog.UpdateDevices.Add(dev);
                    }
                }

                foreach (Device dev in Devices)
                {
                    Device device = devs.Find((dv) => dv.Id == dev.Id);
                    if (device == null)   // Remove
                    {
                        DevLog.RemoveDevices.Add(dev);
                    }
                }

                Devices = devs;
                DispatchChangeEvent();

                // Put Json Log
                string logs = DevLog.PutLog();
                string guid = Log.GetDeviceGuid();

                if (guid != null)
                {
                    // Send log to server
                    apiConnector.SendLogToServer(guid, logs);
                }
            }
        }

        // Stop watching device changes
        public void Stop()
        {
            using (var log = new Logger(TraceLevel.Verbose, "DeviceManager.Stop"))
            {
                LogTimer.Enabled = false;
                log.WriteLine(TraceLevel.Verbose, $"Stop monitoring.");
            }
        }

        void DispatchChangeEvent()
        {
            if ( Changed != null){
                EventHandler handler = Changed;
                if (handler != null) {
                    handler(this, EventArgs.Empty);
                }
            }
        }


    }
}
