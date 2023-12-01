using Application.Common.Interfaces;
using Application.Requests.Users.Models;
using MediatR;

namespace Application.Requests.Users.Commands;

public record ChangePasswordCommand(ChangePasswordVm ChangePasswordVm) : IRequest<IResponse<Unit>>;


internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, IResponse<Unit>>
{
    private readonly IAuthService _authService;


    public ChangePasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IResponse<Unit>> Handle(ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        return await _authService.ChangePasswordAsync(request.ChangePasswordVm);
    }
}