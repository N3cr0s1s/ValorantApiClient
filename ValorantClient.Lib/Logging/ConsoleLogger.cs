namespace ValorantClient.Lib.Logging
{
    public class ConsoleLogger<T> : ILogger<T>
    {
        private readonly bool _debug;

        public ConsoleLogger(LoggerOptions options)
        {
            _debug = options.Debug;
        }

        public void LogInformation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLogMessage($"INFO: {message}");
            Console.ResetColor();
        }

        public void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteLogMessage($"WARNING: {message}");
            Console.ResetColor();
        }

        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLogMessage($"ERROR: {message}");
            Console.ResetColor();
        }

        public void LogDebug(string message)
        {
            if (_debug)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                WriteLogMessage($"DEBUG: {message}");
                Console.ResetColor();
            }
        }

        private void WriteLogMessage(string message)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] [{typeof(T).Name}] {message}");
        }
    }
}
