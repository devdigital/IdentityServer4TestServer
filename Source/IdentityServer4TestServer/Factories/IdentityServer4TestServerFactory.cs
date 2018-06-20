// <copyright file="IdentityServer4TestServerFactory.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Factories
{
    using System;
    using System.Collections.Generic;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using IdentityServer4TestServer.Token;
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
        private readonly List<Client> currentClients;

        private readonly List<IdentityResource> currentIdentityResources;

        private readonly List<ApiResource> currentApiResources;

        private Action<WebHostBuilderContext, IConfigurationBuilder> currentConfiguration;

        private Action<IApplicationBuilder> currentConfigureApp;

        private Action<WebHostBuilderContext, IServiceCollection> currentConfigureServices;

        private ILoggerFactory currentLoggerFactory;

        private IdentityServerEventCapture currentEventCapture;

        private bool enableTokenCreation;

        private IWebHostBuilder currentWebHostBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServer4TestServerFactory{TServerFactory}"/> class.
        /// </summary>
        protected IdentityServer4TestServerFactory()
        {
            this.currentClients = new List<Client>();
            this.currentIdentityResources = new List<IdentityResource>();
            this.currentApiResources = new List<ApiResource>();
            this.enableTokenCreation = true;
        }

        /// <summary>
        /// Adds logging.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithLogging(ILoggerFactory loggerFactory)
        {
            this.currentLoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
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

            this.currentClients.AddRange(clients);
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

            this.currentIdentityResources.AddRange(identityResources);
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

            this.currentApiResources.AddRange(apiResources);
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configuration)
        {
            this.currentConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds token creation.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> enable token creation.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithTokenCreation(bool enable)
        {
            this.enableTokenCreation = enable;
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds app configuration.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithConfigureApp(Action<IApplicationBuilder> app)
        {
            this.currentConfigureApp = app ?? throw new ArgumentNullException(nameof(app));
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds configure services.
        /// </summary>
        /// <param name="configureServices">The configure services.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
        {
            this.currentConfigureServices = configureServices ?? throw new ArgumentNullException(nameof(configureServices));
            return this as TServerFactory;
        }

        /// <summary>
        /// Sets the web host builder.
        /// </summary>
        /// <param name="webHostBuilder">The web host builder.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithWebHostBuilder(IWebHostBuilder webHostBuilder)
        {
            this.currentWebHostBuilder = webHostBuilder ?? throw new ArgumentNullException(nameof(webHostBuilder));
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds event capture.
        /// </summary>
        /// <param name="eventCapture">The event capture.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithEventCapture(IdentityServerEventCapture eventCapture)
        {
            this.currentEventCapture = eventCapture ?? throw new ArgumentNullException(nameof(eventCapture));
            return this as TServerFactory;
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>The identity server.</returns>
        public virtual IIdentityServer Create()
        {
            var hostBuilder = this.GetWebHostBuilder();

            if (this.currentLoggerFactory != null)
            {
                hostBuilder = hostBuilder.ConfigureLogging(loggingBuilder =>
                    loggingBuilder.AddProvider(new DefaultLoggingProvider(this.currentLoggerFactory)));
            }

            var testServer = new TestServer(hostBuilder);
            return new IdentityServer(testServer);
        }

        private IWebHostBuilder GetWebHostBuilder()
        {
            if (this.currentWebHostBuilder != null)
            {
                return this.currentWebHostBuilder;
            }

            return new WebHostBuilder()
                .ConfigureAppConfiguration(this.currentConfiguration ?? this.DefaultConfiguration)
                .Configure(app =>
                {
                    if (this.enableTokenCreation)
                    {
                        app.UseTokenCreation();
                    }

                    var configure = this.currentConfigureApp ?? this.DefaultConfigureApp;
                    configure(app);
                })
                .ConfigureServices((context, services) =>
                {
                    if (this.currentEventCapture != null)
                    {
                        services.AddSingleton<IEventSink>(new EventCaptureEventSink(this.currentEventCapture));
                    }

                    var configure = this.currentConfigureServices ?? this.DefaultConfigureServices;
                    configure(context, services);
                });
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

            if (this.currentClients != null)
            {
                identityServerConfig.AddInMemoryClients(this.currentClients);
            }

            if (this.currentIdentityResources != null)
            {
                identityServerConfig.AddInMemoryIdentityResources(this.currentIdentityResources);
            }

            if (this.currentApiResources != null)
            {
                identityServerConfig.AddInMemoryApiResources(this.currentApiResources);
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
