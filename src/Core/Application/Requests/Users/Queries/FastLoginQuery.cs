using Application.Common.Interfaces;
using MediatR;

namespace Application.Requests.Users.Queries;

public record FastLoginQuery(string UserId) : IRequest<IResponse<Unit>>;


internal sealed class FastLoginQueryHandler : IRequestHandler< FastLoginQuery , IResponse<Unit>>
{

    private readonly IAuthService _authService;


    public FastLoginQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }


    public async Task<IResponse<Unit>> Handle(FastLoginQuery request, CancellationToken cancellationToken)
    {
        return await _authService.FastLogin(request.UserId); 
    }
}
