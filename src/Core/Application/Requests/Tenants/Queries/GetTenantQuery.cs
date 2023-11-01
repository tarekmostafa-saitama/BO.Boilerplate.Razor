using Application.Repositories;
using Application.Requests.Tenants.Models;
using Mapster;
using MediatR;

namespace Application.Requests.Tenants.Queries;

public class GetTenantQuery: IRequest<TenantVm>
{
    public Guid TenantId { get; }

    public GetTenantQuery(Guid tenantId)
    {
        TenantId = tenantId;
    }
}

internal class GetTenantQueryHandler : IRequestHandler<GetTenantQuery, TenantVm>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTenantQueryHandler(IUnitOfWork  unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<TenantVm> Handle(GetTenantQuery request, CancellationToken cancellationToken)
    {
        var returnedResult =await  _unitOfWork.TenantsRepository.GetSingleAsync(x => x.Id == request.TenantId, false);

        return returnedResult.Adapt<TenantVm>(); 
    }
}
