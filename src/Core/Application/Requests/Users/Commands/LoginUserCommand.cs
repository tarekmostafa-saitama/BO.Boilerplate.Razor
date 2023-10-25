using Application.Common.Interfaces;
using Application.Requests.Users.Models;
using MediatR;

namespace Application.Requests.Users.Commands;

public record LoginUserCommand(LoginUserRequest LoginUserRequest) : IRequest<IResponse<Unit>>;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IResponse<Unit>>
{
    private readonly IAuthService _authService;


    public LoginUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IResponse<Unit>> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _authService.SignInUserASync(request.LoginUserRequest);
    }
}