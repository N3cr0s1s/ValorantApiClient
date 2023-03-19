namespace ValorantClient.Lib.Logging
{
    public interface ILogger<T>
    {
        void LogInformation(string message);

        void LogWarning(string message);

        void LogError(string message);

        void LogDebug(string message);

    }
}
