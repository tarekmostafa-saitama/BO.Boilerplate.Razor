using System.Linq.Expressions;

namespace Application.Specifications;

public interface ISpecification<T>
{
    // Order Section
    Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; }


    // Where Section
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, bool>>> AdditionalCriteria { get; }



    // Paginate Section
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}