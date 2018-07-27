// <copyright file="IServerFactoryConfiguration.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Factories
{
    /// <summary>
    /// Server factory configuration.
    /// </summary>
    /// <typeparam name="TServerFactory">The type of the server factory.</typeparam>
    public interface IServerFactoryConfiguration<in TServerFactory>
    {
        /// <summary>
        /// Configures the specified factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        void Configure(TServerFactory factory);
    }
}