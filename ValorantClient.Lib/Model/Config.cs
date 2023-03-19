namespace ValorantClient.Lib.Model
{
    [Serializable]
    public class Config
    {
        public string BaseEndpointLocal { get; set; } = string.Empty;
        public string BaseEndpoint { get; set; } = string.Empty;
        public string BaseEndpointGlz { get; set; } = string.Empty;
        public string BaseEndpointShared { get; set; } = string.Empty;
        public string[] Regions { get; set; } = new string[0];
        public string[] RegionShardOverride { get; set; } = new string[0];
        public string[] ShardRegionOverride { get; set; } = new string[0];
        public string[] Queues { get; set; } = new string[0];
        public string ClientPlatform { get; set; } = string.Empty;

        public Config(string baseEndpointLocal, string baseEndpoint, string baseEndpointGlz, string baseEndpointShared, string[] regions, string[] regionShardOverride, string[] shardRegionOverride, string[] queues, string clientPlatform)
        {
            BaseEndpointLocal = baseEndpointLocal;
            BaseEndpoint = baseEndpoint;
            BaseEndpointGlz = baseEndpointGlz;
            BaseEndpointShared = baseEndpointShared;
            Regions = regions;
            RegionShardOverride = regionShardOverride;
            ShardRegionOverride = shardRegionOverride;
            Queues = queues;
            ClientPlatform = clientPlatform;
        }

        public Config() {}
    }

}
