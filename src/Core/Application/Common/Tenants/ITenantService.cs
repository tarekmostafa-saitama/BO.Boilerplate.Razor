using Shared.ServiceContracts;

namespace Application.Common.Tenants;

public interface ITenantService : IScopedService
{
    public string GetDatabaseProvider();

    public string GetConnectionString();

    public Tenant GetTenant();
}