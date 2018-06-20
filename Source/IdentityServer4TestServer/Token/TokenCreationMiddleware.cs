// <copyright file="TokenCreationMiddleware.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Token
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using IdentityServer4;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Newtonsoft.Json;

    /// <summary>
    /// Token creation middleware.
    /// Adds endpoints that are used for token generation.
    /// </summary>
    internal class TokenCreationMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IApplicationBuilder app;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenCreationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="app">The application.</param>
        public TokenCreationMiddleware(RequestDelegate next, IApplicationBuilder app)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.app = app ?? throw new ArgumentNullException(nameof(app));
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="identityServerTools">The identity server tools.</param>
        /// <returns>The task.</returns>
        public async Task Invoke(HttpContext context, IdentityServerTools identityServerTools)
        {
            var body = GetBody(context);

            if (context.Request.Path.StartsWithSegments(new PathString("/api/test/token/create")))
            {
                var request = JsonConvert.DeserializeObject<IdentityServerTokenRequest>(body);
                await CreateToken(context, request, identityServerTools);
                return;
            }

            if (context.Request.Path.StartsWithSegments(new PathString("/api/test/token/create-client")))
            {
                var request = JsonConvert.DeserializeObject<IdentityServerClientTokenRequest>(body);
                await CreateClientToken(context, request, identityServerTools);
                return;
            }

            await this.next(context);
        }

        private static string GetBody(HttpContext context)
        {
            string body;
            context.Request.EnableRewind();

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                body = reader.ReadToEnd();
            }

            context.Request.Body.Position = 0;
            return body;
        }

        private static async Task CreateToken(
            HttpContext context,
            IdentityServerTokenRequest request,
            IdentityServerTools identityServerTools)
        {
            var token = await identityServerTools.IssueJwtAsync(
                lifetime: request.Lifetime,
                claims: request.Claims);

            var response = new IdentityServerTokenResponse
            {
                Token = token,
            };

            await context.Response.WriteJsonAsync(response);
        }

        private static async Task CreateClientToken(
            HttpContext context,
            IdentityServerClientTokenRequest request,
            IdentityServerTools identityServerTools)
        {
            var token = await identityServerTools.IssueClientJwtAsync(
                lifetime: request.Lifetime,
                clientId: request.ClientId,
                scopes: request.Scopes,
                audiences: request.Audiences);

            var response = new IdentityServerTokenResponse
            {
                Token = token,
            };

            await context.Response.WriteJsonAsync(response);
        }
    }
}