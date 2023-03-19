using MediatR;
using RestSharp;
using ValorantClient.Lib.API.Auth.Headers;
using ValorantClient.Lib.API.Auth.LocalHeaders;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Config;
using ValorantClient.Lib.Logging;
using ValorantClient.Lib.Model;
using ValorantClient.Lib.Network;
using ValorantClient.Lib.RiotFiles.Lock;

namespace ValorantClient.Lib.API.Network.Fetch
{
    public class FetchHandler : IRequestHandler<FetchCommand, RestResponse>
    {
        private readonly ILogger<FetchHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly ILockFileService _lockFileService;
        private readonly ICache _cache;
        private readonly Model.Config _config;

        public FetchHandler(
            ILogger<FetchHandler> logger,
            IMediator mediator,
            IHttpClientService httpClientService,
            IConfiguration configuration,
            ILockFileService lockFileService,
            ICache cache
            )
        {
            _logger = logger;
            _mediator = mediator;
            _httpClientService = httpClientService;
            _configuration = configuration;
            _lockFileService = lockFileService;
            _cache = cache;
            _config = configuration.Parse<Model.Config>("config");
        }

        public async Task<RestResponse> Handle(FetchCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Fetch request: " + request.ToString());
            string baseUri = "";

            LockFile lockFile = await _lockFileService.LoadLockFileAsync(await _configuration["lockFile:path"]);

            switch (request.Type)
            {
                case FetchCommand.EndpointType.Pd:
                    baseUri = _config.BaseEndpoint;
                    break;
                case FetchCommand.EndpointType.Glz:
                    baseUri = _config.BaseEndpointGlz;
                    break;
                case FetchCommand.EndpointType.Shared:
                    baseUri = _config.BaseEndpointShared;
                    break;
                case FetchCommand.EndpointType.Local:
                    baseUri = _config.BaseEndpointLocal.Replace("{port}",lockFile.Port.ToString());
                    break;
                default:
                    throw new Exception("Invalid endpoint type : " + request.Type.ToString());
            }

            if (!_cache.TryGetValue(CacheValues.Region, out string region))
            {
                _logger.LogError("Region is null! Please set it!");
                throw new NullReferenceException("Region is null");
            }

            if (!_cache.TryGetValue(CacheValues.Shard, out string shard))
            {
                _logger.LogError("Shard is null! Please set it!");
                throw new NullReferenceException("Shard is null");
            }

            baseUri = baseUri
                .Replace("{shard}", shard)
                .Replace("{region}", shard);

            if (request.Endpoint.StartsWith("/"))
                baseUri = baseUri + request.Endpoint;
            else baseUri = baseUri + "/" + request.Endpoint;

            _logger.LogDebug($"Fetch url: {request.Method}: {baseUri}");

            IRestClient client = await _httpClientService.CreateRestClientAsync(baseUri,true);
            var restRequest = new RestRequest();

            ICollection<KeyValuePair<string, string>> headers;

            if (request.Type.Equals(FetchCommand.EndpointType.Local))
                headers = await _mediator.Send(new LocalHeadersQuery());
            else headers = await _mediator.Send(new HeadersQuery());

            restRequest.AddHeaders(headers);
            restRequest.Method = request.Method;

            RestResponse response = await client.ExecuteAsync(restRequest);

            if (request.Exceptions.ContainsKey((int)response.StatusCode))
                throw new Exception(request.Exceptions[(int)response.StatusCode]);

            return response;
        }
    }
}
