using System;
using Microsoft.Extensions.Logging;

namespace IdentityServer4TestServer
{
    internal class DefaultLoggingProvider : ILoggerProvider
    {
        private readonly ILoggerFactory loggerFactory;

        public DefaultLoggingProvider(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public void Dispose()
        {            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return this.loggerFactory.CreateLogger(categoryName);
        }
    }
}