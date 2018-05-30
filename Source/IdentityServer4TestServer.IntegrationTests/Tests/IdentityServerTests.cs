// <copyright file="IdentityServerTests.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.IntegrationTests.Tests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoFixture.Xunit2;
    using IdentityModel.Client;
    using IdentityServer4.Models;
    using IdentityServer4TestServer.IntegrationTests.Helpers;
    using IdentityServer4TestServer.IntegrationTests.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using Xunit.Abstractions;

    // ReSharper disable StyleCop.SA1600
    #pragma warning disable SA1600
    #pragma warning disable 1591

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
                    AllowedScopes = new List<string> { apiResourceName },
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
        public async Task InvalidClientIdRaiseUnknownClientEvent(
            TestIdentityServer4TestServerFactory serverFactory,
            TestIdentityServer4TestClientFactory clientFactory,
            string clientId,
            string clientSecret,
            string apiResourceName,
            string apiResourceDisplayName,
            IdentityServerEventCapture eventCapture)
        {
            using (var server = serverFactory
                .WithApiResource(new ApiResource(apiResourceName, apiResourceDisplayName))
                .WithEventCapture(eventCapture)
                .Create())
            {
                using (var client = clientFactory
                    .WithClientId(clientId)
                    .WithClientSecret(clientSecret)
                    .Create(server))
                {
                    await client.GetToken();
                    Assert.True(eventCapture.ContainsMessage("Unknown client"));
                }
            }
        }

        [Theory]
        [AutoData]
        public async Task InvalidClientSecretRaisesInvalidClientSecretEvent(
            TestIdentityServer4TestServerFactory serverFactory,
            TestIdentityServer4TestClientFactory clientFactory,
            string clientId,
            string clientSecret,
            string invalidClientSecret,
            string apiResourceName,
            string apiResourceDisplayName,
            IdentityServerEventCapture eventCapture)
        {
            using (var server = serverFactory
                .WithClient(new Client
                {
                    ClientId = clientId,
                    ClientSecrets = new List<Secret> { new Secret(clientSecret.Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string> { apiResourceName },
                })
                .WithApiResource(new ApiResource(apiResourceName, apiResourceDisplayName))
                .WithEventCapture(eventCapture)
                .Create())
            {
                using (var client = clientFactory
                    .WithClientId(clientId)
                    .WithClientSecret(invalidClientSecret)
                    .Create(server))
                {
                    await client.GetToken();
                    Assert.True(eventCapture.ContainsMessage("Invalid client secret"));
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
                                AllowedScopes = new List<string> { apiResourceName },
                            },
                        })
                        .AddInMemoryApiResources(new List<ApiResource>
                        {
                            new ApiResource(apiResourceName, apiResourceDisplayName),
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

        [Theory]
        [AutoData]
        public async Task CreateTokenReturnsToken(
            TestIdentityServer4TestServerFactory serverFactory,
            int lifetime)
        {
            using (var server = serverFactory.Create())
            {
                var tokenFactory = server.CreateTokenFactory();

                var tokenResult = await tokenFactory.CreateToken(
                    lifetime: lifetime,
                    claims: new List<Claim>());

                Assert.NotNull(tokenResult.Token);
            }
        }

        [Theory]
        [AutoData]
        public async Task CreateTokenWithClaimsReturnsToken(
            TestIdentityServer4TestServerFactory serverFactory,
            int lifetime,
            string claimType,
            string claimValue)
        {
            using (var server = serverFactory.Create())
            {
                var tokenFactory = server.CreateTokenFactory();

                var tokenResult = await tokenFactory.CreateToken(
                    lifetime: lifetime,
                    claims: new List<Claim>
                    {
                        new Claim(claimType, claimValue),
                    });

                Assert.NotNull(tokenResult.Token);
            }
        }

        [Theory]
        [AutoData]
        public async Task CreateClientTokenReturnsToken(
            TestIdentityServer4TestServerFactory serverFactory,
            int lifetime,
            string clientId,
            IEnumerable<string> scopes,
            IEnumerable<string> audiences)
        {
            using (var server = serverFactory.Create())
            {
                var tokenFactory = server.CreateTokenFactory();

                var tokenResult = await tokenFactory.CreateClientToken(
                    lifetime: lifetime,
                    clientId: clientId,
                    scopes: scopes,
                    audiences: audiences);

                Assert.NotNull(tokenResult.Token);
            }
        }

        [Theory]
        [AutoData]
        public async Task GetDiscoveryDocReturnsDoc(
            TestIdentityServer4TestServerFactory serverFactory,
            TestIdentityServer4TestClientFactory clientFactory)
        {
            using (var server = serverFactory.Create())
            {
                using (var client = clientFactory.Create(server))
                {
                    var disco = await client.GetDiscovery();
                    Assert.NotNull(disco.TokenEndpoint);
                }
            }
        }

        [Theory]
        [AutoData]
        public async Task RequestClientCredentialsReturnsAcccessToken(
            TestIdentityServer4TestServerFactory serverFactory,
            TestIdentityServer4TestClientFactory clientFactory,
            string clientId,
            string clientSecret,
            string apiResourceName,
            string apiResourceDisplayName)
        {
            using (var server = serverFactory
                .WithLogging(new XUnitLoggerFactory(this.output))
                .WithApiResource(new ApiResource(apiResourceName, apiResourceDisplayName))
                .WithClient(new Client
            {
                ClientId = clientId,
                ClientSecrets = new List<Secret> { new Secret(clientSecret.Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new List<string> { apiResourceName },
            }).Create())
            {
                using (var client = clientFactory
                    .WithClientId(clientId)
                    .WithClientSecret(clientSecret)
                    .Create(server))
                {
                    using (var tokenClient = await client.CreateTokenClient())
                    {
                        var response = await tokenClient.RequestClientCredentialsAsync();
                        Assert.NotNull(response.AccessToken);
                    }
                }
            }
        }

        [Theory]
        [AutoData]
        public async Task CreateTokenFactoryReturnsToken(
            TestIdentityServer4TestServerFactory serverFactory,
            TestIdentityServer4TestClientFactory clientFactory,
            string clientId,
            string clientSecret,
            string apiResourceName,
            string apiResourceDisplayName,
            int lifetime)
        {
            using (var server = serverFactory
                .WithLogging(new XUnitLoggerFactory(this.output))
                .WithApiResource(new ApiResource(apiResourceName, apiResourceDisplayName))
                .WithClient(new Client
                {
                    ClientId = clientId,
                    ClientSecrets = new List<Secret> { new Secret(clientSecret.Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string> { apiResourceName },
                }).Create())
            {
                var tokenFactory = server.CreateTokenFactory();
                var tokenResult = await tokenFactory.CreateToken(
                    lifetime: lifetime,
                    claims: new List<Claim>());

                Assert.NotNull(tokenResult.Token);
            }
        }
    }
}