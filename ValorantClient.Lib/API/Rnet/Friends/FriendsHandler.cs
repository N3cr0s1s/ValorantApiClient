using MediatR;
using Newtonsoft.Json;
using ValorantClient.Lib.API.Network.Fetch;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.Rnet.Friends
{
    public class FriendsHandler : IRequestHandler<FriendsQuery, FriendsResponse>
    {
        private readonly ILogger<FriendsHandler> _logger;
        private readonly IMediator _mediator;

        public FriendsHandler(
            ILogger<FriendsHandler> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<FriendsResponse> Handle(FriendsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Fetch Friends list");

            var resposne = await _mediator.Send(new FetchCommand()
            {
                Endpoint = "/chat/v4/friends",
                Type = FetchCommand.EndpointType.Local,
                Method = RestSharp.Method.Get
            });

            FriendsResponse friends = JsonConvert.DeserializeObject<FriendsResponse>(resposne.Content);

            return friends;
        }
    }
}
