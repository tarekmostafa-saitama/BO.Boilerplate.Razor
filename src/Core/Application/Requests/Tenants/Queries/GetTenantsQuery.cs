using Application.Repositories;
using Application.Requests.Tenants.Models;
using Application.Requests.Trails.Models;
using Application.Requests.Trails.Queries;
using Application.Specifications;
using Domain.Entities;
using Mapster;
using MediatR;
using Shared.Models.PaginateModels;

namespace Application.Requests.Tenants.Queries;

public class GetTenantsQuery: IRequest<List<TenantVm>>
{
    
}

internal class GetTenantsQueryHandler : IRequestHandler<GetTenantsQuery, List<TenantVm>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTenantsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TenantVm>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var tenants = await _unitOfWork.TenantsRepository.GetAllAsync();
        return tenants.Adapt<List<TenantVm>>(); 

    }
}