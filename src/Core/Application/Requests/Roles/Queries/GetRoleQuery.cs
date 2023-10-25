using Application.Common.Interfaces;
using Application.Requests.Roles.Models;
using MediatR;

namespace Application.Requests.Roles.Queries;

public class GetRoleQuery : IRequest<RoleVm>
{
    public GetRoleQuery(string roleId, bool includePermissions)
    {
        RoleId = roleId;
        IncludePermissions = includePermissions;
    }

    public string RoleId { get; }
    public bool IncludePermissions { get; }
}

internal class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, RoleVm>
{
    private readonly IRoleService _roleService;

    public GetRoleQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<RoleVm> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleAsync(request.RoleId);
        if (request.IncludePermissions)
            role.Permissions = await _roleService.GetRolePermissionsAsync(role.Id);
        return role;
    }
}