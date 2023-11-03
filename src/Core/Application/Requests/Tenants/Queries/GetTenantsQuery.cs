using Application.Common.Tenants;
using Application.Repositories;
using Application.Requests.Tenants.Models;
using Application.Requests.Trails.Models;
using Application.Requests.Trails.Queries;
using Application.Specifications;
using Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Models.PaginateModels;

namespace Application.Requests.Tenants.Queries;

public class GetTenantsQuery: IRequest<List<TenantVm>>
{
    public bool IncludeDefaultTenant { get; }

    public GetTenantsQuery(bool includeDefaultTenant = false)
    {
        IncludeDefaultTenant = includeDefaultTenant;
    }
}

internal class GetTenantsQueryHandler : IRequestHandler<GetTenantsQuery, List<TenantVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TenantSettings _tenantSettings;

    public GetTenantsQueryHandler(IUnitOfWork unitOfWork, IOptions<TenantSettings> tenantSettings)
    {
        _unitOfWork = unitOfWork;
        _tenantSettings = tenantSettings.Value;
    }

    public async Task<List<TenantVm>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var tenants = await _unitOfWork.TenantsRepository.GetAllAsync(false);
        var tenantsAsList = tenants.ToList();
        if (request.IncludeDefaultTenant)
        {
            tenantsAsList.Add(new Tenant()
            {
                Name = _tenantSettings.Default.Name,
                Id = _tenantSettings.Default.Id,
                ConnectionString = _tenantSettings.Default.ConnectionString,
                DbProvider = _tenantSettings.Default.DbProvider,
                UseSharedDb = _tenantSettings.Default.UseSharedDb
            });
        }
        return tenantsAsList.Adapt<List<TenantVm>>(); 

    }
}