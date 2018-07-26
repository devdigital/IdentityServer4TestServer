// <copyright file="IServerFactoryConfiguration.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Factories
{
    /// <summary>
    /// Server factory configuration.
    /// </summary>
    public interface IServerFactoryConfiguration
    {
        /// <summary>
        /// Configures the specified factory.
        /// </summary>
        /// <typeparam name="TServerFactory">The type of the server factory.</typeparam>
        /// <param name="factory">The factory.</param>
        void Configure<TServerFactory>(IdentityServer4TestServerFactory<TServerFactory> factory)
            where TServerFactory : IdentityServer4TestServerFactory<TServerFactory>;
    }
}