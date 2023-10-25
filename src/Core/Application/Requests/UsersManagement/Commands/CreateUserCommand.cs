using Application.Common.Interfaces;
using Application.Common.Models.UserModels;
using MediatR;

namespace Application.Requests.UsersManagement.Commands;

public class CreateUserCommand : IRequest<IResponse<string>>
{
    public CreateUserCommand(CreateUserVm userVm)
    {
        UserVm = userVm;
    }

    public CreateUserVm UserVm { get; set; }
}

internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IResponse<string>>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IResponse<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.CreateUserAsync(request.UserVm);
    }
}