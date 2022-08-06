using System.Diagnostics;
using System.Text;

namespace zsh_ultra
{
	internal class ZSHUltra : IDisposable
	{
		public static object? Singleton { get; private set; }
		private readonly ProcessStartInfo? terminalInfo;
		//Thread safety StringBuilder
		private readonly object builderLock = new();
		private readonly StringBuilder outputBuilder = new();
		private readonly StringBuilder errorBuilder = new();
		private int inputLineCount, oldInputLineCount = -1;
		private volatile bool hasInput = true;
		private volatile bool outputDone = false;
		//Thread safety IDisposable
		private volatile bool disposedValue;

		private ZSHUltra() => Dispose();

		public ZSHUltra(ProcessStartInfo terminalInfo)
		{
			if (Singleton == null)
			{
				Singleton = this;
				this.terminalInfo = terminalInfo;
			}
			else
				Dispose();
		}
		public void StartTerminal()
		{
			if (disposedValue) throw new ObjectDisposedException(ToString() + " " + terminalInfo?.FileName);
            Console.CancelKeyPress += CancelKeyPressHandler;
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
							Tools.ColorWrite($"{tOutput}", ConsoleColor.DarkBlue);
						else if (tError != string.Empty)
							Tools.ColorWrite($"{tError}", ConsoleColor.DarkRed);
						hasInput = false;
						outputDone = false;
						outputBuilder.Clear();
						errorBuilder.Clear();
					}
					else if (inputLineCount != 0 && hasInput) continue;
				}

				if (oldInputLineCount == inputLineCount +-1)
				{
					oldInputLineCount = inputLineCount;
					Tools.ColorWrite($"[{inputLineCount}]-[{Environment.UserName}@{Environment.UserDomainName}]-$ ",
							ConsoleColor.DarkCyan);
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

		private Process InitializeChildProcess()
		{
			var terminal = new Process { StartInfo = terminalInfo ?? throw new NullReferenceException() { Source = nameof(terminalInfo) } };
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

		private static void InputTechniqueToStdin(Process wslProcess, string input)
		{
			switch (input)
			{
				case "clear":
					wslProcess.StandardInput.WriteLine($"echo $({input}); echo /ec/");
					break;
				default:
					wslProcess.StandardInput.WriteLine(input.ToLower().Contains("cd") ? $"{input}; pwd; echo /ec/" : $"{input}; echo /ec/");
					break;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposedValue) return;
			if (disposing)
				if (Singleton == this)
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
