// <copyright file="IdentityServerClientTokenRequest.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System.Collections.Generic;

    public class IdentityServerClientTokenRequest
    {
        public int Lifetime { get; set; }

        public string ClientId { get; set; }

        public IEnumerable<string> Scopes { get; set; }

        public IEnumerable<string> Audiences { get; set; }
    }
}