﻿using Domain.Common;
using Domain.Enums;

namespace Application.Requests.Tenants.Models;

public class TenantVm : BaseAuditableEntity<Guid>
{
    public string Name { get; set; }
    public bool UseSharedDb { get; set; }

    public string ConnectionString { get; set; }
    public TenantDbProvider DbProvider { get; set; }
}