using Application.Common.Interfaces;
using Application.Common.Models.UserModels;
using MediatR;

namespace Application.Requests.UsersManagement.Commands;

public class UpdateUserCommand:IRequest<IResponse<string>>
{
    public UpdateUserCommand(UpdateUserVm userVm)
    {
        UserVm = userVm;
    }

    public UpdateUserVm UserVm { get; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand ,IResponse<string>>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<IResponse<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updateUserResult = await _userService.UpdateUserAsync(request.UserVm);
        if (updateUserResult.Succeeded)
            await _userService.ReplaceRolesToUserAsync(request.UserVm.Id, request.UserVm.RoleVms.Select(x=>x.Name).ToList());

        return updateUserResult; 
    }
}
