using MediatR;
using Newtonsoft.Json;
using ValorantClient.Lib.API.Auth.PUUID;
using ValorantClient.Lib.API.Network.Fetch;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.PreGame.FetchPlayer
{
    public class GetPlayerHandler : IRequestHandler<GetPlayerQuery, GetPlayerResponse>
    {
        private readonly ILogger<GetPlayerHandler> _logger;
        private readonly IMediator _mediator;

        public GetPlayerHandler(
            ILogger<GetPlayerHandler> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<GetPlayerResponse> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
        {
            string puuid = await _mediator.Send(new PUUIDQuery());

            var resp = await _mediator.Send(new FetchCommand
            {
                Endpoint = "/pregame/v1/players/" + puuid,
                Method = RestSharp.Method.Get,
                Type = FetchCommand.EndpointType.Glz,
                Exceptions = new Dictionary<int, string>()
                {
                    { 404, "You are not in pregame" }
                }
            });

            var playerResponse = JsonConvert.DeserializeObject<GetPlayerResponse>(resp.Content);
            return playerResponse;
        }
    }
}
