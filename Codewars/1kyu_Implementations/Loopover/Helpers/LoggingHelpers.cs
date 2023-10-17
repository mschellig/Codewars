namespace _1kyu_Implementations.Loopover
{
    public static class LoggingHelpers
    {
        public static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:dd.MM.yyyy - HH:mm:ss}: {message}");
        }

        public static void LogHeadline(string message)
        {
            Console.WriteLine("#######################################");
            Console.WriteLine(message);
            Console.WriteLine("#######################################");
        }

        public static void LogBlank(string message)
        {
            Console.WriteLine(message);
        }
    }
}
