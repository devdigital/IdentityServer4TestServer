using System;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;

namespace IdentityServer4TestServer
{
    public class IdentityServer : IIdentityServer
    {
        private readonly TestServer testServer;

        public IdentityServer(TestServer testServer)
        {
            this.testServer = testServer ?? throw new ArgumentNullException(nameof(testServer));
        }

        public void Dispose()
        {
            this.testServer.Dispose();
        }

        public HttpMessageHandler CreateHandler()
        {
            return this.testServer.CreateHandler();
        }

        public Uri BaseAddress
        {
            get => this.testServer.BaseAddress;
            set => this.testServer.BaseAddress = value;
        }
    }
}