using System.Diagnostics;
using System.Text;

namespace zsh_ultra
{
    internal class ZSHUltra : IDisposable
    {
        //Thread safe instance
        public static object? Singleton { get; private set; }
        //Terminal Info
        private readonly TerminalInfo terminalInfo;
        //Standard Streams redirection string builders
        private readonly StringBuilder outputBuilder = new();
        private readonly StringBuilder errorBuilder = new();
        //Thread safety StringBuilders
        private readonly object builderLock = new();
        //Thread safety IDisposable
        private volatile bool hasInput = true;
        private volatile bool outputDone = false;
        private volatile bool disposedValue;
        //Input lines
        private int inputLineCount, oldInputLineCount = -1;

        public ZSHUltra()
        {
            if (Singleton == null)
            {
                Singleton = this;
                terminalInfo = TerminalInfo.GetTerminalInfo();
            }
            else
                Dispose();
        }
        public void StartTerminal()
        {
            if (disposedValue) throw new ObjectDisposedException($"{this} {terminalInfo.processInfo?.FileName}");
            Console.CancelKeyPress += CancelKeyPressHandler;
            try
            {
                using var terminal = InitializeChildProcess();
                Tools.ColorWriteLine("Welcome to ZSH ULTRA powered by .NET", ConsoleColor.DarkMagenta);
                do
                {
                    lock (builderLock)
                    {
                        var tOutput = outputBuilder.ToString();
                        var tError = errorBuilder.ToString();

                        if ((tOutput != string.Empty || tError != string.Empty) && hasInput && outputDone)
                        {
                            if (tOutput != string.Empty)
                                Tools.ColorWrite($"{tOutput}", ConsoleColor.Gray);
                            else if (tError != string.Empty)
                                Tools.ColorWrite($"{tError}", ConsoleColor.DarkRed);
                            hasInput = false;
                            outputDone = false;
                            outputBuilder.Clear();
                            errorBuilder.Clear();
                        }
                        else if (inputLineCount != 0 && hasInput) continue;
                    }

                    if (oldInputLineCount == inputLineCount +- 1)
                    {
                        oldInputLineCount = inputLineCount;
                        InputHeader();
                    }
                    var input = Console.ReadLine();
                    if (string.IsNullOrEmpty(input)) continue;
                    hasInput = true;
                    InputTechniqueToStdin(terminal, input);
                    terminal.StandardInput.Flush();
                    inputLineCount++;
                }
                while (!terminal.HasExited);
                terminal.WaitForExit();
                terminal.Close();
            }
            catch (ApplicationException)
            {
                throw new ApplicationException("Cascading to main call...") { Source = nameof(terminalInfo.processInfo) };
            }
        }

        private Process InitializeChildProcess()
        {
            var terminal = new Process { StartInfo = terminalInfo.processInfo ?? 
                                         throw new ApplicationException("Null process info for the terminal. Starting exception cascade...")
                                       { Source = nameof(terminalInfo.processInfo) } };
            terminal.OutputDataReceived += OutputHandler;
            terminal.ErrorDataReceived += ErrorHandler;
            terminal.Start();
            terminal.StandardInput.NewLine = "\n";
            terminal.BeginOutputReadLine();
            terminal.BeginErrorReadLine();
            return terminal;
        }

        private void CancelKeyPressHandler(object? sendingProcess, ConsoleCancelEventArgs args)
        {
            outputDone = hasInput;
            args.Cancel = true;
        }

        private void OutputHandler(object sendingProcess, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            if (e.Data == "/ec/")
            {
                outputDone = true;
                return;
            }
            lock (builderLock)
                outputBuilder?.Append(e.Data + Environment.NewLine);
        }

        private void ErrorHandler(object sendingProcess, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            outputDone = true;
            lock (builderLock)
                errorBuilder?.Append(e.Data.Replace("zsh", "zsh-ultra") + Environment.NewLine);
        }

        protected virtual void InputHeader()
        {
            Tools.ColorWrite("[", ConsoleColor.DarkCyan);
            Tools.ColorWrite($"{inputLineCount}", ConsoleColor.Gray);
            Tools.ColorWrite("]", ConsoleColor.DarkCyan);
            Tools.ColorWrite("-", ConsoleColor.Gray);
            Tools.ColorWrite("[", ConsoleColor.DarkCyan);
            Tools.ColorWrite($"{}", ConsoleColor.Gray);
            Tools.ColorWrite("]", ConsoleColor.DarkCyan);
            Tools.ColorWrite("-", ConsoleColor.Gray);
            Tools.ColorWrite("[", ConsoleColor.DarkCyan);
            Tools.ColorWrite($"{Environment.UserName}@{Environment.UserDomainName}", ConsoleColor.Gray);
            Tools.ColorWrite("]", ConsoleColor.DarkCyan);
            Tools.ColorWrite("-", ConsoleColor.Gray);
            Tools.ColorWrite("$", ConsoleColor.DarkYellow);
        }

        private static void InputTechniqueToStdin(Process terminalProcess, string input)
        {
            switch (input)
            {
                case "clear":
                    terminalProcess.StandardInput.WriteLine($"echo $({input}); echo /ec/");
                    break;
                default:
                    terminalProcess.StandardInput.WriteLine(input.ToLower().Contains("cd") ? $"{input}; pwd; echo /ec/" : $"{input}; echo /ec/");
                    break;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing && ReferenceEquals(Singleton, this))
                Singleton = null;
            disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
