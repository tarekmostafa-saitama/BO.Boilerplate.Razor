using Application.Common.Interfaces;
using Application.Common.Tenants;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ISerializerService serializerService,
        ITenantService tenantService,
        ICurrentUserService currentUserService) : base(options, serializerService, currentUserService, tenantService)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}