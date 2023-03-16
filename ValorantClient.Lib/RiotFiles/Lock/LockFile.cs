namespace ValorantClient.Lib.RiotFiles.Lock
{
    public class LockFile
    {
        public string Name { get; init; } = string.Empty;
        public uint PID { get; init; }
        public uint Port { get; init; }
        public string Password { get; init; } = string.Empty;
        public string Protocol { get; init; } = string.Empty;

    }
}
