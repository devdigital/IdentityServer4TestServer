using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace IdentityServer4TestServer
{
    public class IdentityServerClient : IDisposable
    {
        private readonly IIdentityServer server;

        public IdentityServerClient(IIdentityServer server, string clientId, string clientSecret)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }

        public async Task<DiscoveryResponse> GetDiscovery()
        {
            using (var proxyHandler = this.server.CreateHandler())
            {
                using (var discoClient = new DiscoveryClient(this.server.BaseAddress.ToString(), proxyHandler))
                {
                    return await discoClient.GetAsync();
                }
            }
        }

        public async Task<TokenClient> CreateTokenClient()
        {
            var disco = await this.GetDiscovery();
            return new TokenClient(disco.TokenEndpoint);
        }

        public async Task<TokenClient> CreateTokenClient(HttpMessageHandler innerHttpMessageHandler)
        {
            var disco = await this.GetDiscovery();
            return new TokenClient(disco.TokenEndpoint, innerHttpMessageHandler);
        }

        public async Task<TokenClient> CreateTokenClient(HttpMessageHandler innerHttpMessageHandler, AuthenticationStyle authenticationStyle)
        {
            var disco = await this.GetDiscovery();
            return new TokenClient(disco.TokenEndpoint, this.ClientId, this.ClientSecret, innerHttpMessageHandler, authenticationStyle);
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

        public async Task<UserInfoResponse> GetUserInfo(string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            var disco = await this.GetDiscovery();
            using (var userInfoClient = new UserInfoClient(disco.UserInfoEndpoint))
            {
                return await userInfoClient.GetAsync(token, cancellationToken);
            }
        }

        public async Task<IntrospectionClient> CreateIntrospectionClient(HttpMessageHandler innerHttpMessageHandler, BasicAuthenticationHeaderStyle headerStyle = BasicAuthenticationHeaderStyle.Rfc6749)
        {
            var disco = await this.GetDiscovery();
            return new IntrospectionClient(
                endpoint: disco.IntrospectionEndpoint,
                clientId: this.ClientId,
                clientSecret: this.ClientSecret,
                innerHttpMessageHandler: innerHttpMessageHandler,
                headerStyle: headerStyle);
        }

        public async Task<RequestUrl> CreateRequestUrl()
        {
            var disco = await this.GetDiscovery();
            return new RequestUrl(disco.AuthorizeEndpoint);
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