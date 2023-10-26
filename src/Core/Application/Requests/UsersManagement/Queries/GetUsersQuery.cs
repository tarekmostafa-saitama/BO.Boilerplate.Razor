using Application.Common.Interfaces;
using Application.Common.Models.UserModels;
using MediatR;
using Shared.Models.PaginateModels;

namespace Application.Requests.UsersManagement.Queries;

public class GetUsersQuery : IRequest<PaginateResponseModel<UserVm>>
{
    public GetUsersQuery(PaginateModel<string> paginateModel)
    {
        PaginateModel = paginateModel;
    }

    public PaginateModel<string> PaginateModel { get; set; }
}

internal class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginateResponseModel<UserVm>>
{
    private readonly IUserService _identityService;

    public GetUsersQueryHandler(IUserService identityService)
    {
        _identityService = identityService;
    }

    public async Task<PaginateResponseModel<UserVm>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
      
       
        var usersData = await _identityService.GetUsersAsync(request.PaginateModel);

        return usersData;
    }
}