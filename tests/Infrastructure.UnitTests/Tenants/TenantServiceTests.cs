using Application.Common.Tenants;
using Infrastructure.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace Infrastructure.UnitTests.Tenants;

public class TenantServiceTests
{
    public TenantSettings TenantSettings = new()
    {
        Defaults = new Configuration
        {
            ConnectionString = "DefaultConnectionString",
            DbProvider = "DefaultDbProvider"
        },
        Tenants = new List<Tenant>
        {
            new()
            {
                Name = "TenantName1", TenantId = "Tenant1", ConnectionString = "Connection1", DbProvider = "mssql"
            },
            new()
            {
                Name = "TenantName2", TenantId = "Tenant2", ConnectionString = "Connection2", DbProvider = "mysql"
            },
            new() { Name = "TenantName3", TenantId = "Tenant3" }
        }
    };

    [Theory]
    [InlineData("Tenant1", "Connection1", "mssql")]
    [InlineData("Tenant2", "Connection2", "mysql")]
    [InlineData("Tenant3", "DefaultConnectionString", "DefaultDbProvider")]
    public void Constructor_WithValidTenantHeader_SetsCurrentTenantSuccessfully(string tenantHeader,
        string connectionString, string provider)
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["tenant"] = tenantHeader;

        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        contextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var optionsMock = new Mock<IOptions<TenantSettings>>();
        optionsMock.Setup(x => x.Value).Returns(TenantSettings);

        // Act
        var tenantService = new TenantService(contextAccessorMock.Object, optionsMock.Object);

        // Assert
        tenantService.GetTenant().Should().NotBeNull();
        tenantService.GetConnectionString().Should().Be(connectionString);
        tenantService.GetDatabaseProvider().Should().Be(provider);
    }

    [Fact]
    public void Constructor_WithNotValidTenantHeader_ThrowsException()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["tenant"] = "UNVALID";

        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        contextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var optionsMock = new Mock<IOptions<TenantSettings>>();
        optionsMock.Setup(x => x.Value).Returns(TenantSettings);

        // Act
        Action act = () => new TenantService(contextAccessorMock.Object, optionsMock.Object);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithNoTenantHeader_ThrowsException()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        contextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var optionsMock = new Mock<IOptions<TenantSettings>>();
        optionsMock.Setup(x => x.Value).Returns(TenantSettings);

        // Act
        Action act = () => new TenantService(contextAccessorMock.Object, optionsMock.Object);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}