namespace ValorantClient.Lib.Caching
{
    public class Cache : ICache
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        public Task<T> GetValueAsync<T>(string key)
        {
            if (_cache.TryGetValue(key,out object value))
                return Task.FromResult((T)value);

            return null;
        }

        public Task<T> GetValueAsync<T>(CacheValues key)
            => GetValueAsync<T>(key.ToString());

        public async Task<bool> IsCached(string key)
        {
            return _cache.ContainsKey(key);
        }

        public Task<T> SetValueAsync<T>(string key, T value)
        {
            _cache[key] = value;
            return Task.FromResult(value);
        }

        public Task<T> SetValueAsync<T>(CacheValues key, T value)
            => SetValueAsync<T>(key.ToString(), value);

        public bool TryGetValue<T>(string key, out T value)
        {
            if (_cache.ContainsKey(key))
            {
                value = (T)_cache[key];
                return true;
            }
            value = default;
            return false;
        }

        public bool TryGetValue<T>(CacheValues key, out T value)
            => TryGetValue<T>(key.ToString(),out value);
    }
}
