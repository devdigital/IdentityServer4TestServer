# IdentityServer4TestServer

Identity Server 4 testing helper for .NET Core.

## Getting Started

Create a test project and install the `IdentityServer4TestServer` package:

```
install-package IdentityServer4TestServer
```

Create a server and client factory deriving from the supplied types:

```csharp
public class MyServerFactory : IdentityServer4TestServerFactory<MyServerFactory>
{
}

public class MyClientFactory : IdentityServer4TestClientFactory<MyClientFactory>
{
}
```

Use the server and client factories in your tests:

```csharp
[Theory]
[AutoData]
public async Task GetTokenValidClientReturns200(
    MyServerFactory serverFactory,
    MyClientFactory clientFactory,
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
```

> Note in this example we are using AutoFixture to allow the factories to be injected into the test methods.
