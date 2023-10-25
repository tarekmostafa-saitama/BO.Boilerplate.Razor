using Shared.ServiceContracts;

namespace Application.Repositories;

public interface IUnitOfWork : IDisposable, IScopedService
{
    public ITrailsRepository TrailsRepository { get; }

    Task<int> CommitAsync();
}