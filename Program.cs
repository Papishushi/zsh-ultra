using System.Diagnostics;
using System.Text;
using zsh_ultra;

ProcessStartInfo terminalInfo;

var platform = Environment.OSVersion.Platform;
Tools.ColorWriteLine("Detecting Platform", ConsoleColor.DarkMagenta);
switch (platform)
{
    case PlatformID.Unix:
        Tools.ColorWriteLine("  Unix detected", ConsoleColor.DarkMagenta);
        terminalInfo = new ProcessStartInfo
        {
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            StandardErrorEncoding = Encoding.UTF8,
            StandardOutputEncoding = Encoding.UTF8,
            StandardInputEncoding = Encoding.UTF8,
            UseShellExecute = false,
            FileName = @$"/home/{Environment.UserName}/.zshrc",
            WorkingDirectory = @$"/home/{Environment.UserName}/"
        };
        break;
    case PlatformID.MacOSX:
        Tools.ColorWriteLine("  MacOSX detected", ConsoleColor.DarkMagenta);
        terminalInfo = new ProcessStartInfo
        {
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            StandardErrorEncoding = Encoding.UTF8,
            StandardOutputEncoding = Encoding.UTF8,
            StandardInputEncoding = Encoding.UTF8,
            UseShellExecute = false,
            FileName = @$"~/.zshrc",
            WorkingDirectory = @$"~/"
        };
        break;
    case PlatformID.Win32NT:
        Tools.ColorWriteLine("  Windows32NT detected", ConsoleColor.DarkMagenta);
        Tools.RunCMDCommand("wsl --list -q");
        terminalInfo = new ProcessStartInfo
        {
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            StandardErrorEncoding = Encoding.UTF8,
            StandardOutputEncoding = Encoding.UTF8,
            StandardInputEncoding = Encoding.UTF8,
            UseShellExecute = false,
            FileName = @"wsl.exe",
            WorkingDirectory = @$"\\wsl.localhost\Ubuntu\home\{Tools.RunCMDCommand("wsl --exec whoami")}\"
        };
        break;
    default:
        Tools.ColorWriteLine("  Unknown platform detected", ConsoleColor.DarkMagenta);
        terminalInfo = new ProcessStartInfo
        {
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            StandardErrorEncoding = Encoding.UTF8,
            StandardOutputEncoding = Encoding.UTF8,
            StandardInputEncoding = Encoding.UTF8,
            UseShellExecute = false,
            FileName = @$"/home/{Environment.UserName}/.zshrc",
            WorkingDirectory = @$"/home/{Environment.UserName}/"
        };
        break;
}

try
{
    using (var terminal = new ZSHUltra(terminalInfo))
    {
        var mainThread = new Thread(terminal.StartTerminal);
        mainThread.Start();
        mainThread.Join();
    }
}
catch (InvalidOperationException e)
{
    Console.WriteLine("Exception:");
    Console.WriteLine(e);
}