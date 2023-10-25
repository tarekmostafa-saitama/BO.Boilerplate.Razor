using Application.Common.Tenants;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class Startup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TenantSettings>(configuration.GetSection(nameof(TenantSettings)));

        var tenantSettings = configuration.GetSection(nameof(TenantSettings)).Get<TenantSettings>();

        var defaultConnectionString = tenantSettings.Defaults.ConnectionString;
        var defaultDbProvider = tenantSettings.Defaults.DbProvider;

        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();


        var tenants = tenantSettings.Tenants;

        foreach (var tenant in tenants)
        {
            var dbProvider = string.IsNullOrWhiteSpace(tenant.DbProvider) ? defaultDbProvider : tenant.DbProvider;


            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                if (dbProvider.ToLower() == "mssql")
                    options.UseSqlServer(e => e.MigrationsAssembly("MSSQL.Migrator"));

                else if (dbProvider.ToLower() == "mysql")
                    options.UseMySQL(e => e.MigrationsAssembly("MySQL.Migrator"));
                else if (dbProvider.ToLower() == "postgresql")
                    options.UseNpgsql(e => e.MigrationsAssembly("Postgres.Migrator"));
                // ...
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.SetConnectionString(string.IsNullOrWhiteSpace(tenant.ConnectionString)
                ? defaultConnectionString
                : tenant.ConnectionString);
            if (dbContext.Database.IsRelational() && dbContext.Database.GetMigrations().Any())
                dbContext.Database.Migrate();
        }

        services.AddScoped<ApplicationDbContextInitialiser>();

        return services;
    }
}