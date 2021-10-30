using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CoWork_Server.Monitor
{
    public class Memory
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GlobalMemoryStatusEx(ref MEMORY_INFO mi);

        [StructLayout(LayoutKind.Sequential)]
        struct MEMORY_INFO
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        MEMORY_INFO GetMemoryStatus()
        {
            var mi = new MEMORY_INFO();
            mi.dwLength = (uint)Marshal.SizeOf(mi);
            GlobalMemoryStatusEx(ref mi);
            return mi;
        }

        ulong GetAvailPhys()
        {
            var mi = GetMemoryStatus();
            return mi.ullAvailPhys;
        }

        ulong GetUsedPhys()
        {
            var mi = GetMemoryStatus();
            return (mi.ullTotalPhys - mi.ullAvailPhys);
        }

        ulong GetTotalPhys()
        {
            var mi = GetMemoryStatus();
            return mi.ullTotalPhys;
        }

        public string GetMemoryUsage()
        {
            return ((float)GetUsedPhys() / GetTotalPhys()).ToString("P2");
        }
    }
}