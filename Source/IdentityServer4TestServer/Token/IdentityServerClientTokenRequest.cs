// <copyright file="IdentityServerClientTokenRequest.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Token
{
    using System.Collections.Generic;

    /// <summary>
    /// Identity server client token request.
    /// </summary>
    public class IdentityServerClientTokenRequest
    {
        /// <summary>
        /// Gets or sets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public int Lifetime { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        public IEnumerable<string> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the audiences.
        /// </summary>
        /// <value>
        /// The audiences.
        /// </value>
        public IEnumerable<string> Audiences { get; set; }
    }
}