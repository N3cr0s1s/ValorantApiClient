using MediatR;

namespace ValorantClient.Lib.API.Regions
{
    public class SetRegionCommand : IRequest<bool>
    {
        public SetRegionCommand(string region, string shard)
        {
            Region = region;
            Shard = shard;
        }

        public string Region { get; set; }
        public string Shard { get; set; }
    }
}
