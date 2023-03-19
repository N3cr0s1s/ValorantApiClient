using RestSharp;

namespace ValorantClient.Lib.Network
{
    public class HttpClientService : IHttpClientService
    {
        public async Task<IRestClient> CreateRestClientAsync(string uri, bool bypassSSL = false)
        {
            var options = new RestClientOptions(uri);
            if (bypassSSL)
            {
                options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }

            RestClient client = new RestClient(options);
            return client;
        }

    }
}
