using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace IdentityServer4TestServer.IntegrationTests.Logging
{
    public class XUnitLogger : ILogger
    {
        private readonly ITestOutputHelper output;

        public XUnitLogger(ITestOutputHelper output)
        {
            this.output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.output.WriteLine(formatter(state, exception));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}