// <copyright file="IdentityServerTokenRequest.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System.Collections.Generic;
    using IdentityServer4TestServer.Token;

    /// <summary>
    /// Identity server token request.
    /// </summary>
    public class IdentityServerTokenRequest
    {
        /// <summary>
        /// Gets or sets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public int Lifetime { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public List<SerializableClaim> Claims { get; set; }
    }
}