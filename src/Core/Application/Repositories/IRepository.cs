using System.Linq.Expressions;
using Application.Specifications;

namespace Application.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetSingleAsync(
        Expression<Func<TEntity, bool>> criteria,
        bool trackChanges = true,
        params Expression<Func<TEntity, object>>[] includes);


    Task<IEnumerable<TEntity>> GetAllAsync(
        bool trackChanges = true,
        params Expression<Func<TEntity, object>>[] includes);


    Task<IEnumerable<TEntity>> GetAllAsync(
        ISpecification<TEntity> specification,
        bool trackChanges = true,
        params Expression<Func<TEntity, object>>[] includes);


    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);

    void RemoveRange(
        Expression<Func<TEntity, bool>> criteria);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria = null);


    Task<int> CountAsync(
        ISpecification<TEntity> specification
      );
}