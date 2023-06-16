# Valorant's API

This is a C# library which uses the Valorant API , and a little Insta locker client.
I don't plan to maintain the api, so I made it open source.
You can easily update it by editing the config.json file.

##  Cigger

![image](https://github.com/N3cr0s1s/ValorantApiClient/assets/60231561/f62ffda9-d08b-48c2-a78b-451a65e19248)

You can navigate with arrows, and select with Enter.
Probably detected, but I don't know any person who get ban for this.

###  Instalock

Lock your agent instantly, use this after "Match found" sound.

![image](https://github.com/N3cr0s1s/ValorantApiClient/assets/60231561/eb15fbe6-1faa-43b4-8a4b-38da014225a3)

###  Dodge queue

You can use this if you haven't chosen an agent yet.

###  Open logs

Read logs, exit with any key

![image](https://github.com/N3cr0s1s/ValorantApiClient/assets/60231561/b22ebdf6-f7cd-4f2a-860f-7ce14ed0373c)

##  Lib

Library basically a little framework? Idk. Uses dependency injection, and MediatR to send requests.
RestSharp responsible for request sending.

### Create lib

```csharp
var builder = new ValoClientBuilder();

//  Set Logger
builder.Services.AddLogger(
    opt => {
        opt.Debug = true;
        opt.Logger = typeof(MemoryLogger<>);
    });

(ServiceProvider Provider, IMediator Mediator) client = await builder.BuildAsync();

//  Get dependencies
var configuration = client.Provider.GetRequiredService<IConfiguration>();

//  Make request
await client.Mediator.Send(
    new SelectAgentQuery()
    {
        AgentId = agents[selectedAgent] //  <-  This readed from config file
    }
);
```

> **_NOTE:_**  If you send a request without prior authentication, authentication will take place automatically using the lockfile.

> **_WARNING:_**  The lockfile only exists when Valorant is running

### Caching

Caching supported some ways. Bugs can be occurred with some miss-cached values.

```csharp
public class VersionHandler : IRequestHandler<VersionQuery, string>
{

    private readonly ICache _cache;
    
    public VersionHandler(
        ICache cache
        )
    {
        _cache = cache;
    }

    public async Task<string> Handle(VersionQuery request, CancellationToken cancellationToken)
    {

        if (_cache.TryGetValue(CacheValues.ShippingVersion.ToString(),out string cachedVersion))
        {
            return cachedVersion;
        }

        string version = "{response.Data.Branch}-shipping-{response.Data.BuildVersion}-{response.Data.Version.Split('.')[3]}";

        await _cache.SetValueAsync(CacheValues.ShippingVersion.ToString(), version);

        return version;
    }
}
```

### Config

The config.json file contains regions,endpoints,shards,queues, lockfile path, agents with ids, etc.
config.json supports env variables, just use like this: `env(LOCALAPPDATA)`, and this will be replaced with the appdata absolute directory.

### Requests

**ValorantClient.Lib.API.Auth.Entitlements.EntitlementQuery**: AccessToken-Entitlements-Issuer-Subject-Token </br>
**ValorantClient.Lib.API.Auth.Headers.HeadersQuery**: Get main headers, used in other requests </br>
**ValorantClient.Lib.API.Auth.LocalHeaders.LocalHeadersQuery**: Get local headers, used in local requests </br>
**ValorantClient.Lib.API.Auth.PUUID.PUUIDQuery**: Get PUUID </br>

**ValorantClient.Lib.API.Chat.ChatQuery**: ChatResponse.cs </br>

**ValorantClient.Lib.API.Inventory.Content.ContentQuery**: ContentResponse.cs </br>
**ValorantClient.Lib.API.Inventory.Entitlements.EntitlementsQuery**: EntitlementsResponse.cs </br>

**ValorantClient.Lib.API.Network.Fetch.FetchCommand**: Make request to Valorant endpoints (Pd, Glz, Shared, Local) </br>

**ValorantClient.Lib.API.Player.XP.XPQuery**: Get Player's xp, see XPResponse.cs to more detailed version </br>

> **_NOTE:_**  I think that's what you came for

**ValorantClient.Lib.API.PreGame.FetchPlayer.GetPlayerQuery**: Get Player's match id, version, and subject, GetPlayerResponse.cs </br>
**ValorantClient.Lib.API.PreGame.QuitMatch.QuitMatchCommand**: If you call this, you can dodge queue. This only can be used in pregame like agent select. </br>
**ValorantClient.Lib.API.PreGame.SelectAgent.SelectAgentQuery**: Lock agents with this query. </br>


**ValorantClient.Lib.API.Regions.SetRegionCommand**: Set the region, and shard. Default is eu </br>

**ValorantClient.Lib.API.Rnet.Friends.FriendsQuery**: Get friends </br>
**ValorantClient.Lib.API.Rnet.Settings.SettingsQuery**: Get settings (Response not implemented!!!!! all the stuff logged to console) </br>

**ValorantClient.Lib.API.Version.VersionQuery**: Get version </br>
