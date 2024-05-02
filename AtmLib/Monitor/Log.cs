using AtmLib.Tracing;
using Microsoft.Win32;

// using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.Json;
using System.Timers;

namespace AtmLib.Monitor
{
    public class Log
    {
        public List<Device> NewDevices = new List<Device>();
        public List<Device> UpdateDevices = new List<Device>();
        public List<Device> RemoveDevices = new List<Device>();

        // public JSONLog jSONLog = new JSONLog();

        private Boolean FirstLogging = true;               // Whether or not, first logging to the file
        public System.Timers.Timer LogTimer;
        private const string LogFilePath = "log\\log.json";

        public Log()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Log.Log"))
            {
                LogTimer = new System.Timers.Timer();
                LogTimer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
                LogTimer.Interval = 10000;
                LogTimer.Enabled = false;

                log.WriteLine(TraceLevel.Verbose, "Component is initialized.");
            }
        }

        public void Start( int interval = 10000 )
        {
            using (var log = new Logger(TraceLevel.Verbose, "Log.Start"))
            {
                LogTimer.Interval = interval;
                LogTimer.Enabled = true;
                log.WriteLine(TraceLevel.Verbose, $"Start logging every {interval} ms.");
            }
        }

        public void Stop() { LogTimer.Enabled = false;}

        public string GetJSONLog()
        {
            return "";
        }

        public string PutLog()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Log.PutLog"))
            {
                // Get Logs
                JSONLogItem logs = new JSONLogItem();
                logs.Time = DateTime.UtcNow.ToString("G");
                logs.Guid = GetDeviceGuid();

                bool changed = NewDevices.Count > 0 || UpdateDevices.Count > 0 || RemoveDevices.Count > 0;
                logs.Status = changed ? "Changed" : "Constant";

                if (changed)
                {
                    List<JSONLogDevice> lDevices = new List<JSONLogDevice>();
                    foreach (Device dev in NewDevices)
                    {
                        lDevices.Add(new JSONLogDevice { Id = dev.Id, Status = "Add", Props = dev.Props.ToDictionary(x => x.Key, x => x.Value) });
                    }
                    foreach (Device dev in UpdateDevices)
                    {
                        lDevices.Add(new JSONLogDevice { Id = dev.Id, Status = "Update", Props = dev.Props.ToDictionary(x => x.Key, x => x.Value) });
                    }
                    foreach (Device dev in RemoveDevices)
                    {
                        lDevices.Add(new JSONLogDevice { Id = dev.Id, Status = "Remove", Props = dev.Props.ToDictionary(x => x.Key, x => x.Value) });
                    }

                    logs.LogDevices = lDevices.ToArray();
                }

                // jSONLog.Logs.Add(log);

                NewDevices.Clear();
                UpdateDevices.Clear();
                RemoveDevices.Clear();

                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(logs, options); // JsonSerializer.Serialize(jSONLog.Logs.ToArray(), options);

                    if (FirstLogging)
                    {
                        string logString = jsonString;
                        if (File.Exists(LogFilePath))
                        {
                            string newName = $"log\\{DateTime.UtcNow.ToString("yyyy_M_d HH_mm_ss")}.json";
                            File.Move(LogFilePath, newName);
                        }
                        else if (!Directory.Exists("log"))
                        {
                            Directory.CreateDirectory("log");
                        }

                        File.WriteAllText(LogFilePath, logString);
                        FirstLogging = false;
                    }
                    else
                    {
                        string logString = ",\n" + jsonString;
                        File.AppendAllText(LogFilePath, logString);
                    }

                    return jsonString;
                }
                catch (NotSupportedException e)
                {
                    Utility.LogException(e);
                }
                catch (IOException e)
                {
                    Utility.LogException(e);
                }
                catch (Exception e)
                {
                    Utility.LogException(e);
                }

                return "";
            }
        }

        public void Clear()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Log.Clear"))
            {
                NewDevices.Clear();
                UpdateDevices.Clear();
                RemoveDevices.Clear();
            }
        }
        public void OnTimerEvent(object source, ElapsedEventArgs e)
        {
            PutLog();
        }

        static public string GetDeviceGuid()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Log.PutLog"))
            {
                string ret = "Invalid";
                RegistryKey regKey = null;

                try
                {
                    RegistryKey localMachine = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    var regDefault = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);

                    if (regDefault != null)
                    {
                        var regVal = regDefault.GetValue("ProductId", "");

                        if (regVal != null)
                        {
                            var tmp = regVal.ToString();

                            if (tmp != null)
                            {
                                ret = tmp;
                            }
                        }

                        regDefault.Close();
                    }
                }
                catch (ArgumentNullException e)
                {
                    Utility.LogException(e);
                }
                catch (ObjectDisposedException e)
                {
                    Utility.LogException(e);
                }
                catch (SecurityException e)
                {
                    Utility.LogException(e);
                }
                catch (Exception e)
                {
                    Utility.LogException(e);
                }
                finally
                {
                    if (regKey != null)
                        regKey.Close();
                }

                log.WriteLine(TraceLevel.Verbose, ret);

                return ret;
            }
        }

    }

    class JSONLog
    {
        public List<JSONLogItem> Logs = new List<JSONLogItem>();
    }

    class JSONLogItem
    {
        public string Time { get; set; }
        public string Guid { get; set; }
        public string Status { get; set; }
        public JSONLogDevice[] LogDevices { get; set; }
    }

    class JSONLogDevice
    {
        public string Id { get; set; }
        public string Status { get; set; }

        public Dictionary< string, object > Props { get; set; } = new Dictionary<string, object> ();
    }

}
