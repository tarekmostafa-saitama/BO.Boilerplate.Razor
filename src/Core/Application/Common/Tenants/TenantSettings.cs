namespace Application.Common.Tenants;

public class TenantSettings
{
    public Configuration Defaults { get; set; }
    public List<Tenant> Tenants { get; set; }
}

public class Tenant
{
    public string Name { get; set; }
    public string TenantId { get; set; }
    public string ConnectionString { get; set; }
    public string DbProvider { get; set; }
}

public class Configuration
{
    public string DbProvider { get; set; }
    public string ConnectionString { get; set; }
}