// <copyright file="IdentityServer.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Factories
{
    using System;
    using IdentityServer4TestServer.Token;
    using Microsoft.AspNetCore.TestHost;

    /// <summary>
    /// Identity server.
    /// </summary>
    /// <seealso cref="IIdentityServer" />
    public class IdentityServer : IIdentityServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServer"/> class.
        /// </summary>
        /// <param name="testServer">The test server.</param>
        public IdentityServer(TestServer testServer)
        {
            this.Value = testServer ?? throw new ArgumentNullException(nameof(testServer));
        }

        /// <inheritdoc/>
        public TestServer Value { get; }

        /// <inheritdoc />
        public IdentityServerTokenFactory CreateTokenFactory()
        {
            return new IdentityServerTokenFactory(
                this.Value);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Value.Dispose();
        }
    }
}