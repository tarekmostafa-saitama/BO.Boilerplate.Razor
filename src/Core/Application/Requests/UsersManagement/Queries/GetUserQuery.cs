using Application.Common.Interfaces;
using Application.Common.Models.UserModels;
using Application.Requests.Roles.Models;
using MediatR;

namespace Application.Requests.UsersManagement.Queries;

public class GetUserQuery : IRequest<UserVm>
{
    public GetUserQuery(string userId, bool includeRoles = false)
    {
        UserId = userId;
        IncludeRoles = includeRoles;
    }

    public string UserId { get; }
    public bool IncludeRoles { get; }
}

internal class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserVm>
{
    private readonly IUserService _identityService;
    private readonly IRoleService _roleService;

    public GetUserQueryHandler(IUserService identityService, IRoleService roleService)
    {
        _identityService = identityService;
        _roleService = roleService;
    }

    public async Task<UserVm> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var roles = new List<RoleVm>();

        var user = await _identityService.GetUserByIdAsync(request.UserId);
        if (request.IncludeRoles) roles = await _roleService.GetRolesAsync(request.UserId);

        var userData = new UserVm
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            RoleVms = roles
        };

        return userData;
    }
}