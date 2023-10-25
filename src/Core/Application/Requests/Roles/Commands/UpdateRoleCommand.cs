using Application.Common.Interfaces;
using Application.Requests.Roles.Models;
using MediatR;

namespace Application.Requests.Roles.Commands;

public class UpdateRoleCommand : IRequest<IResponse<string>>
{
    public UpdateRoleCommand(RoleVm roleVm)
    {
        RoleVm = roleVm;
    }

    public RoleVm RoleVm { get; }
}

internal class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, IResponse<string>>
{
    private readonly IRoleService _roleService;

    public UpdateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<IResponse<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var createRoleResult = await _roleService.UpdateRoleAsync(request.RoleVm.Id, request.RoleVm.Name);
        if (createRoleResult.Succeeded)
            await _roleService.ReplacePermissionsToRoleAsync(createRoleResult.Data, request.RoleVm.Permissions);


        return createRoleResult;
    }
}