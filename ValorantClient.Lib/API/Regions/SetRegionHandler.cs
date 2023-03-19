using MediatR;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Config;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Lib.API.Regions
{
    public class SetRegionHandler : IRequestHandler<SetRegionCommand, bool>
    {
        private readonly ILogger<SetRegionHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICache _cache;
        private readonly Model.Config _config;

        public SetRegionHandler(
            ILogger<SetRegionHandler> logger,
            IConfiguration configuration,
            ICache cache
            )
        {
            _logger = logger;
            _configuration = configuration;
            _cache = cache;
            _config = configuration.Parse<Model.Config>("config");
        }

        public async Task<bool> Handle(
            SetRegionCommand request, 
            CancellationToken cancellationToken)
        {
            _logger.LogDebug("Setting up regions");

            if (!_config.Regions.Contains(request.Region))
            {
                _logger.LogError("Invalid region value! Region: " + request.Region);
                _logger.LogError("Allowed values: " + string.Join(',', _config.Regions));
                return false;
            }

            if (!_config.Regions.Contains(request.Shard))
            {
                _logger.LogError("Invalid shard value! Shard: " + request.Shard);
                _logger.LogError("Allowed values: " + string.Join(',', _config.Regions));
                return false;
            }

            foreach(string regionShard in _config.RegionShardOverride)
            {
                string[] overrides = regionShard.Split(':');

                if (!overrides.Contains(request.Region)) 
                    continue;

                string oldRegion = request.Region;
                request.Region = overrides[1];

                _logger.LogDebug("Region Shard Override");
                _logger.LogDebug($"{oldRegion} => {request.Region}");
            }

            foreach (string shardRegion in _config.ShardRegionOverride)
            {
                string[] overrides = shardRegion.Split(':');

                if (!overrides.Contains(request.Shard))
                    continue;

                string oldShard = request.Shard;
                request.Shard = overrides[1];

                _logger.LogDebug("Shard Region Override");
                _logger.LogDebug($"{oldShard} => {request.Shard}");
            }

            await _cache.SetValueAsync(CacheValues.Region, request.Region);
            await _cache.SetValueAsync(CacheValues.Shard, request.Shard);
            return true;
        }
    }
}
