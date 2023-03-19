namespace ValorantClient.Lib.Config
{
    public interface IConfiguration
    {
        /// <summary>
        /// Read key value from config with indexer.
        /// </summary>
        /// <param name="path">Example: "route:to:path:key"</param>
        /// <returns>Value</returns>
        public Task<string> this[string path]{ get; }

        public Task LoadConfigFileAsync(string path);

        public Task<T> ParseAsync<T>(string path);

        public T Parse<T>(string path);
    }
}
