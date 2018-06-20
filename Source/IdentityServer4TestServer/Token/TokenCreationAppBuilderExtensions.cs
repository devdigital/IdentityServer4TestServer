// <copyright file="TokenCreationAppBuilderExtensions.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Token
{
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Token creation app builder extensions.
    /// </summary>
    internal static class TokenCreationAppBuilderExtensions
    {
        /// <summary>
        /// Use token creation.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseTokenCreation(this IApplicationBuilder app)
        {
            app.UseMiddleware<TokenCreationMiddleware>(app);
        }
    }
}