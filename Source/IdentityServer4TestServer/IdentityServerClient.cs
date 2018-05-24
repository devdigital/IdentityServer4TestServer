// <copyright file="IdentityServerClient.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using IdentityModel.Client;

    /// <summary>
    /// Identity server client.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class IdentityServerClient : IDisposable
    {
        private readonly IIdentityServer server;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServerClient"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        public IdentityServerClient(IIdentityServer server, string clientId, string clientSecret)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }

        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; }

        /// <summary>
        /// Gets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; }

        /// <summary>
        /// Gets the discovery response.
        /// </summary>
        /// <returns>The discovery response.</returns>
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

        /// <summary>
        /// Creates a token client.
        /// </summary>
        /// <returns>The token client.</returns>
        public async Task<TokenClient> CreateTokenClient()
        {
            var disco = await this.GetDiscovery();
            return new TokenClient(disco.TokenEndpoint);
        }

        /// <summary>
        /// Creates a token client.
        /// </summary>
        /// <param name="innerHttpMessageHandler">The inner HTTP message handler.</param>
        /// <returns>The token client.</returns>
        public async Task<TokenClient> CreateTokenClient(HttpMessageHandler innerHttpMessageHandler)
        {
            var disco = await this.GetDiscovery();
            return new TokenClient(disco.TokenEndpoint, innerHttpMessageHandler);
        }

        /// <summary>
        /// Creates the token client.
        /// </summary>
        /// <param name="innerHttpMessageHandler">The inner HTTP message handler.</param>
        /// <param name="authenticationStyle">The authentication style.</param>
        /// <returns>The token client.</returns>
        public async Task<TokenClient> CreateTokenClient(
            HttpMessageHandler innerHttpMessageHandler,
            AuthenticationStyle authenticationStyle)
        {
            var disco = await this.GetDiscovery();
            return new TokenClient(disco.TokenEndpoint, this.ClientId, this.ClientSecret, innerHttpMessageHandler, authenticationStyle);
        }

        /// <summary>
        /// Gets a token.
        /// </summary>
        /// <returns>The token response.</returns>
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

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The user information.</returns>
        public async Task<UserInfoResponse> GetUserInfo(string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            var disco = await this.GetDiscovery();
            using (var userInfoClient = new UserInfoClient(disco.UserInfoEndpoint))
            {
                return await userInfoClient.GetAsync(token, cancellationToken);
            }
        }

        /// <summary>
        /// Creates the introspection client.
        /// </summary>
        /// <param name="innerHttpMessageHandler">The inner HTTP message handler.</param>
        /// <param name="headerStyle">The header style.</param>
        /// <returns>The introspection client.</returns>
        public async Task<IntrospectionClient> CreateIntrospectionClient(
            HttpMessageHandler innerHttpMessageHandler,
            BasicAuthenticationHeaderStyle headerStyle = BasicAuthenticationHeaderStyle.Rfc6749)
        {
            var disco = await this.GetDiscovery();
            return new IntrospectionClient(
                endpoint: disco.IntrospectionEndpoint,
                clientId: this.ClientId,
                clientSecret: this.ClientSecret,
                innerHttpMessageHandler: innerHttpMessageHandler,
                headerStyle: headerStyle);
        }

        /// <summary>
        /// Creates the request URL.
        /// </summary>
        /// <returns>The request URL.</returns>
        public async Task<RequestUrl> CreateRequestUrl()
        {
            var disco = await this.GetDiscovery();
            return new RequestUrl(disco.AuthorizeEndpoint);
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <returns>The HTTP response handler.</returns>
        public HttpMessageHandler CreateHandler()
        {
            return this.server.CreateHandler();
        }
    }
}