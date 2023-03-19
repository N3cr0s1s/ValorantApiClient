using MediatR;
using RestSharp;

namespace ValorantClient.Lib.API.Auth.Headers
{
    /// <summary>
    /// Load headers
    /// </summary>
    public class HeadersQuery : IRequest<ICollection<KeyValuePair<string,string>>>
    {

    }
}
