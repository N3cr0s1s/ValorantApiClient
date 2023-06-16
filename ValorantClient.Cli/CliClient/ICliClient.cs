namespace ValorantClient.Cli.CliClient
{
    public interface ICliClient
    {
        /// <summary>
        /// <see cref="ICliClient"/> version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Debug mode
        /// </summary>
        bool Debug { get; set; }

        /// <summary>
        /// Start client async
        /// </summary>
        Task StartAsync();
    }
}
