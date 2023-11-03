using Domain.Entities;
using Domain.Enums;
using Shared.ServiceContracts;

namespace Application.Common.Tenants;

public interface ITenantService : IScopedService
{
    public TenantDbProvider GetDatabaseProvider();

    public string GetConnectionString();

    public Tenant GetTenant();
}