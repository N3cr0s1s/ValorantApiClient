using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ValorantClient.Lib.API.Rnet.Friends
{
    public class FriendsResponse
    {
        [JsonPropertyName("friends")]
        public Friend[] Friends { get; set; }

        public class Friend
        {
            [JsonPropertyName("activePlatform")]
            [JsonProperty("activePlatform")]
            public object ActivePlatform { get; set; }

            [JsonPropertyName("displayGroup")]
            [JsonProperty("displayGroup")]
            public string DisplayGroup { get; set; }

            [JsonPropertyName("game_name")]
            [JsonProperty("game_name")]
            public string GameName { get; set; }

            [JsonPropertyName("game_tag")]
            [JsonProperty("game_tag")]
            public string GameTag { get; set; }

            [JsonPropertyName("group")]
            [JsonProperty("group")]
            public string Group { get; set; }

            [JsonPropertyName("last_online_ts")]
            [JsonProperty("last_online_ts")]
            public long? LastOnlineTs { get; set; }

            [JsonPropertyName("name")]
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonPropertyName("note")]
            [JsonProperty("note")]
            public string Note { get; set; }

            [JsonPropertyName("pid")]
            [JsonProperty("pid")]
            public string Pid { get; set; }

            [JsonPropertyName("puuid")]
            [JsonProperty("puuid")]
            public string Puuid { get; set; }

            [JsonPropertyName("region")]
            [JsonProperty("region")]
            public string Region { get; set; }
        }

    }
}
