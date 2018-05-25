// <copyright file="IdentityServerTokenFactory.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.TestHost;
    using Newtonsoft.Json;

    /// <summary>
    /// Identity server token factory.
    /// </summary>
    public class IdentityServerTokenFactory
    {
        private readonly TestServer testServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServerTokenFactory" /> class.
        /// </summary>
        /// <param name="testServer">The test server.</param>
        public IdentityServerTokenFactory(TestServer testServer)
        {
            this.testServer = testServer ?? throw new ArgumentNullException(nameof(testServer));
        }

        /// <summary>
        /// Creates a token.
        /// </summary>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="claims">The claims.</param>
        /// <returns>The token.</returns>
        public async Task<string> CreateToken(int lifetime, List<Claim> claims)
        {
            var json = JsonConvert.SerializeObject(new IdentityServerTokenRequest
            {
                Lifetime = lifetime,
                Claims = claims,
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = this.testServer.CreateClient())
            {
                var response = await client.PostAsync("api/test/token/create", content);
                var responseString = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<IdentityServerTokenResponse>(responseString);
                return token.Token;
            }
        }

        /// <summary>
        /// Creates a client token.
        /// </summary>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="audiences">The audiences.</param>
        /// <returns>The client token.</returns>
        public async Task<string> CreateClientToken(
            int lifetime,
            string clientId,
            IEnumerable<string> scopes,
            IEnumerable<string> audiences)
        {
            var json = JsonConvert.SerializeObject(new IdentityServerClientTokenRequest
            {
                Lifetime = lifetime,
                ClientId = clientId,
                Scopes = scopes,
                Audiences = audiences,
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = this.testServer.CreateClient())
            {
                var response = await client.PostAsync("api/test/token/create-client", content);
                var responseString = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<IdentityServerTokenResponse>(responseString);
                return token.Token;
            }
        }
    }
}