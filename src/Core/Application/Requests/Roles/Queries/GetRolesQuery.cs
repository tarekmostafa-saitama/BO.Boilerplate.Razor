using Application.Common.Interfaces;
using Application.Requests.Roles.Models;
using MediatR;

namespace Application.Requests.Roles.Queries;

public class GetRolesQuery : IRequest<List<RoleVm>>
{
}

internal class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RoleVm>>
{
    private readonly IRoleService _roleService;

    public GetRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<List<RoleVm>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _roleService.GetRolesAsync();
    }
}