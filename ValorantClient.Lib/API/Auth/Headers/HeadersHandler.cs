using MediatR;
using RestSharp;
using ValorantClient.Lib.API.Auth.Entitlements;
using ValorantClient.Lib.API.Version;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Config;
using ValorantClient.Lib.Logging;
using ValorantClient.Lib.Model;

namespace ValorantClient.Lib.API.Auth.Headers
{
    public class HeadersHandler : IRequestHandler<HeadersQuery, ICollection<KeyValuePair<string, string>>>
    {
        private readonly ILogger<HeadersHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly ICache _cache;
        private readonly Model.Config _config;

        public HeadersHandler(
            ILogger<HeadersHandler> logger, 
            IMediator mediator,
            IConfiguration configuration,
            ICache cache)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
            _cache = cache;
            _config = configuration.Parse<Model.Config>("config");
        }

        public async Task<ICollection<KeyValuePair<string, string>>> Handle(
            HeadersQuery request, 
            CancellationToken cancellationToken)
        {
            _logger.LogDebug("Get main headers");

            if (_cache.TryGetValue(CacheValues.MainHeaders.ToString(),out List<KeyValuePair<string, string>> cachedHeaders))
            {
                return cachedHeaders;
            }

            Entitlement entitlement = await _mediator.Send(new EntitlementQuery());

            if (entitlement == null)
            {
                _logger.LogError("Entitlement is null!");
                throw new NullReferenceException("Entitlement is null!");
            }

            string version = await _mediator.Send(new VersionQuery());

            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>("Authorization",$"Bearer {entitlement.AccessToken}"),
                new KeyValuePair<string,string>("X-Riot-Entitlements-JWT",$"{entitlement.Token}"),
                new KeyValuePair<string,string>("X-Riot-ClientPlatform",_config.ClientPlatform),
                new KeyValuePair<string, string>("X-Riot-ClientVersion",version)
            };

            await _cache.SetValueAsync(CacheValues.MainHeaders.ToString(), headers);

            return headers;
        }
    }
}
