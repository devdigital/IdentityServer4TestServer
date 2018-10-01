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
    using (var server = await serverFactory
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

## Testing Production Setup

The `IdentityServer4TestServerFactory<T>` type provides builder methods `WithClient(Client client)`, `WithIdentityResource(IdentityResource identityResource)` and `WithApiResource(ApiResource apiResource)` to configure Identity Server clients and resources.

However, you will also want to run tests against your production Identity Server setup, i.e. the clients/resources etc. that you run in production.

The `WithWebHostBuilder` method takes an ASP.NET Core `IWebHostBuilder` and uses the supplied web host builder to configure the test server. This means you can pass the instance of `IWebHostBuilder` that you use in your production bootstrapping. For example:

```csharp
// Program.cs
public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
}

[Theory]
[AutoData]
public async Task Test(MyServerFactory serverFactory)
{
    using (var server = await serverFactory
        .WithWebHostBuilder(Program.CreateWebHostBuilder(new string[] {})
        .Create())
    {
        ...
    }
}
```

Rather than use the `WithWebHostBuilder` method within every test, you can override the `Create` method of the test server factory, and configure the factory there:

```csharp
public class MyServerFactory : IdentityServer4TestServerFactory<MyServerFactory>
{
    public override async Task<IIdentityServer> Create()
    {
       this.WithWebHostBuilder(...);

       return await base.Create();
    }
}
```

Then the test becomes:

```csharp
[Theory]
[AutoData]
public async Task Test(MyServerFactory serverFactory)
{
    using (var server = await serverFactory.Create())
    {
        ...
    }
}
```
