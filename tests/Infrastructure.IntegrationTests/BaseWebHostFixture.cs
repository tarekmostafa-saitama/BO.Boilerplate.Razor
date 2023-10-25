using Microsoft.AspNetCore.Mvc.Testing;

namespace Infrastructure.IntegrationTests;

public class BaseWebHostFixture<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public BaseWebHostFixture()
    {
        var applicationFactory = new WebApplicationFactory<TStartup>();
        Client = applicationFactory.CreateClient();
        Client.BaseAddress = new Uri("http://localhost:7227");
    }

    public HttpClient Client { get; }
}