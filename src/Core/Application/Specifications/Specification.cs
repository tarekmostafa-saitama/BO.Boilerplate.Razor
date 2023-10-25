using System.Linq.Expressions;

namespace Application.Specifications;

public class Specification<T> : ISpecification<T>
{
    public Specification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>> Criteria { get; }
    public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    public Specification<T> OrderByAsc(Expression<Func<T, object>> orderBy)
    {
        OrderBy = query => query.OrderBy(orderBy);
        return this;
    }

    public Specification<T> OrderByDesc(Expression<Func<T, object>> orderBy)
    {
        OrderBy = query => query.OrderByDescending(orderBy);
        return this;
    }

    public Specification<T> ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
        return this;
    }
}

public static class SpecificationExtensions
{
    public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> spec)
    {
        var queryableResultWithOrdering = spec.OrderBy != null
            ? spec.OrderBy(query)
            : query;

        var finalQuery = spec.IsPagingEnabled
            ? queryableResultWithOrdering.Skip(spec.Skip).Take(spec.Take)
            : queryableResultWithOrdering;

        return finalQuery.Where(spec.Criteria);
    }
}