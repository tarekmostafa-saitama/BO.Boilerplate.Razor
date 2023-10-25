using Application.Common.Interfaces;
using MediatR;

namespace Application.Requests.Users.Commands;

public record LogOutCommand : IRequest<IResponse<Unit>>;

internal sealed class LogOutCommandHandler : IRequestHandler<LogOutCommand, IResponse<Unit>>
{
    private readonly IAuthService _authService;


    public LogOutCommandHandler(
        IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IResponse<Unit>> Handle(LogOutCommand request, CancellationToken cancellationToken)
    {
        return await _authService.SignOutUserASync();
    }
}