// <copyright file="IdentityServer4TestClientFactory.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;

    /// <summary>
    /// Identity server test client factory.
    /// </summary>
    /// <typeparam name="TClientFactory">The type of the client factory.</typeparam>
    public abstract class IdentityServer4TestClientFactory<TClientFactory>
        where TClientFactory : IdentityServer4TestClientFactory<TClientFactory>
    {
        private string currentClientId;

        private string currentClientSecret;

        /// <summary>
        /// Adds a client identifier.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns>The factory.</returns>
        public TClientFactory WithClientId(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            this.currentClientId = clientId;
            return this as TClientFactory;
        }

        /// <summary>
        /// Adds a client secret.
        /// </summary>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns>The factory.</returns>
        public TClientFactory WithClientSecret(string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            this.currentClientSecret = clientSecret;
            return this as TClientFactory;
        }

        /// <summary>
        /// Creates the test client using the specified server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns>The test client.</returns>
        public virtual IdentityServerClient Create(IIdentityServer server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            return new IdentityServerClient(
                server: server,
                clientId: this.currentClientId,
                clientSecret: this.currentClientSecret);
        }
    }
}