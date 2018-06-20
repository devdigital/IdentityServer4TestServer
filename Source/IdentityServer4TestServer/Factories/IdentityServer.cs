// <copyright file="IdentityServer.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Factories
{
    using System;
    using System.Net.Http;
    using Microsoft.AspNetCore.TestHost;

    /// <summary>
    /// Identity server.
    /// </summary>
    /// <seealso cref="IIdentityServer" />
    public class IdentityServer : IIdentityServer
    {
        private readonly TestServer testServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServer"/> class.
        /// </summary>
        /// <param name="testServer">The test server.</param>
        public IdentityServer(TestServer testServer)
        {
            this.testServer = testServer ?? throw new ArgumentNullException(nameof(testServer));
        }

        /// <inheritdoc />
        public Uri BaseAddress => this.testServer.BaseAddress;

        /// <inheritdoc />
        public void Dispose()
        {
            this.testServer.Dispose();
        }

        /// <inheritdoc />
        public HttpMessageHandler CreateHandler()
        {
            return this.testServer.CreateHandler();
        }

        /// <inheritdoc />
        public IdentityServerTokenFactory CreateTokenFactory()
        {
            return new IdentityServerTokenFactory(
                this.testServer);
        }
    }
}