using System.Linq.Expressions;

namespace Application.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}