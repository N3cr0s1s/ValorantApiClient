using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using ValorantClient.Lib.Exceptions;

namespace ValorantClient.Lib.Config
{
    public class JsonConfiguration : IConfiguration
    {
        private JObject? _config;
        private readonly Dictionary<string,object> _cache = new Dictionary<string,object>();

        public Task<string> this[string path]
        {
            get
            {
                if (_config is null)
                    throw new NullReferenceException($"Config is null, please load the config file with {LoadConfigFileAsync}");

                if (path.Contains(":"))
                {
                    string[] oldKeys = path.Split(':');
                    string[] keys = new string[oldKeys.Length-1];
                    for (int i = 1; i < oldKeys.Length; i++)
                    {
                        keys[i - 1] = oldKeys[i];
                    }
                    JToken? token = _config.SelectToken(oldKeys[0]);

                    if (token is null)
                        throw new KeyNotFoundException($"Key not found with this value {path}");

                    foreach (string key in keys)
                    {
                        token = token.SelectToken(key);

                        if (token is null)
                            throw new KeyNotFoundException($"Key not found with this value {path}");

                    }
                    return Task.FromResult(token.ToString());
                }

                JToken? propertyToken = _config.SelectToken(path);

                if (propertyToken is null)
                    throw new KeyNotFoundException($"Key not found with this value {path}");

                return Task.FromResult(propertyToken.ToString());
            }
        }

        /// <summary>
        /// Load a json file to <see cref="_config"/>
        /// </summary>
        /// <param name="path">Path to json file</param>
        /// <exception cref="ConfigurationException">Config already loaded!</exception>
        /// <exception cref="ConfigurationException">File not exist with this path:</exception>
        public async Task LoadConfigFileAsync(string path)
        {
            if (_config != null)
                throw new ConfigurationException($"Config already loaded!");

            if (!File.Exists(path))
                throw new ConfigurationException($"File not exist with this path: {path}");

            string content;
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream,Encoding.Default))
            {
                content = await reader.ReadToEndAsync();
            }

            _config = JObject.Parse(content);
        }

        public async Task<T> ParseAsync<T>(string path)
        {
            if (_cache.ContainsKey(path))
                return (T)_cache[path];

            if (_config is null)
                throw new NullReferenceException($"Config is null, please load the config file with {LoadConfigFileAsync}");

            T? value = JsonConvert.DeserializeObject<T>(await this[path]);

            if (value is null)
                throw new NullReferenceException($"Deserialized object is null.");

            _cache[path] = value;

            return value;
        }

        public T Parse<T>(string path)
        {
            if (_cache.ContainsKey(path))
                return (T)_cache[path];

            if (_config is null)
                throw new NullReferenceException($"Config is null, please load the config file with {LoadConfigFileAsync}");

            T? value = JsonConvert.DeserializeObject<T>(this[path].Result);

            if (value is null)
                throw new NullReferenceException($"Deserialized object is null.");

            _cache[path] = value;

            return value;
        }
    }
}
