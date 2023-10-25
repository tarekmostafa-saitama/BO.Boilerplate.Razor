using Application.Common.Interfaces;
using Application.Requests.Roles.Models;
using MediatR;

namespace Application.Requests.Roles.Commands;

public class CreateRoleCommand : IRequest<IResponse<string>>
{
    public CreateRoleCommand(RoleVm roleVm)
    {
        RoleVm = roleVm;
    }

    public RoleVm RoleVm { get; set; }
}

internal class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, IResponse<string>>
{
    private readonly IRoleService _roleService;

    public CreateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<IResponse<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var createRoleResult = await _roleService.CreateRoleAsync(request.RoleVm.Name);
        if (createRoleResult.Succeeded)
            await _roleService.ReplacePermissionsToRoleAsync(createRoleResult.Data, request.RoleVm.Permissions);


        return createRoleResult;
    }
}