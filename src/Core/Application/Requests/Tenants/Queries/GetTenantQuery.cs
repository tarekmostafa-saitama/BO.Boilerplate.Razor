using Application.Requests.Tenants.Models;
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