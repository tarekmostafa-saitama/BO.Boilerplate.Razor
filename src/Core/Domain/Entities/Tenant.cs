using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Tenant: BaseAuditableEntity<Guid>
{
    public string Name { get; set; }

    public bool UseSharedDb { get; set; }

    public string ConnectionString { get; set; }
    public TenantDbProvider DbProvider { get; set; }
}