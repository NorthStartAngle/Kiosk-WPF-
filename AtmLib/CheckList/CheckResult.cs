using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace AtmLib.CheckList
{
    public class CheckResult
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public Boolean IsChecked { get; set; } = false;

        public Version RequiredVersion { get; set; } = new Version();
        public Version CurrentVersion { get; set; } = new Version();
        public object Details { get; set; } = null;
    }

    public class DotNetPackage
    {
        public string Name { get; set; } = "";
        public Version Ver { get; set; } = new Version();
        public string Path { get; set; } = "";
    }

    public class AppModule : IComparable
    {
        public string Name { get; set; } = "";
        public int HashCode { get; set; } = 0;
        public string ValidString { get; set; } = "";
        public Boolean IsValid { get; set; } = false;
        public string Path { get; set; } = "";
        public string MemorySize { get; set; } = "";
        public string Site { get; set; } = "";

        public int CompareTo(object obj)
        {
            if (obj == null) return -1;
            return Name.CompareTo(((AppModule)obj).Name);
        }
    }

    public class CheckStorageItem
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public string TotalSize { get; set; }
        public string FreeSpace { get; set; }
    }

    public class CheckResourceItem
    {
        public string Name { get; set; }
        public string Size { get; set; }
    }
    public class CheckFileItem
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string Path { get; set; }
    }

    public class CheckPermissionItem
    {
        public string Name { get; set; }
    }

    public class CheckInternetResult
    {
        public List< CheckInternetInterfaceItem > NetworkInterfaces { get; set; }
        public IPAddress[] IPAddresses { get; set; }
        public Boolean IsNetworkAvailable { get; set; } = false;
        public Boolean IsServerAvailable { get; set; } = false;
        public PingReply ServerPingReply { get; set; }
        public string ServerIP { get; set; }
        public string ServerHostName { get; set; }
        public string ServerStatus { get; set; }
    }

    public class CheckInternetInterfaceItem
    {
        public string Name { get; set; }
        public NetworkInterfaceType Type { get; set; }
        public string Description { get; set; }
        public OperationalStatus Status { get; set; }

        public string Speed { get; set; }
        public string GatewayAddresses { get; set; }
        public string DnsAddresses { get; set; }
        public PhysicalAddress PhysicalAddr { get; set; }
        
    }

    public class CheckUpdateItem
    {
        public Version TargetVersion { get; set; }
        public Version CurrentVersion { get; set; }
        public string Description { get; set; }
        public DateTime UpdateTime { get; set; }
    }

}
