using System;
using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer4TestServer
{
    public abstract class IdentityServer4TestServerFactory<TServerFactory>
        where TServerFactory : IdentityServer4TestServerFactory<TServerFactory>
    {
        private readonly List<Client> clients;

        private readonly List<IdentityResource> identityResources;

        private readonly List<ApiResource> apiResources;

        private Action<IApplicationBuilder> configureApp;

        private Action<WebHostBuilderContext, IServiceCollection> configureServices;

        private ILoggerFactory loggerFactory;
        
        protected IdentityServer4TestServerFactory()
        {
            this.clients = new List<Client>();
            this.identityResources = new List<IdentityResource>();
            this.apiResources = new List<ApiResource>();
        }

        public TServerFactory WithLogging(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            return this as TServerFactory;
        }

        public TServerFactory WithClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            return this.WithClients(new List<Client> { client });
        }

        public TServerFactory WithClients(IEnumerable<Client> clients)
        {
            if (clients == null)
            {
                throw new ArgumentNullException(nameof(clients));
            }

            this.clients.AddRange(clients);
            return this as TServerFactory;
        }

        public TServerFactory WithIdentityResource(IdentityResource identityResource)
        {
            if (identityResource == null)
            {
                throw new ArgumentNullException(nameof(identityResource));
            }

            return this.WithIdentityResources(new List<IdentityResource> { identityResource });
        }

        public TServerFactory WithIdentityResources(IEnumerable<IdentityResource> identityResources)
        {
            if (identityResources == null)
            {
                throw new ArgumentNullException(nameof(identityResources));
            }

            this.identityResources.AddRange(identityResources);
            return this as TServerFactory;
        }

        public TServerFactory WithApiResource(ApiResource apiResource)
        {
            if (apiResource == null)
            {
                throw new ArgumentNullException(nameof(apiResource));
            }

            return this.WithApiResources(new List<ApiResource> { apiResource });
        }

        public TServerFactory WithApiResources(IEnumerable<ApiResource> apiResources)
        {
            if (apiResources == null)
            {
                throw new ArgumentNullException(nameof(apiResources));
            }

            this.apiResources.AddRange(apiResources);
            return this as TServerFactory;
        }

        public TServerFactory WithConfigureApp(Action<IApplicationBuilder> app)
        {
            this.configureApp = app ?? throw new ArgumentNullException(nameof(app));
            return this as TServerFactory;
        }

        public TServerFactory WithConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
        {
            this.configureServices = configureServices ?? throw new ArgumentNullException(nameof(configureServices));
            return this as TServerFactory;
        }

        public virtual IIdentityServer Create()
        {
            var hostBuilder = new WebHostBuilder()
                .Configure(configureApp ?? DefaultConfigureApp)
                .ConfigureServices(configureServices ?? DefaultConfigureServices);

            if (this.loggerFactory != null)
            {
                hostBuilder = hostBuilder.ConfigureLogging(loggingBuilder =>
                    loggingBuilder.AddProvider(new DefaultLoggingProvider(this.loggerFactory)));
            }

            var testServer = new TestServer(hostBuilder);
            return new IdentityServer(testServer);
        }

        private void DefaultConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            var identityServerConfig = services.AddIdentityServer();

            if (this.clients != null)
            {
                identityServerConfig.AddInMemoryClients(this.clients);
            }

            if (this.identityResources != null)
            {
                identityServerConfig.AddInMemoryIdentityResources(this.identityResources);
            }

            if (this.apiResources != null)
            {
                identityServerConfig.AddInMemoryApiResources(this.apiResources);
            }

            identityServerConfig
                .AddDefaultEndpoints()
                .AddDefaultSecretParsers()
                .AddDefaultSecretValidators()
                .AddDeveloperSigningCredential();
        }

        private void DefaultConfigureApp(IApplicationBuilder app)
        {
            app.UseIdentityServer();            
        }
    }
}
