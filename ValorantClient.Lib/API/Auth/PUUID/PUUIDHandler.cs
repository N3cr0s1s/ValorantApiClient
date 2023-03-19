using MediatR;
using ValorantClient.Lib.API.Auth.Entitlements;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.Auth.PUUID
{
    public class PUUIDHandler : IRequestHandler<PUUIDQuery, string>
    {
        private readonly ILogger<PUUIDHandler> _logger;
        private readonly ICache _cache;
        private readonly IMediator _mediator;

        public PUUIDHandler(
            ILogger<PUUIDHandler> logger,
            ICache cache,
            IMediator mediator
            )
        {
            _logger = logger;
            _cache = cache;
            _mediator = mediator;
        }

        public async Task<string> Handle(PUUIDQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Get PUUID");
            if (_cache.TryGetValue(CacheValues.PUUID,out string cachedPUUID))
            {
                _logger.LogDebug("Cached puuid: " + cachedPUUID);
                return cachedPUUID;
            }

            Entitlement entitlement = await _mediator.Send(new EntitlementQuery());
            string puuid = entitlement.Subject;

            await _cache.SetValueAsync(CacheValues.PUUID, puuid);

            _logger.LogDebug("Puuid: " + puuid);
            return puuid;
        }
    }
}
