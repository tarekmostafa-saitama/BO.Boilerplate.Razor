using Application.Common.Interfaces;
using MediatR;

namespace Application.Requests.UsersManagement.Commands;

public class DeleteUserCommand : IRequest<Unit>
{
    public DeleteUserCommand(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }
}

internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserService _userService;

    public DeleteUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userService.DeleteUserAsync(request.UserId);
        return Unit.Value;
    }
}