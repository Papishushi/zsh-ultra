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
    }
}
