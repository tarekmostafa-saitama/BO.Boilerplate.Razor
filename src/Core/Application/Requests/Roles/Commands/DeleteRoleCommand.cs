using Application.Common.Interfaces;
using MediatR;

namespace Application.Requests.Roles.Commands;

public class DeleteRoleCommand : IRequest<Unit>
{
    public DeleteRoleCommand(string roleId)
    {
        RoleId = roleId;
    }

    public string RoleId { get; }
}

internal class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
{
    private readonly IRoleService _roleService;

    public DeleteRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        await _roleService.DeleteRoleAsync(request.RoleId);
        return Unit.Value;
    }
}