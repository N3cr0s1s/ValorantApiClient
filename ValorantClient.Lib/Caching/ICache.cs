namespace ValorantClient.Lib.Caching
{
    public interface ICache
    {

        Task<bool> IsCached(string key);

        Task<T> SetValueAsync<T>(string key, T value);

        Task<T> SetValueAsync<T>(CacheValues key, T value);

        Task<T> GetValueAsync<T>(string key);

        Task<T> GetValueAsync<T>(CacheValues key);

        bool TryGetValue<T>(string key, out T value);

        bool TryGetValue<T>(CacheValues key, out T value);
    }
}
