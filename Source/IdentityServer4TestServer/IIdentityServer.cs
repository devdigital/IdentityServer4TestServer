using System;
using System.Net.Http;

namespace IdentityServer4TestServer
{
    public interface IIdentityServer : IDisposable
    {
        HttpMessageHandler CreateHandler();

        Uri BaseAddress { get; set; }
    }
}