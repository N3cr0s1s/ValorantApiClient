using MediatR;
using RestSharp;
using ValorantClient.Lib.API.Auth.LocalHeaders;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Config;
using ValorantClient.Lib.Logging;
using ValorantClient.Lib.Network;
using ValorantClient.Lib.RiotFiles.Lock;

namespace ValorantClient.Lib.API.Auth.Entitlements
{
    public class EntitlementHandler : IRequestHandler<EntitlementQuery, Entitlement>
    {
        private readonly ILogger<EntitlementHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientService _httpClientService;
        private readonly ILockFileService _lockFileService;
        private readonly ICache _cache;
        private readonly Model.Config _config;

        public EntitlementHandler(
            ILogger<EntitlementHandler> logger,
            IMediator mediator,
            IConfiguration configuration,
            IHttpClientService httpClientService,
            ILockFileService lockFileService,
            ICache cache
            )
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
            _httpClientService = httpClientService;
            _lockFileService = lockFileService;
            _cache = cache;
            _config = configuration.Parse<Model.Config>("config");
        }

        public async Task<Entitlement> Handle(EntitlementQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Get Entitlements");

            if (_cache.TryGetValue(CacheValues.Entitlement.ToString(), out Entitlement cacheEntitlement))
                return cacheEntitlement;

            ICollection<KeyValuePair<string, string>> headerParameters = await _mediator.Send(new LocalHeadersQuery());
            LockFile lockFile = await _lockFileService.LoadLockFileAsync(await _configuration["lockFile:path"]);
            string entitlementsUrl = await _configuration["endpoints:entitlements"];
            entitlementsUrl = entitlementsUrl
                .Replace("{baseEndpointLocal}", _config.BaseEndpointLocal)
                .Replace("{port}", lockFile.Port.ToString());

            _logger.LogDebug("Entitlements url: " + entitlementsUrl);

            var client = await _httpClientService.CreateRestClientAsync(entitlementsUrl, true);

            var restRequest = new RestRequest();
            restRequest.AddHeaders(headerParameters);

            _logger.LogDebug("Sending entitlements request");
            Entitlement entitlements = await client.GetAsync<Entitlement>(restRequest);
            _logger.LogDebug("Entitlements request sent");

            await _cache.SetValueAsync(CacheValues.Entitlement.ToString(), entitlements);

            return entitlements;
        }
    }
}
