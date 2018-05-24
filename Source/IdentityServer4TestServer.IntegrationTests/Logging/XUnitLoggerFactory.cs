// <copyright file="XUnitLoggerFactory.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.IntegrationTests.Logging
{
    using System;
    using Microsoft.Extensions.Logging;
    using Xunit.Abstractions;

    /// <summary>
    /// XUnit logger factory.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILoggerFactory" />
    public class XUnitLoggerFactory : ILoggerFactory
    {
        private readonly ITestOutputHelper output;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLoggerFactory"/> class.
        /// </summary>
        /// <param name="output">The output.</param>
        public XUnitLoggerFactory(ITestOutputHelper output)
        {
            this.output = output ?? throw new ArgumentNullException(nameof(output));
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(this.output);
        }

        /// <inheritdoc />
        public void AddProvider(ILoggerProvider provider)
        {
        }
    }
}