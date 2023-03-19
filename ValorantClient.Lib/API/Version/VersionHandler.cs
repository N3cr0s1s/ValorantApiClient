using MediatR;
using RestSharp;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Logging;
using ValorantClient.Lib.Network;

namespace ValorantClient.Lib.API.Version
{
    public class VersionHandler : IRequestHandler<VersionQuery, string>
    {
        private readonly ILogger<VersionHandler> _logger;
        private readonly IHttpClientService _httpClientService;
        private readonly ICache _cache;

        public VersionHandler(
            ILogger<VersionHandler> logger,
            IHttpClientService httpClientService,
            ICache cache
            )
        {
            _logger = logger;
            _httpClientService = httpClientService;
            _cache = cache;
        }

        public async Task<string> Handle(VersionQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Requesting version");

            if (_cache.TryGetValue(CacheValues.ShippingVersion.ToString(),out string cachedVersion))
            {
                return cachedVersion;
            }

            var client = await _httpClientService.CreateRestClientAsync("https://valorant-api.com/v1/version");
            VersionResponse response = await client.GetAsync<VersionResponse>(new RestRequest());
            _logger.LogDebug("Request sent");
            string version = $"{response.Data.Branch}-shipping-{response.Data.BuildVersion}-{response.Data.Version.Split('.')[3]}";
            _logger.LogDebug("Formatted version: " + version);
            return version;
        }
    }
}
