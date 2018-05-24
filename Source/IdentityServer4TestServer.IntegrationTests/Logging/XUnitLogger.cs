// <copyright file="XUnitLogger.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.IntegrationTests.Logging
{
    using System;
    using Microsoft.Extensions.Logging;
    using Xunit.Abstractions;

    /// <summary>
    /// XUnit logger.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger" />
    public class XUnitLogger : ILogger
    {
        private readonly ITestOutputHelper output;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLogger"/> class.
        /// </summary>
        /// <param name="output">The output.</param>
        public XUnitLogger(ITestOutputHelper output)
        {
            this.output = output ?? throw new ArgumentNullException(nameof(output));
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.output.WriteLine(formatter(state, exception));
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}