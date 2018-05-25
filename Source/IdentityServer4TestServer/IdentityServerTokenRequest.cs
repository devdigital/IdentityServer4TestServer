// <copyright file="IdentityServerTokenRequest.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public class IdentityServerTokenRequest
    {
        public int Lifetime { get; set; }

        public List<Claim> Claims { get; set; }
    }
}