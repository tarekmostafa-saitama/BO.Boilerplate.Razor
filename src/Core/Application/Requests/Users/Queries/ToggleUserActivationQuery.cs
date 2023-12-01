using Application.Common.Interfaces;
using MediatR;

namespace Application.Requests.Users.Queries;

public record ToggleUserActivationQuery(string UserId) :  IRequest<IResponse<Unit>>;

internal sealed class ToggleUserActivationQueryHandler : IRequestHandler<ToggleUserActivationQuery, IResponse<Unit>>
{

    private readonly IAuthService _authService;


    public ToggleUserActivationQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }


    public async Task<IResponse<Unit>> Handle(ToggleUserActivationQuery request, CancellationToken cancellationToken)
    {
        return await _authService.ToggleUserActivation(request.UserId);
    }
}
