using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace IdentityServer4TestServer
{
    public class IdentityServerClient : IDisposable
    {
        private readonly IIdentityServer server;

        public IdentityServerClient(IIdentityServer server, string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }

        public async Task<TokenResponse> GetToken()
        {
            using (var proxyHandler = this.server.CreateHandler())
            {
                using (var discoClient = new DiscoveryClient(this.server.BaseAddress.ToString(), proxyHandler))
                {
                    var disco = await discoClient.GetAsync();

                    using (var tokenClient = new TokenClient(disco.TokenEndpoint, this.ClientId, this.ClientSecret, proxyHandler))
                    {
                        var tokenResponse = await tokenClient.RequestClientCredentialsAsync();
                        return tokenResponse;
                    }                        
                }                    
            }
        }

        public void Dispose()
        {
        }

        public HttpMessageHandler CreateHandler()
        {
            return this.server.CreateHandler();
        }

        public string ClientId { get; }

        public string ClientSecret { get; }
    }
}