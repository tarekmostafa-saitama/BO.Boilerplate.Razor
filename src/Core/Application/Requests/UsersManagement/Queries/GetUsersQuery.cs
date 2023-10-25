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

    //TODO: Need to be handled to support ordering and real pagination
    public async Task<PaginateResponseModel<UserVm>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PaginateModel.Request.Length != null
            ? Convert.ToInt32(request.PaginateModel.Request.Length)
            : 0;
        var skip = request.PaginateModel.Request.Start != null
            ? Convert.ToInt32(request.PaginateModel.Request.Start)
            : 0;
        var recordsTotal = 0;
        var usersData = await _identityService.GetUsersAsync(new PaginateModel<string>());


        throw new NotImplementedException();
    }
}