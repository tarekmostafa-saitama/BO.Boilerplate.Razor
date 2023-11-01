using Application.Repositories;
using Application.Requests.Tenants.Models;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Requests.Tenants.Commands;

public class SetTenantCommand:IRequest<Unit>
{
    public TenantVm TenantVm { get; set;  }

    public SetTenantCommand(TenantVm tenantVm)
    {
        TenantVm = tenantVm;
    }
}


internal class SetTenantCommandHandler : IRequestHandler<SetTenantCommand ,Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetTenantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(SetTenantCommand request, CancellationToken cancellationToken)
    {
        if (request.TenantVm.Id == default)
        {
            request.TenantVm.Id= Guid.NewGuid();
            request.TenantVm.UseSharedDb = true;
            _unitOfWork.TenantsRepository.Add(request.TenantVm.Adapt<Tenant>());

        }
        else
        {
            _unitOfWork.TenantsRepository.Update(request.TenantVm.Adapt<Tenant>());

        }

        await _unitOfWork.CommitAsync();
        return Unit.Value ; 
    }
}
