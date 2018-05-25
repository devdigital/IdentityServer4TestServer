// <copyright file="IIdentityServer.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;
    using System.Net.Http;

    /// <summary>
    /// Identity server.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IIdentityServer : IDisposable
    {
        /// <summary>
        /// Gets the base address.
        /// </summary>
        /// <value>
        /// The base address.
        /// </value>
        Uri BaseAddress { get; }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <returns>The HTTP message handler.</returns>
        HttpMessageHandler CreateHandler();

        /// <summary>
        /// Creates tools for token generation.
        /// </summary>
        /// <returns>The tools.</returns>
        IdentityServerTokenFactory CreateTokenFactory();
    }
}