using Application.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<ITrailsRepository> _trailsRepository; 
    private readonly Lazy<ITenantsRepository> _tenantsRepository; 
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _trailsRepository = new Lazy<ITrailsRepository>(() => new TrailsRepository(_context));
        _tenantsRepository = new Lazy<ITenantsRepository>(() => new TenantsRepository(_context));
    }

    public ITrailsRepository TrailsRepository => _trailsRepository.Value;
    public ITenantsRepository TenantsRepository => _tenantsRepository.Value;

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}