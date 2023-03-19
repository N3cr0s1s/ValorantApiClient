using MediatR;
using Newtonsoft.Json;
using RestSharp;
using ValorantClient.Lib.API.Network.Fetch;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.Chat
{
    public class ChatHandler : IRequestHandler<ChatQuery, RestResponse>
    {
        private readonly ILogger<ChatHandler> _logger;
        private readonly IMediator _mediator;
        private readonly ICache _cache;

        public ChatHandler(
            ILogger<ChatHandler> logger,
            IMediator mediator,
            ICache cache
            )
        {
            _logger = logger;
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<RestResponse> Handle(ChatQuery request, CancellationToken cancellationToken)
        {
            var resp = await _mediator.Send(new FetchCommand
            {
                Endpoint = "/chat/v1/session",
                Method = Method.Get,
                Type = FetchCommand.EndpointType.Local
            });
            ChatResponse chatResponse = JsonConvert.DeserializeObject<ChatResponse>(resp.Content);
            await _cache.SetValueAsync(CacheValues.GameTag, chatResponse.Tag);
            await _cache.SetValueAsync(CacheValues.Username, chatResponse.Username);
            await _cache.SetValueAsync(CacheValues.ChatResponse, chatResponse);
            return resp;
        }
    }
}
