using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AtmLib.Monitor;
using AtmLib.Tracing;

namespace AtmLib.Monitor
{
    public class DeviceEnumeration
    {

        [DllImport("cfgmgr32.dll", SetLastError = true)]
        static extern uint CM_Enumerate_Classes(
          uint ulClassIndex,
          ref Guid ClassGuid,
          uint flags
        );

                
        [DllImport("CfgMgr32.dll", CharSet = CharSet.Unicode)]
        public static extern uint CM_Get_DevNode_Property(
            uint dnDevInst,
            ref DEVPROPKEY DEVPROPKEY ,
            out uint PropertyType,
            byte[] PropertyBuffer,
            ref uint PropertyBufferSize,
            uint ulFlags);

        [DllImport("Cfgmgr32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern int CM_Get_Device_ID_List_Size(ref uint idListlen, int dnDevInst, uint ulFlags);
        
        [DllImport("Cfgmgr32.dll", CharSet = CharSet.Unicode)]
        static extern int CM_Get_Device_ID_List(string filter, byte[] bffr, uint bffrLen, uint ulFlags);

        [DllImport("Cfgmgr32.dll", CharSet = CharSet.Unicode)]
        static extern int CM_Locate_DevNode(ref int pdnDevInst, string pDeviceID, uint flags);

        [DllImport("Cfgmgr32.dll", CharSet = CharSet.Unicode)]
        static extern int CM_Get_DevNode_Status(out uint status, out uint probNum, uint devInst, uint flags);
        
        [DllImport("Cfgmgr32.dll", CharSet = CharSet.Unicode)]
        static extern int CM_Get_Class_Property(
            ref Guid ClassGUID,
            ref DEVPROPKEY PropertyKey,
            out uint PropertyType,
            byte[] PropertyBuffer,
            ref uint PropertyBufferSize,
            uint flags
        );

        const uint CM_GETIDLIST_FILTER_PRESENT = 0x00000100;
        const int CR_SUCCESS = 0x0;
        public const int CM_LOCATE_DEVNODE_NORMAL = 0x00000000;

        public const uint DEVPROP_TYPE_STRING = 0x12;
        public const uint DEVPROP_TYPE_FILETIME = 0x10;
        public const uint DEVPROP_TYPE_INT32 = 0x06;
        public const uint DEVPROP_TYPE_UINT32 = 0x07;
        public const uint DEVPROP_TYPE_GUID = 0x0D;
        public const uint DEVPROP_TYPE_STRING_LIST = 0x2012;

        public static DEVPROPKEY  PKEY_Device_ClassGuid = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 10);     // DEVPROP_TYPE_GUID
        public static DEVPROPKEY  PKEY_Device_DeviceDesc = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 2);     // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_HardwareIds = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 3);     // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_CompatibleIds = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 4);     // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_Service = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 6);     // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_Class = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 9);     // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_Driver = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 11);    // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_Manufacturer = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 13);    // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_FriendlyName = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 14);    // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_LocationInfo = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 15);    // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_PDOName = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 16);    // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_UINumber = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 18);    // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_UpperFilters = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 19);    // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_LowerFilters = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 20);    // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_EnumeratorName = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 24);    // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_UINumberDescFormat = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 31);    // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_LocationPaths = new DEVPROPKEY (0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 37);    // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_EjectionRelations = new DEVPROPKEY (0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 4);     // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_RemovalRelations = new DEVPROPKEY (0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 5);     // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_PowerRelations = new DEVPROPKEY (0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 6);     // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_BusRelations = new DEVPROPKEY (0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 7);     // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_Parent = new DEVPROPKEY (0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 8);     // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_Children = new DEVPROPKEY (0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 9);     // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_Siblings = new DEVPROPKEY (0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 10);    // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_InstanceId = new DEVPROPKEY (0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 256);   // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_DriverVersion = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 3);      // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_DriverDesc = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 4);      // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_DriverInfPath = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 5);      // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_DriverInfSection = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 6);      // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_DriverInfSectionExt = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 7);      // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_MatchingDeviceId = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 8);      // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_DriverProvider = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 9);      // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_DriverPropPageProvider = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 10);     // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_DriverCoInstallers = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 11);     // DEVPROP_TYPE_STRING_LIST
        public static DEVPROPKEY  PKEY_Device_ResourcePickerTags = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 12);     // DEVPROP_TYPE_STRING
        public static DEVPROPKEY  PKEY_Device_ResourcePickerExceptions = new DEVPROPKEY (0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 13); // DEVPROP_TYPE_STRING
        public static DEVPROPKEY PKEY_Device_Capabilities = new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 17);    // 
        public static DEVPROPKEY PKEY_Device_BusNumber = new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 23);    // DEVPROP_TYPE_UINT32
        public static DEVPROPKEY PKEY_Device_Security= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 25);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public static DEVPROPKEY PKEY_Device_SecuritySDS= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 26);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public static DEVPROPKEY PKEY_Device_DevType= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 27);    // DEVPROP_TYPE_UINT32
        public static DEVPROPKEY PKEY_Device_Exclusive= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 28);    // DEVPROP_TYPE_BOOLEAN
        public static DEVPROPKEY PKEY_Device_Characteristics= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 29);    // DEVPROP_TYPE_UINT32
        public static DEVPROPKEY PKEY_Device_Address= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 30);    // DEVPROP_TYPE_UINT32
        public static DEVPROPKEY PKEY_Device_PowerData= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 32);    // DEVPROP_TYPE_BINARY
        public static DEVPROPKEY PKEY_Device_RemovalPolicy= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 33);    // DEVPROP_TYPE_UINT32
        public static DEVPROPKEY PKEY_Device_RemovalPolicyDefault= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 34);    // DEVPROP_TYPE_UINT32
        public static DEVPROPKEY PKEY_Device_RemovalPolicyOverride= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 35);    // DEVPROP_TYPE_UINT32
        public static DEVPROPKEY PKEY_Device_InstallState= new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 36);    // DEVPROP_TYPE_UINT32
        public static DEVPROPKEY PKEY_Device_BaseContainerId = new DEVPROPKEY(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 38);    // DEVPROP_TYPE_GUID

        public static DEVPROPKEY PKEY_Device_DevNodeStatus = new DEVPROPKEY( 0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 2);     // DEVPROP_TYPE_UINT32
        public static DEVPROPKEY PKEY_Device_ProblemCode = new DEVPROPKEY( 0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7, 3);     // DEVPROP_TYPE_UINT32
        public static DEVPROPKEY PKEY_Device_ModelId = new DEVPROPKEY( 0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b, 2); // DEVPROP_TYPE_GUID
        public static DEVPROPKEY PKEY_Device_ConfigurationId = new DEVPROPKEY( 0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2, 7);    // DEVPROP_TYPE_STRING
        public static DEVPROPKEY PKEY_Device_InstallDate = new DEVPROPKEY( 0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29, 100);   // DEVPROP_TYPE_FILETIME
        public static DEVPROPKEY PKEY_Device_FirstInstallDate = new DEVPROPKEY( 0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29, 101);   // DEVPROP_TYPE_FILETIME
        public static DEVPROPKEY PKEY_Device_LastArrivalDate = new DEVPROPKEY( 0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29, 102);   // DEVPROP_TYPE_FILETIME
        public static DEVPROPKEY PKEY_Device_LastRemovalDate = new DEVPROPKEY(0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29, 103);   // DEVPROP_TYPE_FILETIME
        public static DEVPROPKEY PKEY_Device_DriverDate = new DEVPROPKEY(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6, 2);     // DEVPROP_TYPE_FILETIME

        public List<Device> EnumerateDevices()
        {
            using (var log = new Logger(TraceLevel.Verbose, "EnumerateDevices.EnumerateDevices"))
            {
                List<Device> Devices = new List<Device>();
                Guid guid = new Guid();
                CM_Enumerate_Classes(0, ref guid, 0);

                int cr;
                uint ulLen = 0;

                cr = CM_Get_Device_ID_List_Size(ref ulLen, 0, (uint)CM_GETIDLIST_FILTER_PRESENT);

                if (cr == CR_SUCCESS && ulLen > 1)
                {
                    byte[] pDeviceID = new byte[ulLen * 2];
                    if (CM_Get_Device_ID_List(null, pDeviceID, ulLen * 2, (uint)CM_GETIDLIST_FILTER_PRESENT) == CR_SUCCESS)
                    {
                        // Get Device IDs
                        string idsStr = System.Text.Encoding.Unicode.GetString(pDeviceID);
                        string[] devIds = idsStr.Split('\0');

                        foreach (string ptrID in devIds)
                        {
                            if (ptrID == "")
                                continue;

                            int devinst = 0;
                            if (CM_Locate_DevNode(ref devinst, ptrID, CM_LOCATE_DEVNODE_NORMAL) == CR_SUCCESS)
                            {
                                Device dev = new Device();
                                dev.Id = ptrID;
                                dev.Name = GetDevicePropertyString((uint)devinst, ref PKEY_Device_FriendlyName);

                                if (dev.Name.Length == 0)
                                    dev.Name = GetDevicePropertyString((uint)devinst, ref PKEY_Device_DeviceDesc);

                                dev.Props.Add(new KeyValuePair<string, object>("Id", dev.Id));
                                dev.Props.Add(new KeyValuePair<string, object>("Name", dev.Name));

                                AddDeviceProp(devinst, PKEY_Device_FriendlyName, DEVPROP_TYPE_STRING, "Friendly Name", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_HardwareIds, DEVPROP_TYPE_STRING_LIST, "Hardware Id", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_InstanceId, DEVPROP_TYPE_STRING, "Instance Id", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_Manufacturer, DEVPROP_TYPE_STRING, "Manufacturer", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_LocationInfo, DEVPROP_TYPE_STRING, "Location", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_DeviceDesc, DEVPROP_TYPE_STRING, "DialogText", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_CompatibleIds, DEVPROP_TYPE_STRING_LIST, "Compatible Id", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_Service, DEVPROP_TYPE_STRING, "Service", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_Class, DEVPROP_TYPE_GUID, "Class", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_Driver, DEVPROP_TYPE_STRING, "Driver", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_PDOName, DEVPROP_TYPE_STRING, "PDO Name", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_Capabilities, DEVPROP_TYPE_UINT32, "Capabilities", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_UINumber, DEVPROP_TYPE_UINT32, "UI Number", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_BusNumber, DEVPROP_TYPE_UINT32, "Bus Number", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_EnumeratorName, DEVPROP_TYPE_STRING, "Enumeration Name", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_DevType, DEVPROP_TYPE_UINT32, "Type", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_Address, DEVPROP_TYPE_UINT32, "Address", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_DevNodeStatus, DEVPROP_TYPE_UINT32, "Status", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_ProblemCode, DEVPROP_TYPE_UINT32, "Problem", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_EnumeratorName, DEVPROP_TYPE_STRING, "Enumerator", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_ClassGuid, DEVPROP_TYPE_STRING, "Class Guid", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_ModelId, DEVPROP_TYPE_GUID, "Model Id", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_ConfigurationId, DEVPROP_TYPE_STRING, "Configuration Id", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_InstallDate, DEVPROP_TYPE_FILETIME, "Install Date", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_FirstInstallDate, DEVPROP_TYPE_FILETIME, "First Install Date", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_LastArrivalDate, DEVPROP_TYPE_FILETIME, "Last Arrival Date", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_LastRemovalDate, DEVPROP_TYPE_FILETIME, "Last Removal Date", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_DriverDate, DEVPROP_TYPE_FILETIME, "Driver Date", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_DriverVersion, DEVPROP_TYPE_STRING, "Driver Version", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_DriverDesc, DEVPROP_TYPE_STRING, "Driver Desc", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_DriverInfPath, DEVPROP_TYPE_STRING, "Driver Inf Path", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_DriverInfSection, DEVPROP_TYPE_STRING, "Driver Inf Section", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_DriverInfSectionExt, DEVPROP_TYPE_STRING, "Driver Inf Section Ext", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_MatchingDeviceId, DEVPROP_TYPE_STRING, "Matching Device Id", ref dev);
                                AddDeviceProp(devinst, PKEY_Device_DriverProvider, DEVPROP_TYPE_STRING, "Driver Provider", ref dev);

                                // Get Status And Problem
                                uint status, problem;

                                CM_Get_DevNode_Status(out status, out problem, (uint)devinst, 0);
                                dev.IsEnabled = (problem == 0);
                                dev.IconPath = dev.IsEnabled ? "/res/blue.bmp" : "/res/red.bmp";

                                Devices.Add(dev);
                            }
                        }
                    }
                }

                return Devices;
            }
        }

        private static void AddDeviceProp(int devinst, DEVPROPKEY key, uint type, String name, ref Device dev)
        {
            byte[] buf = new byte[2000];
            uint bufSize = (uint) buf.Length;

            Guid guid = GetDevicePropertyGUID((uint)devinst, ref PKEY_Device_ClassGuid);

            if ( CM_Get_DevNode_Property((uint)devinst, ref key, out type, buf, ref bufSize, 0) == CR_SUCCESS ||
                CM_Get_Class_Property(ref guid, ref key, out type, buf, ref bufSize, 0) == CR_SUCCESS)
            {
                byte[] temp = new byte[bufSize];
                Array.Copy(buf, temp, bufSize);

                object value = null;
                switch( type )
                {
                    case DEVPROP_TYPE_STRING:
                        value = System.Text.Encoding.Unicode.GetString(temp, 0, (int) bufSize - 2);
                        break;
                    case DEVPROP_TYPE_STRING_LIST:
                        value = System.Text.Encoding.Unicode.GetString(temp, 0, (int)bufSize - 2).Replace('\0', ';');
                        break;
                    case DEVPROP_TYPE_GUID:
                        value = new Guid(temp);
                        break;
                    case DEVPROP_TYPE_INT32:
                        value = BitConverter.ToInt32(temp, 0);
                        break;
                    case DEVPROP_TYPE_UINT32:
                        value = (uint) BitConverter.ToInt32(temp, 0);
                        break;
                    case DEVPROP_TYPE_FILETIME:
                        long utime = BitConverter.ToInt64(temp, 0);
                        value = DateTime.FromFileTime(utime);
                        break;
                }

                if (value != null) {
                    dev.Props.Add( new KeyValuePair<string, object> (name, value) );
                }
            }
        }

        private static string GetDevicePropertyString(uint DevInst, ref DEVPROPKEY DevProKey)
        {
            uint PropertyType = DEVPROP_TYPE_STRING;
            byte[] Buf = new byte[1000];
            uint PropertySize = (uint) Buf.Length;
            uint cr = CM_Get_DevNode_Property(DevInst, ref DevProKey, out PropertyType, Buf, ref PropertySize, 0);

            return (cr == CR_SUCCESS) ? System.Text.Encoding.Unicode.GetString(Buf, 0, (int) PropertySize - 2) : "";
        }

        private static Guid GetDevicePropertyGUID(uint DevInst, ref DEVPROPKEY DevProKey)
        {
            uint PropertyType = DEVPROP_TYPE_GUID;
            byte[] Buf = new byte[16];
            uint PropertySize = (uint)Buf.Length;
            uint cr = CM_Get_DevNode_Property(DevInst, ref DevProKey, out PropertyType, Buf, ref PropertySize, 0);
            
            return new Guid(Buf);
        }

        private static string GetDevicePropertyStringList(uint DevInst, ref DEVPROPKEY DevProKey)
        {
            uint PropertyType = DEVPROP_TYPE_STRING_LIST;
            byte[] Buf = new byte[2000];
            uint PropertySize = (uint)Buf.Length;
            uint cr = CM_Get_DevNode_Property(DevInst, ref DevProKey, out PropertyType, Buf, ref PropertySize, 0);
            string str = (cr == CR_SUCCESS) ? System.Text.Encoding.Unicode.GetString(Buf, 0, (int)PropertySize - 2) : "";
            str.Replace('\0', ';');
            return str;
        }

        private static int GetDevicePropertyINT32(uint DevInst, ref DEVPROPKEY DevProKey)
        {
            uint PropertyType = DEVPROP_TYPE_INT32;
            byte[] Buf = new byte[10];
            uint PropertySize = (uint)Buf.Length;
            uint cr = CM_Get_DevNode_Property(DevInst, ref DevProKey, out PropertyType, Buf, ref PropertySize, 0);

            return BitConverter.ToInt32(Buf, 0);
        }

        public struct DEVPROPKEY
        {
            public Guid guid;
            public uint pid;

            public DEVPROPKEY(uint a, int b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k, uint pId)
            {
                this.guid = new Guid((int) a, (short) b, c, d, e, f, g, h, i, j, k);
                this.pid = pId;
            }
        }

    }
}
