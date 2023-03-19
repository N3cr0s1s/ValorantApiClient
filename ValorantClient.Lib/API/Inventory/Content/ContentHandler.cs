using MediatR;
using Newtonsoft.Json;
using ValorantClient.Lib.API.Network.Fetch;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.Inventory.Content
{
    public class ContentHandler : IRequestHandler<ContentQuery, ContentResponse>
    {
        private readonly ILogger<ContentHandler> _logger;
        private readonly IMediator _mediator;
        private readonly ICache _cache;

        public ContentHandler(
            ILogger<ContentHandler> logger,
            IMediator mediator,
            ICache cache
            )
        {
            _logger = logger;
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<ContentResponse> Handle(
            ContentQuery request, 
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheValues.ContentResponse,out ContentResponse cachedResponse))
                return cachedResponse;

            var response = await _mediator.Send(new FetchCommand
            {
                Endpoint = "/content-service/v3/content",
                Method = RestSharp.Method.Get,
                Type = FetchCommand.EndpointType.Shared
            });

            ContentResponse contentResponse = JsonConvert.DeserializeObject<ContentResponse>(response.Content);

            await _cache.SetValueAsync(CacheValues.ContentResponse,contentResponse);

            return contentResponse;
        }
    }
}
