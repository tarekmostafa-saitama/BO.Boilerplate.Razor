using Application.Common.Interfaces;
using Application.Repositories;
using MediatR;

namespace Application.Requests.Tenants.Commands;

public class DeleteTenantCommand :IRequest<Unit>
    {
    public DeleteTenantCommand(Guid tenantId)
    {
        TenantId = tenantId;
    }

    public Guid TenantId { get; }
}

internal class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTenantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.TenantsRepository.GetSingleAsync(x => x.Id == request.TenantId);
        _unitOfWork.TenantsRepository.Remove(entity);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}