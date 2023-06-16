using ValorantClient.Lib.Logging;

namespace ValorantClient.Cli.Logging
{
    /// <summary>
    /// Warning: !! Lazy solution to an easy problem :p !!
    /// 
    /// Store logs in memory
    /// </summary>
    public static class Memory
    {
        public static List<string> All { get; } = new();
        public static List<string> Info { get; } = new();
        public static List<string> Debug { get; } = new();
        public static List<string> Error { get; } = new();
        public static List<string> Warning { get; } = new();
    }

    public class MemoryLogger<T> : ILogger<T>
    {

        public void LogDebug(string message)
        {
            Memory.Debug.Add(ProcessMessage("DEBUG: " + message, "\x1b[36m"));
        }

        public void LogError(string message)
        {
            Memory.Error.Add(ProcessMessage("ERR: " + message, "\x1b[31m"));
        }

        public void LogInformation(string message)
        {
            Memory.Info.Add(ProcessMessage("INFO: " + message, "\x1b[32m"));
        }

        public void LogWarning(string message)
        {
            Memory.Warning.Add(ProcessMessage("WARN: " + message, "\x1b[33m"));
        }

        private string ProcessMessage(string message,string colorCode = "\x1b[37m")
        {
            var newMessage =  colorCode + $"[{DateTime.Now.ToString("HH:mm:ss")}] [{typeof(T).Name}] {message} " + "\x1b[0m";
            Memory.All.Add(newMessage);
            return newMessage;
        }
    }
}
