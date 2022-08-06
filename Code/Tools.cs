using System.Diagnostics;
using System.Text;

namespace zsh_ultra
{
    internal static class Tools
    {
        public static void ColorWriteLine(object text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void ColorWrite(object text, ConsoleColor color)
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
            var cmdInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
                UseShellExecute = false,
                FileName = @"CMD.exe",
                Arguments = "/C " + command
            };

            using var cmdProcess = new Process() { StartInfo = cmdInfo };
            cmdProcess.Start();
            var output = cmdProcess.StandardOutput.ReadToEnd();
            if (output == null)
            {
                var error = cmdProcess.StandardError.ReadToEnd();
                ColorWrite(error, ConsoleColor.DarkRed);
                cmdProcess.WaitForExit();
                cmdProcess.Close();
                return null;
            }
            cmdProcess.WaitForExit();
            cmdProcess.Close();
            return output.ReplaceLineEndings(string.Empty);
        }
    }
}
