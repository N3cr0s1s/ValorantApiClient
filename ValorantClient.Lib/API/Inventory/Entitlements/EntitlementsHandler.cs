using MediatR;
using ValorantClient.Lib.API.Auth.PUUID;
using ValorantClient.Lib.API.Network.Fetch;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.Inventory.Entitlements
{
    public class EntitlementsHandler : IRequestHandler<EntitlementsQuery, EntitlementsResponse>
    {
        private readonly ILogger<EntitlementsHandler> _logger;
        private readonly IMediator _mediator;
        private readonly ICache _cache;

        public EntitlementsHandler(
            ILogger<EntitlementsHandler> logger,
            IMediator mediator,
            ICache cache
            )
        {
            _logger = logger;
            _mediator = mediator;
            _cache = cache;
        }


        public async Task<EntitlementsResponse> Handle(EntitlementsQuery request, CancellationToken cancellationToken)
        {
            string puuid = await _mediator.Send(new PUUIDQuery());
            var resp = await _mediator.Send(new FetchCommand
            {
                Endpoint = $"/store/v1/entitlements/{puuid}/{request.Type}"
            });
            _logger.LogInformation(resp.Content);
            return null;
        }
    }
}
