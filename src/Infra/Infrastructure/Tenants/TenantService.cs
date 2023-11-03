using Application.Common.Tenants;
using Application.Repositories;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Tenants
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private Tenant _currentTenant;

        public TenantService(IHttpContextAccessor contextAccessor, IOptions<TenantSettings> tenantSettings)
        {
            _tenantSettings = tenantSettings.Value;

            // Initialize the current tenant based on the HttpContext or use the default tenant if not found.
            SetCurrentTenant(contextAccessor.HttpContext != null && contextAccessor.HttpContext.Request.Cookies.TryGetValue("tenant", out var tenantId)
                ? Guid.Parse(tenantId)
                : Guid.Empty);
        }

        public string GetConnectionString()
        {
            // Get the connection string based on the current tenant or the default if using shared DB.
            return _currentTenant is null || _currentTenant.UseSharedDb
                ? _tenantSettings.Default.ConnectionString
                : _currentTenant.ConnectionString;
        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }

        public TenantDbProvider GetDatabaseProvider()
        {
            // Get the database provider based on the current tenant or the default if using shared DB.
            return _currentTenant.UseSharedDb
                ? _tenantSettings.Default.DbProvider
                : _currentTenant.DbProvider;
        }

        private void SetCurrentTenant(Guid tenantId)
        {
            if (tenantId != Guid.Empty)
            {
                if (tenantId == _tenantSettings.Default.Id)
                {
                    // Use the default tenant settings if the tenant ID matches the default.
                    _currentTenant = new Tenant()
                    {
                        ConnectionString = _tenantSettings.Default.ConnectionString,
                        DbProvider = _tenantSettings.Default.DbProvider,
                        Id = _tenantSettings.Default.Id,
                        Name = _tenantSettings.Default.Name,
                        UseSharedDb = true
                    };
                }
                else
                {
                    IDbConnection dbConnection = new SqlConnection(_tenantSettings.Default.ConnectionString);

                    // Query the list of tenants from the database.
                    _tenantSettings.Tenants = dbConnection.Query<Tenant>("SELECT * FROM Tenants").AsList();

                    // Find the current tenant by ID.
                    _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.Id == tenantId);

                    if (_currentTenant is null)
                    {
                        throw new ArgumentException("Invalid tenant ID");
                    }
                }
            }
            else
            {
                // Use the default tenant settings if no tenant ID is provided.
                _currentTenant = new Tenant()
                {
                    ConnectionString = _tenantSettings.Default.ConnectionString,
                    DbProvider = _tenantSettings.Default.DbProvider,
                    Id = _tenantSettings.Default.Id,
                    Name = _tenantSettings.Default.Name,
                    UseSharedDb = true
                };
            }

            if (_currentTenant.UseSharedDb)
            {
                _currentTenant.ConnectionString = _tenantSettings.Default.ConnectionString;
            }
        }
    }
}
