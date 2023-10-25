using FluentAssertions;

namespace Infrastructure.IntegrationTests.Middlewares;

public class AttachDefaultTenantToHealthChecksMiddlewareTests : IClassFixture<BaseWebHostFixture<Program>>
{
    private readonly BaseWebHostFixture<Program> _baseWebHostFixture;

    public AttachDefaultTenantToHealthChecksMiddlewareTests(BaseWebHostFixture<Program> baseWebHostFixture)
    {
        _baseWebHostFixture = baseWebHostFixture;
    }

    [Fact]
    public async Task InvokeAsync_HitHealthPoint_ShouldAddDefaultTenantHeader()
    {
        // Arrange

        // Act
        var result = await _baseWebHostFixture.Client.GetAsync("health");

        // Assert
        result.RequestMessage.Headers.TryGetValues("tenant", out var values).Should().BeTrue();
    }
}