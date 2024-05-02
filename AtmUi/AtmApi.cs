using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace AtmUi {
    public class AtmApi {
        // {561452EE-5EAA-401F-ABD6-5EE6D99DE901}
        public static Guid ClientId { get; private set; } = new Guid("{561452EE-5EAA-401F-ABD6-5EE6D99DE901}");
        public static string ClientName { get; private set; } = "Just.Cash ATM";

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct VersionData {
            public ushort major;
            public ushort minor;
            public ushort revision;
            public ushort build;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct AppData {
            public Guid appID;
            public VersionData appVersionData;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        /* We need to consider versioning */
        public struct InstanceData {
            public AppData clientAppData;
            public AppData apiAddData;
            public Handle atmHandle;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string location;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        /* We need to consider versioning */
        public struct TransactionData {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string userName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
            public string trxID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string languageID;
            [MarshalAs(UnmanagedType.I8)]
            public long transactionTime;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct RegistrationData {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string userName;
        }

        internal static event MyEventDelegate? MyEvent;

        static AtmApi() {
            _instanceData = new InstanceData {
                clientAppData = new AppData {
                    appVersionData = new VersionData { major = 0, minor = 0, revision = 0 }
                }
            };
        }

        public static AtmViewModel Atm { get; set; } = new AtmViewModel();
        public static ViewModels.Transaction Transaction { get; set; } = new ViewModels.Transaction();


        public static HandleRef ApiHandle { get; private set; }

        private static InstanceData _instanceData;

        public static void OnMyEvent(IntPtr messagePtr) {
            string? value = Marshal.PtrToStringUni(messagePtr);
            Transaction.UserName = value;
        }

        public delegate void MyEventDelegate(IntPtr messagePtr);

        [DllImport("AtmApi.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr Connect(ref InstanceData instanceData);

        public static async Task<bool> Connect() {
            bool result = await Application.Current.Dispatcher.InvokeAsync(() => {
                IntPtr handle = Connect(ref _instanceData);

                if (handle == 0) {
                    return false;
                }

                ApiHandle = new HandleRef(null, handle);
                Atm.Update(_instanceData);

                MyEvent += OnMyEvent;
                RegisterCallback(ApiHandle.Handle, MyEvent);
                return true;
            }, DispatcherPriority.Background);

            return result;
        }

        [DllImport("AtmApi.dll", CharSet = CharSet.Unicode)]
        public static extern int Disconnect(IntPtr handle);

        public static void Disconnect() {
            Disconnect(ApiHandle.Handle);
        }

        public static TransactionData _transactionData = new TransactionData();

        [DllImport("AtmApi.dll", CharSet = CharSet.Unicode)]
        public static extern int RegisterCallback(IntPtr handle, MyEventDelegate callback);

        [DllImport("AtmApi.dll", CharSet = CharSet.Unicode)]
        private static extern int StartTransaction(IntPtr handle, ref TransactionData transactionData);

        public static async Task<bool> StartTransaction() {
            int result = await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                return StartTransaction(ApiHandle.Handle, ref _transactionData);
            }, DispatcherPriority.Background);

            if (result < 0) {
                return false;
                // throw new ApplicationException($"StartTransaction failed with HRESULT 0x{result.ToString("X")}");
            }

            Transaction.Update(_transactionData);
            return true;
        }

        [DllImport("AtmApi.dll", CharSet = CharSet.Unicode)]
        private static extern int EndTransaction(IntPtr handle, ref TransactionData transactionData);

        public static async Task<bool> EndTransaction() {
            int result = await Application.Current.Dispatcher.InvokeAsync(() => {
                return EndTransaction(ApiHandle.Handle, ref _transactionData);
            }, DispatcherPriority.Background);

            if (result < 0) {
                return false;
                // throw new ApplicationException($"StartTransaction failed with HRESULT 0x{result.ToString("X")}");
            }

            Transaction.Update(_transactionData);
            return true;
        }

        [DllImport("AtmApi.dll", CharSet = CharSet.Unicode)]
        private static extern int RegisterPhoneKYC(IntPtr handle, ref RegistrationData registrationData);

        public static async Task<bool> RegisterPhoneKYC(RegistrationData registrationData) {
            int result = await Application.Current.Dispatcher.InvokeAsync(() => {
                return RegisterPhoneKYC(ApiHandle.Handle, ref registrationData);
            }, DispatcherPriority.Background);

            if (result < 0) {
                return false;
                // throw new ApplicationException($"RegisterPhoneKYC failed with HRESULT 0x{result.ToString("X")}");
            }

            // Registration.Update(registrationData);
            return true;
        }
    }
}
