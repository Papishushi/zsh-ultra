using System.Diagnostics;
using System.Text;
using zsh_ultra;

var platform = Environment.OSVersion.Platform;
var wslInfo = new ProcessStartInfo
{
    CreateNoWindow = true,
    RedirectStandardInput = true,
    RedirectStandardError = true,
    RedirectStandardOutput = true,
    StandardErrorEncoding = Encoding.UTF8,
    StandardOutputEncoding = Encoding.UTF8,
    StandardInputEncoding = Encoding.UTF8,
    UseShellExecute = false,
    FileName = platform == PlatformID.Win32NT ? @"wsl.exe" : @"~/home/*/.zshrc", //TO-DO Check if it really works.
    WorkingDirectory = platform == PlatformID.Win32NT ? @"\\wsl.localhost\Ubuntu\home\" : @"~/home/*/" //TO-DO Check if it really works.
};

try
{
    using var terminal = new ZSHUltra(wslInfo);
    terminal.StartTerminal();
}
catch (InvalidOperationException e)
{
    Console.WriteLine("Exception:");
    Console.WriteLine(e);
}