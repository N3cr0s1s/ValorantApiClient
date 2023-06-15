using MediatR;
using Newtonsoft.Json;
using ValorantClient.Lib.API.Auth.PUUID;
using ValorantClient.Lib.API.Network.Fetch;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.Player.XP
{
    public class XPHandler : IRequestHandler<XPQuery, XPResponse>
    {
        private readonly ILogger<XPHandler> _logger;
        private readonly IMediator _mediator;

        public XPHandler(
            ILogger<XPHandler> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<XPResponse> Handle(XPQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Get player xp");
            var puuid = await _mediator.Send(new PUUIDQuery());
            var response = await _mediator.Send(new FetchCommand
            {
                Endpoint = $"/account-xp/v1/players/{puuid}",
                Type = FetchCommand.EndpointType.Pd,
                Method = RestSharp.Method.Get
            });
            XPResponse xpResponse = JsonConvert.DeserializeObject<XPResponse>(response.Content);
            return xpResponse;
        }
    }
}
