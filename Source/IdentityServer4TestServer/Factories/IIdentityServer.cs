// <copyright file="IIdentityServer.cs" company="DevDigital">
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
    /// <seealso cref="System.IDisposable" />
    public interface IIdentityServer : IDisposable
    {
        /// <summary>
        /// Gets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        TestServer Value { get; }

        /// <summary>
        /// Creates tools for token generation.
        /// </summary>
        /// <returns>The tools.</returns>
        IdentityServerTokenFactory CreateTokenFactory();
    }
}