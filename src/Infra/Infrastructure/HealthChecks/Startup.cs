using Application.Common.Tenants;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthChecks;

internal static class Startup
{
    internal static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var tenantSettings = configuration.GetSection(nameof(TenantSettings)).Get<TenantSettings>();

        services.AddHealthChecksUI().AddInMemoryStorage();

        var healthBuilder = services.AddHealthChecks();

        healthBuilder.AddCheck("Api Health Check", () => HealthCheckResult.Healthy(), new List<string> { "app" });
        foreach (var tenant in tenantSettings.Tenants)
        {
            var connectionString = !string.IsNullOrWhiteSpace(tenant.ConnectionString)
                ? tenant.ConnectionString
                : tenantSettings.Defaults.ConnectionString;
            healthBuilder.AddSqlServer(connectionString,
                tags: new List<string> { "database", "rational", "ado" },
                name: $"Tenant Db: {tenant.Name} Health Check");
        }

        healthBuilder.AddDbContextCheck<ApplicationDbContext>("Default DbContext Health Check", null,
            new List<string> { "database", "rational", "orm" });

        return services;
    }
}