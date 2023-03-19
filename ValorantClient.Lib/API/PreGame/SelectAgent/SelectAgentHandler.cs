using MediatR;
using Newtonsoft.Json;
using ValorantClient.Lib.API.Network.Fetch;
using ValorantClient.Lib.API.PreGame.FetchPlayer;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.PreGame.SelectAgent
{
    public class SelectAgentHandler : IRequestHandler<SelectAgentQuery,SelectAgentResponse>
    {
        private readonly ILogger<SelectAgentHandler> _logger;
        private readonly IMediator _mediator;

        public SelectAgentHandler(
            ILogger<SelectAgentHandler> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<SelectAgentResponse> Handle(SelectAgentQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Select agent");
            _logger.LogDebug("Agent: " + request.AgentId + " , MatchId: " + request.MatchId);
            if (request.MatchId is null || request.MatchId.Equals(string.Empty))
            {
                _logger.LogDebug("MatchId is null, trying to get current match id");
                var player = await _mediator.Send(new GetPlayerQuery());
                request.MatchId = player.MatchID;
                _logger.LogDebug("New match id is: " + request.MatchId);
            }

            var resp = await _mediator.Send(new FetchCommand
            {
                Endpoint = $"/pregame/v1/matches/{request.MatchId}/lock/{request.AgentId}",
                Method = RestSharp.Method.Post,
                Type = FetchCommand.EndpointType.Glz,
                Exceptions = new Dictionary<int, string>
                {
                    { 404 , "User not in pre-game" }
                }
            });

            SelectAgentResponse response = JsonConvert.DeserializeObject<SelectAgentResponse>(resp.Content);
            return response;
        }
    }
}
