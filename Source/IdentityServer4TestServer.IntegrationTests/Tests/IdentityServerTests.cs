using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using IdentityServer4.Models;
using IdentityServer4TestServer.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace IdentityServer4TestServer.IntegrationTests.Tests
{
    public class IdentityServerTests
    {
        private readonly ITestOutputHelper output;

        public IdentityServerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [AutoData]
        public async Task GetTokenValidClientReturns200(
            TestIdentityServer4TestServerFactory serverFactory,
            TestIdentityServer4TestClientFactory clientFactory,
            string clientId,
            string clientSecret,
            string apiResourceName,
            string apiResourceDisplayName)
        {
            using (var server = serverFactory
                .WithClient(new Client
                {
                    ClientId = clientId,
                    ClientSecrets = new List<Secret> { new Secret(clientSecret.Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string> { apiResourceName }
                })
                .WithApiResource(new ApiResource(apiResourceName, apiResourceDisplayName))
                .WithLogging(new XUnitLoggerFactory(this.output))
                .Create())
            {
                using (var client = clientFactory
                    .WithClientId(clientId)
                    .WithClientSecret(clientSecret)
                    .Create(server))
                {
                    var response = await client.GetToken();
                    Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
                }                    
            }
        }

        [Theory]
        [AutoData]
        public async Task ConfigureServicesValidClientReturns200(
            TestIdentityServer4TestServerFactory serverFactory,
            TestIdentityServer4TestClientFactory clientFactory,
            string clientId,
            string clientSecret,
            string apiResourceName,
            string apiResourceDisplayName)
        {
            using (var server = serverFactory
                .WithConfigureServices((context, services) => 
                {
                    services
                        .AddIdentityServer()
                        .AddInMemoryClients(new List<Client>
                        {
                            new Client
                            {
                                ClientId = clientId,
                                ClientSecrets = new List<Secret> { new Secret(clientSecret.Sha256()) },
                                AllowedGrantTypes = GrantTypes.ClientCredentials,
                                AllowedScopes = new List<string> { apiResourceName }
                            }
                        })
                        .AddInMemoryApiResources(new List<ApiResource>
                        {
                            new ApiResource(apiResourceName, apiResourceDisplayName)
                        })
                        .AddDefaultEndpoints()
                        .AddDefaultSecretParsers()
                        .AddDefaultSecretValidators()
                        .AddDeveloperSigningCredential();
                })
                .WithLogging(new XUnitLoggerFactory(this.output))
                .Create())
            {
                using (var client = clientFactory
                    .WithClientId(clientId)
                    .WithClientSecret(clientSecret)
                    .Create(server))
                {
                    var response = await client.GetToken();
                    Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
                }
            }
        }
    }
}
