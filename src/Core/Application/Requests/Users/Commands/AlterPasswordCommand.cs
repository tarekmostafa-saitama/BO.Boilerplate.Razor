using Application.Common.Interfaces;
using Application.Requests.Users.Models;
using MediatR;

namespace Application.Requests.Users.Commands;

public record AlterPasswordCommand(AlterPasswordVm AlterPasswordVm) : IRequest<IResponse<Unit>>;


internal sealed class AlterPasswordCommandHandler : IRequestHandler<AlterPasswordCommand, IResponse<Unit>>
{
    private readonly IAuthService _authService;


    public AlterPasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IResponse<Unit>> Handle(AlterPasswordCommand request,
        CancellationToken cancellationToken)
    {
        return await _authService.AlterPasswordAsync(request.AlterPasswordVm);
    }
}