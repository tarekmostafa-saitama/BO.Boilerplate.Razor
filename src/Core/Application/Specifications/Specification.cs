using System.Linq;
using System.Linq.Expressions;
using Shared.Extensions;

namespace Application.Specifications;

public class Specification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get;}
    public List<Expression<Func<T, object>>> Includes { get; } = new();


    public Specification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }
    public Specification<T> ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
        return this;
    }


    public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; private set; }

    public Specification<T> ApplyOrderByAsc(string orderBy)
    {
        OrderBy = query => query.OrderBy(orderBy);
        return this;
    }

    public Specification<T> ApplyOrderByDesc(string orderBy)
    {
        OrderBy = query => query.OrderByDescending(orderBy);
        return this;
    }
    public Specification<T> ApplyOrderByAsc(Expression<Func<T, object>> orderBy)
    {
        OrderBy = query => query.OrderBy(orderBy);
        return this;
    }

    public Specification<T> ApplyOrderByDesc(Expression<Func<T, object>> orderBy)
    {
        OrderBy = query => query.OrderByDescending(orderBy);
        return this;
    }




    public List<Expression<Func<T, bool>>> AdditionalCriteria { get; private set; } = new();

    public Specification<T> ApplyCriteria(Expression<Func<T, bool>> criteria)
    {
        AdditionalCriteria.Add(criteria);
        return this;
    }




}

public static class SpecificationExtensions
{
    public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> spec)
    {
        // Apply Order Section
        var queryableResultWithOrdering = spec.OrderBy != null
            ? spec.OrderBy(query)
            : query;

        // Apply Paginate Section
        var queryableResultWithPaginating = spec.IsPagingEnabled
            ? queryableResultWithOrdering.Skip(spec.Skip).Take(spec.Take)
            : queryableResultWithOrdering;

        // Apply Base Where Section
        var queryableResultWithBaseWhere = spec.Criteria != null 
            ? queryableResultWithPaginating.Where(spec.Criteria)
            : queryableResultWithPaginating;

        // Apply Base Where Section
        var queryableResultWithExtraWhere = spec.AdditionalCriteria.Any()
            ? spec.AdditionalCriteria.Aggregate(queryableResultWithBaseWhere, 
                (current, whereCondition) => current.Where(whereCondition))
            : queryableResultWithBaseWhere;



        return queryableResultWithExtraWhere;
    }
}