// <copyright file="IdentityServer4TestServerFactory.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;
    using System.Collections.Generic;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Identity server test server factory.
    /// </summary>
    /// <typeparam name="TServerFactory">The type of the server factory.</typeparam>
    public abstract class IdentityServer4TestServerFactory<TServerFactory>
        where TServerFactory : IdentityServer4TestServerFactory<TServerFactory>
    {
        private readonly List<Client> clients;

        private readonly List<IdentityResource> identityResources;

        private readonly List<ApiResource> apiResources;

        private Action<WebHostBuilderContext, IConfigurationBuilder> configuration;

        private Action<IApplicationBuilder> configureApp;

        private Action<WebHostBuilderContext, IServiceCollection> configureServices;

        private ILoggerFactory loggerFactory;

        private IdentityServerEventCapture eventCapture;

        private bool enableTokenTools;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServer4TestServerFactory{TServerFactory}"/> class.
        /// </summary>
        protected IdentityServer4TestServerFactory()
        {
            this.clients = new List<Client>();
            this.identityResources = new List<IdentityResource>();
            this.apiResources = new List<ApiResource>();
            this.enableTokenTools = true;
        }

        /// <summary>
        /// Adds logging.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithLogging(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds a client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            return this.WithClients(new List<Client> { client });
        }

        /// <summary>
        /// Adds clients.
        /// </summary>
        /// <param name="clients">The clients.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithClients(IEnumerable<Client> clients)
        {
            if (clients == null)
            {
                throw new ArgumentNullException(nameof(clients));
            }

            this.clients.AddRange(clients);
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds the identity resource.
        /// </summary>
        /// <param name="identityResource">The identity resource.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithIdentityResource(IdentityResource identityResource)
        {
            if (identityResource == null)
            {
                throw new ArgumentNullException(nameof(identityResource));
            }

            return this.WithIdentityResources(new List<IdentityResource> { identityResource });
        }

        /// <summary>
        /// Adds identity resources.
        /// </summary>
        /// <param name="identityResources">The identity resources.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithIdentityResources(IEnumerable<IdentityResource> identityResources)
        {
            if (identityResources == null)
            {
                throw new ArgumentNullException(nameof(identityResources));
            }

            this.identityResources.AddRange(identityResources);
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds an API resource.
        /// </summary>
        /// <param name="apiResource">The API resource.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithApiResource(ApiResource apiResource)
        {
            if (apiResource == null)
            {
                throw new ArgumentNullException(nameof(apiResource));
            }

            return this.WithApiResources(new List<ApiResource> { apiResource });
        }

        /// <summary>
        /// Adds API resources.
        /// </summary>
        /// <param name="apiResources">The API resources.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithApiResources(IEnumerable<ApiResource> apiResources)
        {
            if (apiResources == null)
            {
                throw new ArgumentNullException(nameof(apiResources));
            }

            this.apiResources.AddRange(apiResources);
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds token tools via MVC.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> enable MVC.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithTokenTools(bool enable)
        {
            this.enableTokenTools = enable;
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds app configuration.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithConfigureApp(Action<IApplicationBuilder> app)
        {
            this.configureApp = app ?? throw new ArgumentNullException(nameof(app));
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds configure services.
        /// </summary>
        /// <param name="configureServices">The configure services.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
        {
            this.configureServices = configureServices ?? throw new ArgumentNullException(nameof(configureServices));
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds event capture.
        /// </summary>
        /// <param name="eventCapture">The event capture.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithEventCapture(IdentityServerEventCapture eventCapture)
        {
            this.eventCapture = eventCapture ?? throw new ArgumentNullException(nameof(eventCapture));
            return this as TServerFactory;
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>The identity server.</returns>
        public virtual IIdentityServer Create()
        {
            var hostBuilder = new WebHostBuilder()
                .ConfigureAppConfiguration(this.configuration ?? this.DefaultConfiguration)
                .Configure(app =>
                {
                    var configure = this.configureApp ?? this.DefaultConfigureApp;
                    configure(app);

                    if (this.enableTokenTools)
                    {
                        app.UseMvc();
                    }
                })
                .ConfigureServices((context, services) =>
                {
                    if (this.enableTokenTools)
                    {
                        services.AddMvc().AddApplicationPart(
                            typeof(TokenController).Assembly);
                    }

                    if (this.eventCapture != null)
                    {
                        services.AddSingleton<IEventSink>(new EventCaptureEventSink(this.eventCapture));
                    }

                    var configure = this.configureServices ?? this.DefaultConfigureServices;
                    configure(context, services);
                });

            if (this.loggerFactory != null)
            {
                hostBuilder = hostBuilder.ConfigureLogging(loggingBuilder =>
                    loggingBuilder.AddProvider(new DefaultLoggingProvider(this.loggerFactory)));
            }

            var testServer = new TestServer(hostBuilder);
            return new IdentityServer(testServer);
        }

        private void DefaultConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
        }

        private void DefaultConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            var identityServerConfig = services.AddIdentityServer(options =>
            {
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
            });

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
