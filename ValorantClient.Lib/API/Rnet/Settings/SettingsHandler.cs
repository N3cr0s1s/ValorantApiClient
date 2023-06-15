using MediatR;
using ValorantClient.Lib.API.Network.Fetch;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.Rnet.Settings
{
    public class SettingsHandler : IRequestHandler<SettingsQuery, SettingsResponse>
    {
        private readonly ILogger<SettingsHandler> _logger;
        private readonly IMediator _mediator;

        public SettingsHandler(
            ILogger<SettingsHandler> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<SettingsResponse> Handle(
            SettingsQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogDebug("Fetch settings");

            var respone = await _mediator.Send(new FetchCommand
            {
                Endpoint= "/player-preferences/v1/data-json/Ares.PlayerSettings",
                Type=FetchCommand.EndpointType.Local,
                Method=RestSharp.Method.Get,
            });

            _logger.LogInformation(respone.Content);
            return null;
        }
    }
}
