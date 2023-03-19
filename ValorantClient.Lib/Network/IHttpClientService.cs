using RestSharp;

namespace ValorantClient.Lib.Network
{
    public interface IHttpClientService
    {

        public Task<IRestClient> CreateRestClientAsync(string uri,bool bypassSSL = false);

    }
}
