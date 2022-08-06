using zsh_ultra;

using (var terminal = new ZSHUltra())
{
    var mainTask = new Task(terminal.StartTerminal);
    mainTask.ContinueWith(ExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
    mainTask.Start();
    mainTask.Wait();
}

static void ExceptionHandler(Task task)
{
    Console.WriteLine("Exception:");
    Tools.ColorWriteLine(task.Exception, ConsoleColor.DarkRed);
}