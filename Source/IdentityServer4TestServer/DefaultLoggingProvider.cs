// <copyright file="DefaultLoggingProvider.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Default logging provider.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILoggerProvider" />
    internal class DefaultLoggingProvider : ILoggerProvider
    {
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLoggingProvider"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public DefaultLoggingProvider(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return this.loggerFactory.CreateLogger(categoryName);
        }
    }
}