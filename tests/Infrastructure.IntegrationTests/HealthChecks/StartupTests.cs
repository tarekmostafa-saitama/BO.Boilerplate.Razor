using Newtonsoft.Json.Linq;

namespace Infrastructure.IntegrationTests.HealthChecks;

public class StartupTests : IClassFixture<BaseWebHostFixture<Program>>
{
    private readonly BaseWebHostFixture<Program> _baseWebHost;

    public StartupTests(BaseWebHostFixture<Program> baseWebHost)
    {
        _baseWebHost = baseWebHost;
    }

    [Fact]
    public async Task Startup_HitHealthPath_ReturnJsonAndAllHealthyAsync()
    {
        var httpResponse = await _baseWebHost.Client.GetAsync("health");
        httpResponse.EnsureSuccessStatusCode();
        var jsonResponse = await httpResponse.Content.ReadAsStringAsync();

        // Parse the JSON string
        var healthData = JObject.Parse(jsonResponse);

        // Assert the overall status is "Healthy"
        Assert.Equal("Healthy", (string)healthData["status"]);

        // Assert the presence of the "entries" object
        Assert.Contains("entries", healthData);

        // Get the children of the "entries" object
        var entries = healthData["entries"].Children();

        // Iterate through the children and validate their "status" property
        foreach (var entry in entries) Assert.Equal("Healthy", (string)entry.First["status"]);
    }
}