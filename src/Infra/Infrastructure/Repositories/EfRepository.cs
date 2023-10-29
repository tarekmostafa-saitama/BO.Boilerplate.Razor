using System.Linq.Expressions;
using Application.Repositories;
using Application.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _entities;

    public EfRepository(DbContext context)
    {
        _entities = context.Set<TEntity>();
    }


    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> criteria, bool trackChanges = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _entities;

        if (!trackChanges)
            query = query.AsNoTracking();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(criteria);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _entities;

        if (!trackChanges)
            query = query.AsNoTracking();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification, bool trackChanges = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _entities;

        if (!trackChanges)
            query = query.AsNoTracking();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

      

        query = query.ApplySpecification(specification);

        return await query.ToListAsync();
    }

    public void Add(TEntity entity)
    {
        _entities.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _entities.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public void RemoveRange(Expression<Func<TEntity, bool>> criteria = null)
    {
        var query = _entities.AsQueryable();
        if (criteria != null) query = query.Where(criteria);
        query.ExecuteDelete();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria = null)
    {
        if (criteria != null) return await _entities.CountAsync(criteria);
        return await _entities.CountAsync();
    }

    public async Task<int> CountAsync(ISpecification<TEntity> specification)
    {
        return await _entities.ApplySpecification(specification).CountAsync();
    }
}