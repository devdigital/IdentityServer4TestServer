// <copyright file="TokenController.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using IdentityServer4;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Token controller.
    /// </summary>
    public class TokenController : Controller
    {
        private readonly IdentityServerTools identityServerTools;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="identityServerTools">The identity server tools.</param>
        public TokenController(IdentityServerTools identityServerTools)
        {
            this.identityServerTools =
                identityServerTools ?? throw new ArgumentNullException(nameof(identityServerTools));
        }

        /// <summary>
        /// Creates a token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The token.
        /// </returns>
        [HttpPost]
        [Route("api/test/token/create")]
        public async Task<IActionResult> CreateToken([FromBody]IdentityServerTokenRequest request)
        {
            var token = await this.identityServerTools.IssueJwtAsync(
                lifetime: request.Lifetime,
                claims: request.Claims);

            return this.Ok(new IdentityServerTokenResponse
            {
                Token = token,
            });
        }

        /// <summary>
        /// Creates a client token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The client token.
        /// </returns>
        [HttpPost]
        [Route("api/test/token/create-client")]
        public async Task<IActionResult> CreateClientToken([FromBody]IdentityServerClientTokenRequest request)
        {
            var token = await this.identityServerTools.IssueClientJwtAsync(
                lifetime: request.Lifetime,
                clientId: request.ClientId,
                scopes: request.Scopes,
                audiences: request.Audiences);

            return this.Ok(new IdentityServerTokenResponse
            {
                Token = token,
            });
        }
    }
}