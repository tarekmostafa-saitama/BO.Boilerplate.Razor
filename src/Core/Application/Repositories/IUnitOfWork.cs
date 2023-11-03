using Shared.ServiceContracts;

namespace Application.Repositories;

public interface IUnitOfWork : IDisposable, IScopedService
{
    public ITrailsRepository TrailsRepository { get; }
    public ITenantsRepository TenantsRepository { get; }

    Task<int> CommitAsync();
}