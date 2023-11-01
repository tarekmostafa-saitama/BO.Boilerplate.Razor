using System.Security.AccessControl;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Tenants;

public class TenantSettings
{
    public Tenant Default { get; set; }
    public List<Tenant> Tenants { get; set; } = new();
}



public class Configuration
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public TenantDbProvider DbProvider { get; set; }
    public string ConnectionString { get; set; }
}