using Application.Repositories;
using Application.Requests.Trails.Models;
using Mapster;
using MediatR;

namespace Application.Requests.Trails.Queries;

public class GetTrailQuery:IRequest<TrailVm>
{
    public int TrailId { get; }

    public GetTrailQuery(int trailId )
    {
        TrailId = trailId;
    }
}

public class GetTrailQueryHandler : IRequestHandler<GetTrailQuery ,TrailVm>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTrailQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<TrailVm> Handle(GetTrailQuery request, CancellationToken cancellationToken)
    {
        var result =await  _unitOfWork.TrailsRepository.GetSingleAsync(x => x.Id == request.TrailId);

        return result.Adapt<TrailVm>(); 
    }
}
