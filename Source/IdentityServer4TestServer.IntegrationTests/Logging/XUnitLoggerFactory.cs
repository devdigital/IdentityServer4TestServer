using System;
using IdentityServer4TestServer.IntegrationTests.Logging;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace IdentityServer4TestServer.IntegrationTests
{
    public class XUnitLoggerFactory : ILoggerFactory
    {
        private readonly ITestOutputHelper output;

        public XUnitLoggerFactory(ITestOutputHelper output)
        {
            this.output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public void Dispose()
        {            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(this.output);
        }

        public void AddProvider(ILoggerProvider provider)
        {         
        }
    }
}