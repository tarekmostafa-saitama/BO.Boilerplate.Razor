using Microsoft.AspNetCore.Authorization;
using Shared.Permissions;

namespace Infrastructure.Identity.PermissionHandlers;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource)
    {
        Policy = Permission.NameFor(action, resource);
    }
}