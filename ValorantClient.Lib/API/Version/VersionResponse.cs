namespace ValorantClient.Lib.API.Version
{

    public class VersionResponse
    {
        public int Status { get; set; }
        public VersionData Data { get; set; }

        public class VersionData
        {
            public string ManifestId { get; set; }
            public string Branch { get; set; }
            public string Version { get; set; }
            public string BuildVersion { get; set; }
            public string EngineVersion { get; set; }
            public string RiotClientVersion { get; set; }
            public string RiotClientBuild { get; set; }
            public DateTime BuildDate { get; set; }
        }

    }



}
