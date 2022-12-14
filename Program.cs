using zsh_ultra;

using (var terminal = new ZSHUltra())
{
    using var mainTask = new Task(terminal.StartTerminal);
    mainTask.ContinueWith(ExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
    mainTask.Start();
    mainTask.Wait();
}

static void ExceptionHandler(Task task)
{
    Console.WriteLine("Exception:");
    Tools.ColorWriteLine(task.Exception == null ? "Null Agreggate Exception" : task.Exception, ConsoleColor.DarkRed);
}