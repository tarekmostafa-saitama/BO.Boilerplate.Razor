using Application.Repositories;
using Application.Requests.Trails.Models;
using Application.Specifications;
using Domain.Entities;
using Mapster;
using MediatR;
using Shared.Models.PaginateModels;
using System.Reflection;

namespace Application.Requests.Trails.Queries;

public record GetTrailsQuery(PaginateModel<string> model) : IRequest<PaginateResponseModel<TrailVm>>;

internal class GetTrailsQueryHandler : IRequestHandler<GetTrailsQuery, PaginateResponseModel<TrailVm>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTrailsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginateResponseModel<TrailVm>> Handle(GetTrailsQuery request, CancellationToken cancellationToken)
    {
        var pageSize = Convert.ToInt32(request.model.Request.Length);
        var skip = Convert.ToInt32(request.model.Request.Start);

        var spec = new Specification<Trail>(string.IsNullOrEmpty(request.model.FilteredById)
            ? null
            : x => x.UserId == request.model.FilteredById);

        if (!string.IsNullOrWhiteSpace(request.model.Request.SortColumn))
            spec = request.model.Request.SortColumnDirection == "asc"
                ? spec.ApplyOrderByAsc(request.model.Request.SortColumn)
                : spec.ApplyOrderByDesc(request.model.Request.SortColumn);
        else
            spec = spec.ApplyOrderByDesc(x => x.DateTime);


        var recordsTotal = await _unitOfWork.TrailsRepository.CountAsync(spec);

        spec = spec.ApplyPaging(skip, pageSize);
        var trails = await _unitOfWork.TrailsRepository.GetAllAsync(spec, false);
        return new PaginateResponseModel<TrailVm>()
        {
            Data = trails.Adapt<List<TrailVm>>(),
            Draw = request.model.Request.Draw,
            RecordsFiltered = recordsTotal,
            RecordsTotal = recordsTotal,
        };
     
    }
}