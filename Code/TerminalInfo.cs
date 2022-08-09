using System.Diagnostics;
using System.Text;
using zsh_ultra;

struct TerminalInfo
{
    public ProcessStartInfo? processInfo;
    public readonly PlatformID platform;
    public string name;

    public TerminalInfo()
    {
        processInfo = null;
        platform = Environment.OSVersion.Platform;
        name = "ZSH-Ultra";
    }

    private TerminalInfo(string terminalName)
    {
        processInfo = null;
        platform = Environment.OSVersion.Platform;
        name = terminalName;
    }

    private TerminalInfo(ProcessStartInfo? processInfo)
    {
        this.processInfo = processInfo;
        platform = Environment.OSVersion.Platform;
        name = "ZSH-Ultra";
    }

    private TerminalInfo(ProcessStartInfo? processInfo, string terminalName)
    {
        this.processInfo = processInfo;
        platform = Environment.OSVersion.Platform;
        name = terminalName;
    }

    public static TerminalInfo GetTerminalInfo(params string[] terminalName)
    {
        var temp = new TerminalInfo(null ,terminalName.Length > 0 ? terminalName.First() : "ZSH-Ultra");
        Tools.ColorWriteLine("Detecting Platform", ConsoleColor.DarkMagenta);
        switch (temp.platform)
        {
            case PlatformID.Unix:
                Tools.ColorWriteLine("  Unix detected", ConsoleColor.DarkMagenta);
                temp.processInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    StandardErrorEncoding = Encoding.UTF8,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardInputEncoding = Encoding.UTF8,
                    UseShellExecute = false,
                    FileName = @$"zsh",
                    WorkingDirectory = @$"/home/{Environment.UserName}/"
                };
                return temp;
            case PlatformID.MacOSX:
                Tools.ColorWriteLine("  MacOSX detected", ConsoleColor.DarkMagenta);
                temp.processInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    StandardErrorEncoding = Encoding.UTF8,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardInputEncoding = Encoding.UTF8,
                    UseShellExecute = false,
                    FileName = @$"zsh",
                    WorkingDirectory = @$"~/"
                };
                return temp;
            case PlatformID.Win32NT:
                Tools.ColorWriteLine("  Windows32NT detected", ConsoleColor.DarkMagenta);
                Tools.RunCMDCommand("wsl --list -q");
                temp.processInfo = new ProcessStartInfo
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
                return temp;
            default:
                Tools.ColorWriteLine("  Unknown platform detected", ConsoleColor.DarkMagenta);
                temp.processInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    StandardErrorEncoding = Encoding.UTF8,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardInputEncoding = Encoding.UTF8,
                    UseShellExecute = false,
                    FileName = @$"/home/{Environment.UserName}/.zsh",
                    WorkingDirectory = @$"/home/{Environment.UserName}/"
                };
                return temp;
        }
    }
}
