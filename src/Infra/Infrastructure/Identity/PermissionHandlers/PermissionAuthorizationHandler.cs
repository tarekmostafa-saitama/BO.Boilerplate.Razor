﻿using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Identity.PermissionHandlers;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User == null)
            return;

        var canAccess = context.User.Claims.Any(c =>
            c.Type == "Permission" && c.Value == requirement.Permission && c.Issuer == "LOCAL AUTHORITY");

        if (canAccess) context.Succeed(requirement);
    }
}