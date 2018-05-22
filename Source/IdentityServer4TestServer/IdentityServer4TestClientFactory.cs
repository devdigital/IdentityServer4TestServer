using System;

namespace IdentityServer4TestServer
{
    public abstract class IdentityServer4TestClientFactory<TClientFactory>
        where TClientFactory : IdentityServer4TestClientFactory<TClientFactory>
    {
        private string currentClientId;

        private string currentClientSecret;

        public TClientFactory WithClientId(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            this.currentClientId = clientId;
            return this as TClientFactory;
        }

        public TClientFactory WithClientSecret(string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            this.currentClientSecret = clientSecret;
            return this as TClientFactory;
        }

        public TClientFactory WithIdentityServer(IIdentityServer identityServer)
        {
            if (identityServer == null)
            {
                throw new ArgumentNullException(nameof(identityServer));
            }

            return this as TClientFactory;
        }

        public virtual IdentityServerClient Create(IIdentityServer server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            return new IdentityServerClient(
                server: server,
                clientId: this.currentClientId,
                clientSecret: this.currentClientSecret);
        }
    }
}