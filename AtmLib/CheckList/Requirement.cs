using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Globalization;
using System.Resources;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Principal;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;
using System.Linq;
using AtmLib.Monitor;
using System.Threading.Tasks;
using AtmLib.Tracing;

namespace AtmLib.CheckList
{
    public class Requirement
    {
        static public readonly string InstallDotNetFile = "./dotnet-install.ps1";
        static public string TargetDotNetName { get; set; } = "Microsoft.NETCore.App";

        static private Version _targetDotNetVersion = new Version("7.0.5");
        static public Version TargetDotNetVersion
        {
            get => _targetDotNetVersion;
            set { _targetDotNetVersion = value; }
        }

        /* -- Check and Install DOT NET Version -- */
        static public CheckResult CheckDotNet()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.CheckDotNet"))
            {
                log.WriteLine(TraceLevel.Verbose, "Component is initialized");
                // Available .net packages
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd";
                startInfo.Arguments = "/c dotnet --list-runtimes";
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                process.StartInfo = startInfo;
                process.Start();

                Version maxVersion = null;
                string output = process.StandardOutput.ReadToEnd();
                if (!string.IsNullOrEmpty(output))
                {
                    var strs = output.Split('\n');
                    foreach (var str in strs)
                    {
                        var pathStartIndex = str.IndexOf(" [");
                        if (pathStartIndex < 0)
                            continue;

                        string name = str.Substring(0, pathStartIndex);
                        var subStrs = name.Split(' ');
                        name = subStrs[0];
                        Version version = new Version(subStrs[1]);

                        if (name == TargetDotNetName && (maxVersion == null || version > maxVersion))
                        {
                            maxVersion = version;
                        }
                    }
                }

                CheckResult item = new CheckResult();
                item.Title = ".Net";
                item.CurrentVersion = maxVersion;
                item.RequiredVersion = TargetDotNetVersion;

                if (maxVersion == null)
                {
                    item.Description = $"{TargetDotNetName} not found. You need at least {TargetDotNetName} {TargetDotNetVersion}.";
                    item.IsChecked = false;
                }
                else
                {
                    item.Description = $".Net is {TargetDotNetName} {maxVersion.ToString()}.";

                    if (maxVersion < TargetDotNetVersion)
                    {
                        item.Description += $" You need at least {TargetDotNetName} {TargetDotNetVersion}.";
                        item.IsChecked = false;
                    }
                    else
                    {
                        item.IsChecked = true;
                    }
                }

                log.WriteLine(TraceLevel.Verbose, $"{item.Title}:{item.Description}");
                return item;
            }
        }

        static public Boolean InstallDotNet()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.InstallDotNet"))
            {
                Process process = RunPS($"{InstallDotNetFile} -Runtime dotnet -Version {TargetDotNetVersion}");
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();
                if (errors != "")
                {
                    log.WriteLine(TraceLevel.Error, errors);
                    return false;
                }
                else
                {
                    log.WriteLine(TraceLevel.Verbose, output);
                    return true;
                }
            }
        }

        static public Process RunPS( string ps )
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.RunPS"))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = @"powershell.exe";
                startInfo.Arguments = $"'{ps}'";
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = false;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                log.WriteLine(TraceLevel.Verbose, $"{ps} executed.");
                return process;
            }
        }

        /* --- Check OS Version --- */

        static public string TargetOSName { get; set; } = "Windows";
        static public string UpdateOSPS { get; set; } = "Install-Module PSWindowsUpdate;\nGet-WindowsUpdate;\nInstall-WindowsUpdate;";

        static private Version _targetOSVersion = new Version("10.0");
        static public Version TargetOSVersion
        {
            get => _targetOSVersion;
            set { _targetOSVersion = value; }
        }

        static public object HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return rk.GetValue(key);
            }
            catch ( ArgumentNullException e )
            {
                Utility.LogException(e);
            }
            catch ( Exception e )
            {
                Utility.LogException(e);
            }

            return "";
        }

        static public CheckResult CheckOS()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.CheckOS"))
            {
                // OS Information
                string ProductName = (string)HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
                string CSDVersion = (string)HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
                string CurrentBuild = (string)HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuild");
                int MajorVersion = (int)HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentMajorVersionNumber");
                int MinorVersion = (int)HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentMinorVersionNumber");
                string EditionID = (string)HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID");
                string InstallationType = (string)HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "InstallationType");

                var osName = $"Windows {EditionID} {MajorVersion}.{MinorVersion} {CurrentBuild}";
                Version vs = new Version(MajorVersion, MinorVersion);

                var item = new CheckResult();

                item.Title = "OS";

                if (vs < TargetOSVersion)
                {
                    item.IsChecked = false;
                    item.Description = $"The system is {osName}. You need at least {TargetOSName} {TargetOSVersion}.";
                }
                else
                {
                    item.IsChecked = true;
                    item.Description = $"The system is {osName}.";
                }

                item.Details = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object> ( "ProductName", ProductName ),
                    new KeyValuePair<string, object> ( "CSDVersion", CSDVersion ),
                    new KeyValuePair<string, object> ( "CurrentBuild", CurrentBuild ),
                    new KeyValuePair<string, object> ( "MajorVersion", MajorVersion ),
                    new KeyValuePair<string, object> ( "MinorVersion", MinorVersion ),
                    new KeyValuePair<string, object> ( "EditionID", EditionID ),
                    new KeyValuePair<string, object> ( "InstallationType", InstallationType ),
                };

                log.WriteLine(TraceLevel.Verbose, item.Description);
                return item;
            }
        }

        static public Boolean UpdateOS()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.UpdateOS"))
            {
                Process process = RunPS(UpdateOSPS);
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();
                if (errors != "")
                {
                    log.WriteLine(TraceLevel.Error, errors);
                    return false;
                }
                else
                {
                    log.WriteLine(TraceLevel.Verbose, output);
                    return true;
                }
            }
        }

        /* ---- Check Memory --- */
        static public long TargetMemory{ get; set; } = 1 * 1024 * 1024 * 1024;       // RAM size in Bytes

        static public CheckResult CheckMemory()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.UpdateOS"))
            {
                ManagementObjectSearcher Search = new ManagementObjectSearcher();
                Search.Query = new ObjectQuery("Select * From Win32_ComputerSystem");
                long totalMemory = 0;
                string model = null;

                foreach (ManagementObject obj in Search.Get())
                {
                    if (obj["TotalPhysicalMemory"] != null)
                    {
                        totalMemory = (long)Math.Round(Convert.ToDouble(obj["TotalPhysicalMemory"]));
                        model = obj["Model"]?.ToString();
                        break;
                    }
                }

                CheckResult item = new CheckResult();
                item.Title = "Memory";
                if (totalMemory >= TargetMemory)
                {
                    item.IsChecked = true;
                    item.Description = $"Physical memory size is {SizeToString(totalMemory)}.";
                }
                else
                {
                    item.IsChecked = false;
                    item.Description = $"Physical memory size is {SizeToString(totalMemory)}. You need at least {SizeToString(TargetMemory)}.";
                }

                item.Details = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object> ("Physical memory size", SizeToString(totalMemory) ),
                    new KeyValuePair<string, object> ("Model", model),
                };

                log.WriteLine(TraceLevel.Verbose, item.Description);
                return item;
            }
        }

        /* --- Check Storage --- */
        static public long TargetStorage { get; set; } = 100 * 1024 * 1024;       // Free Space in Bytes

        static public CheckResult CheckStorage()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.CheckStorage"))
            {
                string workingDirectory = Environment.CurrentDirectory;
                CheckResult item = new CheckResult();
                long freeSpace = 0;

                foreach (DriveInfo info in DriveInfo.GetDrives())
                {
                    if (workingDirectory.StartsWith(info.Name))
                    {
                        freeSpace = info.TotalFreeSpace;
                    }
                }

                item.Title = "Persistent Storage";
                if (freeSpace >= TargetStorage)
                {
                    item.IsChecked = true;
                    item.Description = $"Available free space is {SizeToString(freeSpace)}.";
                }
                else
                {
                    item.IsChecked = false;
                    item.Description = $"Available free space is {SizeToString(freeSpace)}. You need at least {SizeToString(TargetStorage)}.";
                }

                item.Details = DriveInfo.GetDrives();

                log.WriteLine(TraceLevel.Verbose, item.Description);
                return item;
            }
        }

        /* --- Check Libraries --- */
        static public CheckResult CheckLibrary()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.CheckLibrary"))
            {
                int invalidModules = 0;

                List<AppModule> modules = new List<AppModule>();
                foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
                {
                    modules.Add(new AppModule
                    {
                        Name = module.ModuleName,
                        HashCode = module.GetHashCode(),
                        IsValid = module.EntryPointAddress != null,
                        ValidString = module.EntryPointAddress != null ? "Valid" : "Invalid",
                        Path = module.FileName,
                        MemorySize = $"{SizeToString(module.ModuleMemorySize)}",
                        Site = (module.Site != null) ? module.Site.Name : ""
                    });

                    if (module.EntryPointAddress == null)
                    {
                        invalidModules++;
                    }
                }

                modules.Sort();

                CheckResult item = new CheckResult();
                item.Title = "Library";
                if (invalidModules == 0)
                {
                    item.IsChecked = true;
                    item.Description = $"{modules.Count} modules loaded.";
                }
                else
                {
                    item.IsChecked = false;
                    item.Description = $"{modules.Count} modules loaded. {invalidModules} modules are invalid.";
                }

                item.Details = modules;

                log.WriteLine(TraceLevel.Verbose, item.Description);
                return item;
            }
        }

        /* --- Check Resources --- */
        static public CheckResult CheckResource()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.CheckResource"))
            {
                var resourceManager = new ResourceManager("AtmLoader.g", Assembly.GetEntryAssembly());
                var resources = resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true,
            true);

                List<CheckResourceItem> details = new List<CheckResourceItem>();
                foreach (DictionaryEntry res in resources)
                {
                    details.Add(new CheckResourceItem
                    {
                        Name = (string)res.Key,
                        Size = SizeToString(((UnmanagedMemoryStream)res.Value).Capacity)
                    });
                }

                CheckResult item = new CheckResult();
                item.Title = "Resources";
                if (details.Count > 0)
                {
                    item.IsChecked = true;
                    item.Description = $"{details.Count} resources loaded.";
                }
                else
                {
                    item.IsChecked = false;
                    item.Description = $"No resources loaded.";
                }

                item.Details = details;

                log.WriteLine(TraceLevel.Verbose, item.Description);
                return item;
            }
        }
        
        /* --- Check Files --- */
        static public CheckResult CheckFile()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.CheckFile"))
            {
                string path = Environment.CurrentDirectory;
                var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories);

                List<CheckFileItem> details = new List<CheckFileItem>();
                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    details.Add(new CheckFileItem
                    {
                        Name = (string)fileInfo.Name,
                        Size = SizeToString(fileInfo.Length),
                        Path = fileInfo.Directory.FullName
                    });
                }

                CheckResult item = new CheckResult();
                item.Title = "Files";
                if (details.Count > 0)
                {
                    item.IsChecked = true;
                    item.Description = $"{details.Count} files found in {path}.";
                }
                else
                {
                    item.IsChecked = false;
                    item.Description = $"No files found  in {path}.";
                }

                item.Details = details;

                log.WriteLine(TraceLevel.Verbose, item.Description);
                return item;
            }
        }

        /* --- Check Permission --- */
        static public WindowsBuiltInRole TargetPermission = WindowsBuiltInRole.User;
        static public CheckResult CheckPermission()
        {

            using (var log = new Logger(TraceLevel.Verbose, "Requirement.CheckPermission"))
            {
                AppDomain myDomain = Thread.GetDomain();

                myDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                WindowsPrincipal myPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
                Console.WriteLine("{0} belongs to: ", myPrincipal.Identity.Name.ToString());
                Array wbirFields = Enum.GetValues(typeof(WindowsBuiltInRole));

                List<CheckPermissionItem> details = new List<CheckPermissionItem>();
                string roles = "";

                foreach (object roleName in wbirFields)
                {
                    try
                    {
                        if (myPrincipal.IsInRole((WindowsBuiltInRole)roleName))
                        {
                            details.Add(new CheckPermissionItem { Name = $"{roleName}" });
                            roles += $"{roleName} ";
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                CheckResult item = new CheckResult();
                item.Title = "Permission";

                if (myPrincipal.IsInRole(TargetPermission))
                {
                    item.IsChecked = true;
                    item.Description = $"The account belongs to {myPrincipal.Identity.Name} and runs as {roles}.";
                }
                else
                {
                    item.IsChecked = false;
                    item.Description = $"The account belongs to {myPrincipal.Identity.Name} and runs as {roles}. You need at least {TargetPermission} role.";
                }

                item.Details = details;

                log.WriteLine(TraceLevel.Verbose, item.Description);
                return item;
            }
        }

        /* --- Check Internet Connection --- */
        static public string ServerName { get; set; } = "https://secure.just.cash";
        static public int ServerPingTimeout { get; set; } = 5 * 1000;   // 5 secs

        static public CheckResult CheckInternet()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.CheckInternet"))
            {
                CheckInternetResult res = new CheckInternetResult();

                res.NetworkInterfaces = new List<CheckInternetInterfaceItem>();
                foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    IPInterfaceProperties ipProps = adapter.GetIPProperties();
                    IPv4InterfaceStatistics stats = adapter.GetIPv4Statistics();

                    CheckInternetInterfaceItem item = new CheckInternetInterfaceItem
                    {
                        Name = adapter.Name,
                        Type = adapter.NetworkInterfaceType,
                        Description = adapter.Description,
                        Status = adapter.OperationalStatus,
                        Speed = SizeToString(adapter.Speed) + "S",
                        PhysicalAddr = adapter.GetPhysicalAddress(),
                    };

                    foreach (GatewayIPAddressInformation gateway in ipProps.GatewayAddresses)
                    {
                        item.GatewayAddresses += $"{gateway.Address} ";
                    }

                    foreach (IPAddress ipAddr in ipProps.DnsAddresses)
                    {
                        item.DnsAddresses += $"{ipAddr} ";
                    }

                    res.NetworkInterfaces.Add(item);

                }

                // IP Addresses
                res.IPAddresses = Dns.GetHostAddresses(Dns.GetHostName());

                // Check Network Available
                res.IsNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();

                // Check If a server is available
                ApiConnector apiConnector = new ApiConnector();
                var task = Task.Run(() => apiConnector.CheckServerStatus());
                task.Wait();
                Boolean isServerAlive = task.Result;

                res.IsServerAvailable = isServerAlive;
                res.ServerHostName = ServerName;
                res.ServerStatus = isServerAlive ? "Alive" : "Unavailable";

                // Make check result
                CheckResult checkResult = new CheckResult();
                checkResult.Title = "Internet";

                checkResult.IsChecked = res.IPAddresses.Length > 0 && res.IsNetworkAvailable && res.IsServerAvailable;

                if (res.IPAddresses.Length > 0)
                {
                    checkResult.Description += $"IP Address: Available ";
                }
                else
                {
                    checkResult.Description += $"IP Address: Unvaliable ";
                }

                if (res.IsNetworkAvailable)
                {
                    checkResult.Description += $"Network: Available ";
                }
                else
                {
                    checkResult.Description += $"Network: Unavailable ";
                }

                if (res.IsServerAvailable)
                {
                    checkResult.Description += $"Server: Available ";
                }
                else
                {
                    checkResult.Description += $"Server: Unavailable ";
                }

                checkResult.Details = res;

                log.WriteLine(TraceLevel.Verbose, checkResult.Description);
                return checkResult;
            }
        }

        /* --- Check Updates -- */
        static public Version TargetAppVersion { get; set; } = new Version("1.0.0.0");
        static public CheckResult CheckUpdate()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.CheckUpdate"))
            {
                CheckResult checkResult = new CheckResult();
                checkResult.Details = new List<CheckUpdateItem>();
                checkResult.Title = "Update";
                checkResult.CurrentVersion = Assembly.GetEntryAssembly().GetName().Version;
                checkResult.IsChecked = checkResult.CurrentVersion >= TargetAppVersion;
                if (checkResult.IsChecked)
                {
                    checkResult.Description = $"Current version is {checkResult.CurrentVersion} (the latest version)";
                }
                else
                {
                    checkResult.Description = $"Current version is {checkResult.CurrentVersion}. The latest version is {TargetAppVersion}.";
                }

                log.WriteLine(TraceLevel.Verbose, checkResult.Description);
                return checkResult;
            }
        }

        /* --- Check Devices --- */
        static public CheckResult CheckDevice()
        {
            using (var log = new Logger(TraceLevel.Verbose, "Requirement.CheckDevice"))
            {
                DeviceEnumeration devEnum = new DeviceEnumeration();
                List<Device> devices = devEnum.EnumerateDevices();
                devices.Sort();

                CheckResult checkResult = new CheckResult
                {
                    Title = "Device",
                    IsChecked = devices.Count > 0,
                    Description = $"{devices.Count} devices installed.",
                    Details = devices
                };

                log.WriteLine(TraceLevel.Verbose, checkResult.Description);
                return checkResult;
            }
        }

        static public string SizeToString( long size )
        {
            if (size / 1024 / 1024 / 1024 > 0)
                return $"{( (double) size / 1024 / 1024 / 1024 ).ToString("F")} GB";
            else if (size / 1024 / 1024 > 0)
                return $"{((double)size / 1024 / 1024 ).ToString("F")} MB";
            else if (size / 1024 > 0)
                return $"{((double)size / 1024 ).ToString("F")} KB";
            else
                return $"{size} Bytes";
        }

        static public string CollectionToString( ICollection iItems)
        {
            string res = "";
            foreach (var item in iItems)
            {
                res += $"{item} ";
            }
            return res;
        }
    }
}
