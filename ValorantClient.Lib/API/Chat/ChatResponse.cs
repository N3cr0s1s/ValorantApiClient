using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ValorantClient.Lib.API.Chat
{
    public class ChatResponse
    {
        [JsonProperty("federated")]
        [JsonPropertyName("federated")]
        public bool Federated { get; set; }

        [JsonProperty("game_name")]
        [JsonPropertyName("game_name")]
        public string Username { get; set; }

        [JsonProperty("game_tag")]
        [JsonPropertyName("game_tag")]
        public string Tag { get; set; }

        [JsonProperty("loaded")]
        [JsonPropertyName("loaded")]
        public bool Loaded { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("pid")]
        [JsonPropertyName("pid")]
        public string Pid { get; set; }

        [JsonProperty("puuid")]
        [JsonPropertyName("puuid")]
        public string PUUID { get; set; }

        [JsonProperty("region")]
        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonProperty("resource")]
        [JsonPropertyName("resource")]
        public string Resource { get; set; }

        [JsonProperty("state")]
        [JsonPropertyName("state")]
        public string State { get; set; }
    }

}
