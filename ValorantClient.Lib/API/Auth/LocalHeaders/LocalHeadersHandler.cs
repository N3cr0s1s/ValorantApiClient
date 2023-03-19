using MediatR;
using RestSharp;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Config;
using ValorantClient.Lib.Helper;
using ValorantClient.Lib.Logging;
using ValorantClient.Lib.RiotFiles.Lock;

namespace ValorantClient.Lib.API.Auth.LocalHeaders
{
    internal class LocalHeadersHandler : IRequestHandler<LocalHeadersQuery, ICollection<KeyValuePair<string, string>>>
    {
        private readonly ILogger<LocalHeadersHandler> _logger;
        private readonly ILockFileService _lockFileService;
        private readonly IConfiguration _configuration;
        private readonly ICache _cache;

        public LocalHeadersHandler(
            ILogger<LocalHeadersHandler> logger,
            ILockFileService lockFileService,
            IConfiguration configuration,
            ICache cache
            )
        {
            _logger = logger;
            _lockFileService = lockFileService;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<ICollection<KeyValuePair<string, string>>> Handle(
            LocalHeadersQuery request, 
            CancellationToken cancellationToken)
        {
            _logger.LogDebug("Load local headers");

            if (_cache.TryGetValue(CacheValues.LocalHeaders.ToString(), out List<KeyValuePair<string, string>> cachedHeaders))
            {
                return cachedHeaders;
            }

            LockFile lockFile = await _lockFileService.LoadLockFileAsync(await _configuration["lockFile:path"]);
            string authorization = $"Basic {( "riot:" + lockFile.Password ).Base64Encode()}";
            _logger.LogDebug("Authorization header: " + authorization);
            List<KeyValuePair<string,string>> localHeaders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair < string, string >("Authorization",authorization)
            };

            await _cache.SetValueAsync(CacheValues.LocalHeaders.ToString(), localHeaders);

            return localHeaders;
        }
    }
}
