using MediatR;
using RestSharp;

namespace ValorantClient.Lib.API.Network.Fetch
{
    public class FetchCommand : IRequest<RestResponse>
    {

        public string Endpoint { get; set; }

        public Method Method { get; set; }

        public Dictionary<int, string> Exceptions { get; set; } = new Dictionary<int, string>();

        public EndpointType Type { get; set; }

        public enum EndpointType
        {
            Pd,
            Glz,
            Shared,
            Local
        }

        public override string? ToString()
        {
            return Endpoint + " : " + Type.ToString();
        }
    }
}
