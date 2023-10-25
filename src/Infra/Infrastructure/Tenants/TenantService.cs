using Application.Common.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Tenants;

public class TenantService : ITenantService
{
    private readonly HttpContext _httpContext;
    private readonly TenantSettings _tenantSettings;
    private Tenant _currentTenant;

    public TenantService(IHttpContextAccessor contextAccessor, IOptions<TenantSettings> tenantSettings)
    {
        _httpContext = contextAccessor.HttpContext;
        _tenantSettings = tenantSettings.Value;

        if (_httpContext is not null)
        {
            if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                SetCurrentTenant(tenantId!);
            else
                // throw new ArgumentException("No tenant provided!");
                // use default tenant
                SetCurrentTenant("Default");
        }
    }

    public string GetConnectionString()
    {
        var currentConnectionString = _currentTenant is null || string.IsNullOrEmpty(_currentTenant.ConnectionString)
            ? _tenantSettings.Defaults.ConnectionString
            : _currentTenant.ConnectionString;

        return currentConnectionString;
    }


    public Tenant GetTenant()
    {
        return _currentTenant;
    }

    public string GetDatabaseProvider()
    {
        return _currentTenant is null || string.IsNullOrEmpty(_currentTenant.DbProvider)
            ? _tenantSettings.Defaults.DbProvider
            : _currentTenant.DbProvider;
    }


    private void SetCurrentTenant(string tenantId)
    {
        _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.TenantId == tenantId);

        if (_currentTenant is null) throw new ArgumentException("Invalid tenant ID");

        if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
    }
}