using MediatR;
using RestSharp;

namespace ValorantClient.Lib.API.Auth.LocalHeaders
{
    /// <summary>
    /// Query to get local host headers.
    /// </summary>
    public class LocalHeadersQuery : IRequest<ICollection<KeyValuePair<string,string>>>
    {

    }
}
