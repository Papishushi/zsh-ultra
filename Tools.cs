using System.Diagnostics;
using System.Text;

namespace zsh_ultra
{
    internal static class Tools
    {
        public static void ColorWriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void ColorWrite(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static string AskForInput(string question)
        {
            try
            {
                ColorWriteLine(question, ConsoleColor.DarkGreen);
                var temp = Console.ReadLine();
                return temp != null ? temp.Trim('"') : throw new ArgumentNullException(temp);
            }
            catch (Exception ex)
            {
                ColorWriteLine(ex.Message, ConsoleColor.DarkRed);
                return string.Empty;
            }
        }

        public static string? RunCMDCommand(string command)
        {
            var psInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
                UseShellExecute = false,
                FileName = @"powershell.exe",
                Arguments = command
            };

            using var psProcess = new Process() { StartInfo = psInfo };
            psProcess.Start();
            var output = psProcess.StandardOutput.ReadToEnd();
            if (output == null)
            {
                var error = psProcess.StandardError.ReadToEnd();
                ColorWrite(error, ConsoleColor.DarkRed);
                psProcess.WaitForExit();
                psProcess.Close();
                return null;
            }
            psProcess.WaitForExit();
            psProcess.Close();
            return output.ReplaceLineEndings(string.Empty);
        }
    }
}
