using Application.Common.Tenants;
using Application.Repositories;
using Domain.Enums;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Persistence;

/// <summary>
///  WE Have Limited the Tenants to Shared Db Type Only !!!
/// </summary>
public static class Startup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TenantSettings>(configuration.GetSection(nameof(TenantSettings)));

        var tenantSettings = configuration.GetSection(nameof(TenantSettings)).Get<TenantSettings>();



       

        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();


        //var tenants = tenantSettings.Tenants.Append(tenantSettings.Default);

        //foreach (var tenant in tenants)
        //{

        //    var dbProvider = tenant.DbProvider;

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {

                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                switch (tenantSettings.Default.DbProvider)
                {
                    case TenantDbProvider.MSSQL:
                        options.UseSqlServer(e => e.MigrationsAssembly("MSSQL.Migrator"));
                        break;
                    case TenantDbProvider.MYSQL:
                        options.UseMySQL(e => e.MigrationsAssembly("MySQL.Migrator"));
                        break;
                    case TenantDbProvider.POSTGRES:
                        options.UseNpgsql(e => e.MigrationsAssembly("Postgres.Migrator"));
                        break;
                }
            });

            // Auto Migrate
            using var scope = services.BuildServiceProvider().CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.SetConnectionString(tenantSettings.Default.ConnectionString);
            if (dbContext.Database.IsRelational() && dbContext.Database.GetMigrations().Any())
                dbContext.Database.Migrate();
        //}

        services.AddScoped<ApplicationDbContextInitialiser>();

        return services;
    }
}