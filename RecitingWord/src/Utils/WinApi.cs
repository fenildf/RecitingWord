using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace NORMAN.NRM411S7
{
    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class NativeMethods
    {
        [DllImport("advapi32.dll",CharSet = CharSet.Unicode)]
        internal static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);
        [DllImport("Advapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName,
        int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpPathName,
        string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);
        [DllImport("advapi32.dll")]
        internal static extern uint CloseServiceHandle(IntPtr SCHANDLE);
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern int StartService(IntPtr SVHANDLE, int dwNumServiceArgs, string lpServiceArgVectors);
        [DllImport("advapi32.dll", SetLastError = true,CharSet = CharSet.Unicode)]
        internal static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);
        [DllImport("advapi32.dll")]
        internal static extern int DeleteService(IntPtr SVHANDLE);
        [DllImport("kernel32.dll")]
        internal static extern int GetLastError();


        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();


        #region Driver
        public static int SERVICE_AUTO_START = 0x00000002;
        public static int SC_MANAGER_CREATE_SERVICE = 0x0002;
        public static int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
        public static int SERVICE_KERNEL_DRIVER = 0x00000001;
        public static int SERVICE_DEMAND_START = 0x00000003;   
        public static int SERVICE_ERROR_NORMAL = 0x00000001;
        public static int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        public static int SERVICE_QUERY_CONFIG = 0x0001;
        public static int SERVICE_CHANGE_CONFIG = 0x0002;
        public static int SERVICE_QUERY_STATUS = 0x0004;
        public static int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
        public static int SERVICE_START = 0x0010;
        public static int SERVICE_STOP = 0x0020;
        public static int SERVICE_PAUSE_CONTINUE = 0x0040;
        public static int SERVICE_INTERROGATE = 0x0080;
        public static int SERVICE_USER_DEFINED_CONTROL = 0x0100;
        public static int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
        SERVICE_QUERY_CONFIG |
        SERVICE_CHANGE_CONFIG |
        SERVICE_QUERY_STATUS |
        SERVICE_ENUMERATE_DEPENDENTS |
        SERVICE_START |
        SERVICE_STOP |
        SERVICE_PAUSE_CONTINUE |
        SERVICE_INTERROGATE |
        SERVICE_USER_DEFINED_CONTROL); 
        #endregion
    }
}
