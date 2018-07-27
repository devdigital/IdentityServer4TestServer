// <copyright file="ServerConfigurationShould.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.IntegrationTests.Tests
{
    using AutoFixture.Xunit2;
    using IdentityServer4TestServer.Factories;
    using IdentityServer4TestServer.IntegrationTests.Helpers;
    using Moq;
    using Xunit;

    // ReSharper disable StyleCop.SA1600
    #pragma warning disable SA1600
    #pragma warning disable 1591

    public class ServerConfigurationShould
    {
        [Theory]
        [AutoData]
        public void InvokeConfigureMethod(
            TestIdentityServer4TestServerFactory serverFactory,
            Mock<IServerFactoryConfiguration<TestIdentityServer4TestServerFactory>> configuration)
        {
            using (serverFactory
                .WithConfiguration(configuration.Object)
                .Create())
            {
                configuration.Verify(c => c.Configure(It.Is<TestIdentityServer4TestServerFactory>(val => val == serverFactory)));
            }
        }
    }
}