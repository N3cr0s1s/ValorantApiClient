using MediatR;
using ValorantClient.Lib.API.Network.Fetch;
using ValorantClient.Lib.API.PreGame.FetchPlayer;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.PreGame.QuitMatch
{
    public class QuitMatchHandler : IRequestHandler<QuitMatchCommand>
    {
        private readonly ILogger<QuitMatchHandler> _logger;
        private readonly IMediator _mediator;

        public QuitMatchHandler(
            ILogger<QuitMatchHandler> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Handle(QuitMatchCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Quit pre game");
            if (request.MatchId is null || request.MatchId.Equals(string.Empty))
            {
                _logger.LogDebug("MatchId is null, trying to get current match id");
                var player = await _mediator.Send(new GetPlayerQuery());
                request.MatchId = player.MatchID;
                _logger.LogDebug("New match id is: " + request.MatchId);
            }
            var response = await _mediator.Send(new FetchCommand
            {
                Endpoint = $"/pregame/v1/matches/{request.MatchId}/quit",
                Method = RestSharp.Method.Post,
                Type = FetchCommand.EndpointType.Glz,
                Exceptions = new()
                {
                    {404,"You are not in a pre-game" },
                    {409,"Pregame not in character select state" }
                }
            });
        }
    }
}
