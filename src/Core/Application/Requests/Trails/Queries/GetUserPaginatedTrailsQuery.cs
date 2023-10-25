using Application.Repositories;
using Application.Requests.Trails.Models;
using Application.Specifications;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Requests.Trails.Queries;

public record GetUserPaginatedTrailsQuery(string UserId, int Page, int PageSize) : IRequest<List<TrailVm>>;

public class GetUserPaginatedTrailsQueryHandler : IRequestHandler<GetUserPaginatedTrailsQuery, List<TrailVm>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserPaginatedTrailsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TrailVm>> Handle(GetUserPaginatedTrailsQuery request, CancellationToken cancellationToken)
    {
        var spec = new Specification<Trail>(x => x.UserId == request.UserId)
            .OrderByDesc(x => x.DateTime)
            .ApplyPaging(request.PageSize * (request.Page - 1), request.PageSize);

        var trails = await _unitOfWork.TrailsRepository.GetAsync(spec, false);
        return trails.Adapt<List<TrailVm>>();
    }
}