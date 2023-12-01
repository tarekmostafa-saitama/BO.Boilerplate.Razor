using Domain.Common;
using Microsoft.AspNetCore.Identity;
using Shared.Contracts;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser, ISoftDelete, IBaseAuditableEntity, IMustHaveTenant
{
    public string FullName { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public string LastModifiedBy { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }
    public string DeletedBy { get; set; }
    public Guid TenantId { get; set; }
    public bool IsLockedDown { get; set; }
}