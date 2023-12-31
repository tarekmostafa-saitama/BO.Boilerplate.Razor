﻿using Application.Common.Interfaces;
using Application.Common.Tenants;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Identity;
using Infrastructure.Trails;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Contracts;

namespace Infrastructure.Persistence.Context;

public abstract class BaseDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ISerializerService _serializerService;
    private readonly ITenantService _tenantService;

    protected BaseDbContext(DbContextOptions<ApplicationDbContext> options, ISerializerService serializerService,
        ICurrentUserService currentUserService, ITenantService tenantService) : base(options)
    {
        _serializerService = serializerService;
        _currentUserService = currentUserService;
        _tenantService = tenantService;
    }

    public DbSet<Trail> Trails { get; set; }
    public DbSet<Tenant> Tenants { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // QueryFilters need to be applied before base.OnModelCreating
        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);
        modelBuilder.AppendGlobalQueryFilter<IMustHaveTenant>(s => s.TenantId == _tenantService.GetTenant().Id);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ChangeTracker.DetectChanges();
        try
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            AttachTenantToEntities();

            var auditEntries = HandleAuditingBeforeSaveChanges(_currentUserService.Id);
            var result = await base.SaveChangesAsync(cancellationToken);
            await HandleAuditingAfterSaveChangesAsync(auditEntries, cancellationToken);
            return result;

        }
        finally
        {
            ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }

    private void AttachTenantToEntities()
    {
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    entry.Entity.TenantId = _tenantService.GetTenant().Id;
                    break;
            }
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenantConnectionString = _tenantService.GetConnectionString();
        if (!string.IsNullOrEmpty(tenantConnectionString))
        {

            var dbProvider = _tenantService.GetDatabaseProvider();

            switch (dbProvider)
            {
                case TenantDbProvider.MSSQL:
                    optionsBuilder.UseSqlServer(tenantConnectionString);
                    break;
                case TenantDbProvider.MYSQL:
                    optionsBuilder.UseMySQL(tenantConnectionString);
                    break;
                case TenantDbProvider.POSTGRES:
                    optionsBuilder.UseNpgsql(tenantConnectionString);
                    break;
            }
        }

        base.OnConfiguring(optionsBuilder);
    }


    private List<AuditTrail> HandleAuditingBeforeSaveChanges(string userId)
    {
        foreach (var entry in ChangeTracker.Entries<IBaseAuditableEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedOn = DateTimeOffset.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTimeOffset.UtcNow;
                    entry.Entity.LastModifiedBy = userId;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedBy = userId;
                        softDelete.DeletedOn = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
                    }

                    break;
            }



        var trailEntries = new List<AuditTrail>();
        foreach (var entry in ChangeTracker.Entries<IBaseAuditableEntity>())
        //.Where(e => e.Entity.GetType().IsGenericType &&
        //            e.Entity.GetType().GetGenericTypeDefinition() == typeof(IBaseAuditableEntity))
        //.Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
        //.ToList())
        {
            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;
            var trailEntry = new AuditTrail(entry, _serializerService)
            {
                TableName = entry.Entity.GetType().Name,
                UserId = userId
            };
            trailEntries.Add(trailEntry);
            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    trailEntry.TemporaryProperties.Add(property);
                    continue;
                }

                var propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    trailEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        trailEntry.TrailType = TrailType.Create;
                        trailEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        trailEntry.TrailType = TrailType.Delete;
                        trailEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified && entry.Entity is ISoftDelete && property.OriginalValue == null &&
                            property.CurrentValue != null)
                        {
                            trailEntry.ChangedColumns.Add(propertyName);
                            trailEntry.TrailType = TrailType.Delete;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                        {
                            trailEntry.ChangedColumns.Add(propertyName);
                            trailEntry.TrailType = TrailType.Update;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                        }

                        break;
                }
            }
        }

        foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
            Trails.Add(auditEntry.ToAuditTrail());

        return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
    }

    private Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries,
        CancellationToken cancellationToken = new())
    {
        if (trailEntries == null || trailEntries.Count == 0) return Task.CompletedTask;

        foreach (var entry in trailEntries)
        {
            foreach (var prop in entry.TemporaryProperties)
                if (prop.Metadata.IsPrimaryKey())
                    entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                else
                    entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;

            Trails.Add(entry.ToAuditTrail());
        }

        return SaveChangesAsync(cancellationToken);
    }
}