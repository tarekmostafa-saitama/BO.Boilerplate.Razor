using Application.Requests.Tenants.Models;
using MediatR;

namespace Application.Requests.Tenants.Queries;

public class GetTenantsQuery: IRequest<List<TenantVm>>
{
    
}